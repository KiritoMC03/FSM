using System.Collections.Generic;
using FSM.Runtime.Utils;

namespace FSM.Runtime
{
    public class ConditionSolver
    {
        private readonly LogicTreeSolver logicTreeSolver = new LogicTreeSolver();
        private readonly GenericPool<LogicNode> logicNodesPool = new GenericPool<LogicNode>(NewLogicNode);
        private readonly Queue<(ILogicLayoutNode self, ILogicLayoutNode left, ILogicLayoutNode right)> originalQueue =
                     new Queue<(ILogicLayoutNode self, ILogicLayoutNode left, ILogicLayoutNode right)>(16);
        private readonly Queue<LogicNode> copyQueue = new Queue<LogicNode>(16);
        private readonly Stack<LogicNode> clearStack = new Stack<LogicNode>(4); 

        public bool Solve(ILogicLayoutNode logicLayoutNode)
        {
            // Creating of temp logic tree to solve him. 
            LogicNode tree = CreateLogicTree(logicLayoutNode);
            bool result = logicTreeSolver.Solve(tree);

            clearStack.Push(tree);
            while (clearStack.Count > 0)
            {
                LogicNode current = clearStack.Pop();
                if (current.Right != null) clearStack.Push(current.Right);
                if (current.Left != null) clearStack.Push(current.Left);
                logicNodesPool.Push(current);
            }

            return result;
        }

        private LogicNode CreateLogicTree(ILogicLayoutNode logicLayoutNode)
        {
            (LogicNode newNode, ILogicLayoutNode rootLeft, ILogicLayoutNode rootRight) = GetNodeFor(logicLayoutNode);
            originalQueue.Enqueue((logicLayoutNode, rootLeft, rootRight));
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
                    result.Value = ((ICondition)layoutNode.LogicObject).Decide();
                    return (result, null, null);
            }
        }

        private static LogicNode NewLogicNode() => new LogicNode();
    }
}