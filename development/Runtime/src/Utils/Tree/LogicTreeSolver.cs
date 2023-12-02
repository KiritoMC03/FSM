using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Runtime.Utils;

namespace FSM.Runtime.Utils
{
    public sealed class LogicTreeSolver
    {
        private readonly Stack<LogicNode> logicChain;

        public LogicTreeSolver(int initialStackCapacity = 32)
        {
            logicChain = new Stack<LogicNode>(initialStackCapacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Solve(LogicNode root) // ToDo: tests
        {
            logicChain.Push(root);
            bool value = false;
            // Simple binary tree traversal. We need to convert every node to simple true/false state
            while (logicChain.Count > 0) 
            {
                LogicNode current = logicChain.Peek();
                if (current.IsLeaf)
                {
                    value = current.Value;
                    logicChain.Pop();
                }
                else
                {
                    switch (current.Operator)
                    {
                        case Operator.And:
                            if (current.Left.IsLeaf && current.Right.IsLeaf)
                            {
                                current.Value = current.Left.Value && current.Right.Value;
                                current.IsLeaf = true;
                            }
                            else if (current.Left.IsLeaf) logicChain.Push(current.Right);
                            else logicChain.Push(current.Left);
                            break;
                        case Operator.Or:
                            if (current.Left.IsLeaf && current.Right.IsLeaf)
                            {
                                current.Value = current.Left.Value || current.Right.Value;
                                current.IsLeaf = true;
                            }
                            else if (current.Left.IsLeaf) logicChain.Push(current.Right);
                            else logicChain.Push(current.Left);
                            break;
                        case Operator.Not:
                            if (current.Left.IsLeaf)
                            {
                                current.Value = !current.Left.Value;
                                current.IsLeaf = true;
                            }
                            else logicChain.Push(current.Left);
                            break;
                        default:
                            Logger.LogError(Messages.InvalidLogicOperatorWithExitError); // ToDo: Maybe not the best 
                            return false;
                    }
                }
            }

            return value;
        }
    }
}