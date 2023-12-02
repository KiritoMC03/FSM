using System.Collections.Generic;
using FSM.Runtime.Utils;

namespace FSM.Runtime
{
    public class DecisionSolver
    {
        private LogicTreeSolver logicTreeSolver = new LogicTreeSolver();
        private readonly GenericPool<LogicNode> logicNodesPool = new GenericPool<LogicNode>(CreateNew);
        private readonly Queue<(ILogicLayoutNode self, ILogicLayoutNode left, ILogicLayoutNode right)> originalQueue =
                     new Queue<(ILogicLayoutNode self, ILogicLayoutNode left, ILogicLayoutNode right)>();
        private readonly Queue<LogicNode> copyQueue = new Queue<LogicNode>();

        public bool Solve(DecisionsBlock decisionsBlock)
        {
            return logicTreeSolver.Solve(CreateLogicTree(decisionsBlock));
        }

        private static LogicNode CreateNew() => new LogicNode();

        private (LogicNode result, ILogicLayoutNode left, ILogicLayoutNode right) GetNodeFor(ILogicLayoutNode layoutNode)
        {
            LogicNode result = logicNodesPool.Pop();
            switch (layoutNode)
            {
                case NotLayoutNode:
                    result.Operator = Operator.Not;
                    return (result, layoutNode.Input, null);
                case OrLayoutNode orLayoutNode:
                    result.Operator = Operator.Or;
                    return (result, orLayoutNode.Input, orLayoutNode.Right);
                case AndLayoutNode andLayoutNode:
                    result.Operator = Operator.And;
                    return (result, andLayoutNode.Input, andLayoutNode.Right);
                default:
                    result.Value = ((IDecision)layoutNode.LogicObject).Decide();
                    return (result, null, null);
            }
        }

        private LogicNode CreateLogicTree(DecisionsBlock decisionsBlock)
        {
            (LogicNode newNode, ILogicLayoutNode rootLeft, ILogicLayoutNode rootRight) = GetNodeFor(decisionsBlock.Node);
            originalQueue.Enqueue((decisionsBlock.Node, rootLeft, rootRight));
            copyQueue.Enqueue(newNode);

            while (originalQueue.Count > 0)
            {
                (ILogicLayoutNode original, ILogicLayoutNode originalLeft, ILogicLayoutNode originalRight) = originalQueue.Dequeue();
                LogicNode copyNode = copyQueue.Dequeue();

                if (originalLeft != null)
                {
                    (LogicNode result, ILogicLayoutNode left, ILogicLayoutNode right) = GetNodeFor(originalLeft);
                    originalQueue.Enqueue((originalLeft, left, right));
                    result.IsLeaf = left == null && right == null;
                    copyNode.Left = result;
                    copyQueue.Enqueue(result);
                }

                if (originalRight != null)
                {
                    (LogicNode result, ILogicLayoutNode left, ILogicLayoutNode right) = GetNodeFor(originalRight);
                    originalQueue.Enqueue((originalRight, left, right));
                    result.IsLeaf = left == null && right == null;
                    copyNode.Right = result;
                    copyQueue.Enqueue(result);
                }
            }

            originalQueue.Clear();
            copyQueue.Clear();
            return newNode;
        }
    }
}