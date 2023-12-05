using System.Collections.Generic;
using FSM.Runtime.Utils;

namespace FSM.Runtime
{
    public class ConditionSolver
    {
        private readonly LogicTreeSolver logicTreeSolver = new LogicTreeSolver();
        private readonly GenericPool<LogicOperationNode> logicNodesPool = new GenericPool<LogicOperationNode>(NewLogicNode);
        private readonly Queue<(IConditionalLayoutNode self, IConditionalLayoutNode left, IConditionalLayoutNode right)> originalQueue =
                     new Queue<(IConditionalLayoutNode self, IConditionalLayoutNode left, IConditionalLayoutNode right)>(16);
        private readonly Queue<LogicOperationNode> copyQueue = new Queue<LogicOperationNode>(16);
        private readonly Stack<LogicOperationNode> clearStack = new Stack<LogicOperationNode>(4);

        public bool Solve(IConditionalLayoutNode layoutNode)
        {
            // Creating of temp logic tree to solve him. 
            LogicOperationNode tree = CreateLogicTree(layoutNode);
            bool result = logicTreeSolver.Solve(tree);

            clearStack.Push(tree);
            while (clearStack.Count > 0)
            {
                LogicOperationNode current = clearStack.Pop();
                if (current.Right != null) clearStack.Push(current.Right);
                if (current.Left != null) clearStack.Push(current.Left);
                logicNodesPool.Push(current);
            }

            return result;
        }

        private LogicOperationNode CreateLogicTree(IConditionalLayoutNode layoutNode)
        {
            (LogicOperationNode newNode, IConditionalLayoutNode rootLeft, IConditionalLayoutNode rootRight) = GetNodeFor(layoutNode);
            originalQueue.Enqueue((layoutNode, rootLeft, rootRight));
            copyQueue.Enqueue(newNode);

            while (originalQueue.Count > 0)
            {
                (IConditionalLayoutNode original, IConditionalLayoutNode originalLeft,
                    IConditionalLayoutNode originalRight) = originalQueue.Dequeue();
                LogicOperationNode copyOperationNode = copyQueue.Dequeue();

                if (originalLeft != null)
                {
                    (LogicOperationNode result, IConditionalLayoutNode left, IConditionalLayoutNode right) = GetNodeFor(originalLeft);
                    originalQueue.Enqueue((originalLeft, left, right));
                    result.IsLeaf = left == null && right == null;
                    copyOperationNode.Left = result;
                    copyQueue.Enqueue(result);
                }

                if (originalRight != null)
                {
                    (LogicOperationNode result, IConditionalLayoutNode left, IConditionalLayoutNode right) = GetNodeFor(originalRight);
                    originalQueue.Enqueue((originalRight, left, right));
                    result.IsLeaf = left == null && right == null;
                    copyOperationNode.Right = result;
                    copyQueue.Enqueue(result);
                }
            }

            originalQueue.Clear();
            copyQueue.Clear();
            return newNode;
        }

        private (LogicOperationNode result, IConditionalLayoutNode left, IConditionalLayoutNode right) GetNodeFor(
            IConditionalLayoutNode layoutNode)
        {
            LogicOperationNode result = logicNodesPool.Pop();
            switch (layoutNode)
            {
                case NotLayoutNode not:
                    result.Operator = Operator.Not;
                    return (result, not.Left, null);
                case OrLayoutNode orLayoutNode:
                    result.Operator = Operator.Or;
                    return (result, orLayoutNode.Left, orLayoutNode.Right);
                case AndLayoutNode andLayoutNode:
                    result.Operator = Operator.And;
                    return (result, andLayoutNode.Left, andLayoutNode.Right);
                default:
                    result.Value = ((ConditionLayoutNode)layoutNode).LogicObject.Decide();
                    result.IsLeaf = true;
                    return (result, null, null);
            }
        }

        private static LogicOperationNode NewLogicNode() => new LogicOperationNode();
    }
}