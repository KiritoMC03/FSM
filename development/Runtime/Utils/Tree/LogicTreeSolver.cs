using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FSM.Runtime.Utils
{
    public sealed class LogicTreeSolver
    {
        private readonly Stack<LogicNode> stack;

        public LogicTreeSolver(int initialStackCapacity = 32)
        {
            stack = new Stack<LogicNode>(initialStackCapacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Solve(LogicNode root) // ToDo: tests
        {
            stack.Push(root);
            bool value = false;
            while (stack.Count > 0)
            {
                LogicNode current = stack.Peek();
                if (current.IsLeaf)
                {
                    value = current.Value;
                    stack.Pop();
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
                            else if (current.Left.IsLeaf) stack.Push(current.Right);
                            else stack.Push(current.Left);
                            break;
                        case Operator.Or:
                            if (current.Left.IsLeaf && current.Right.IsLeaf)
                            {
                                current.Value = current.Left.Value || current.Right.Value;
                                current.IsLeaf = true;
                            }
                            else if (current.Left.IsLeaf) stack.Push(current.Right);
                            else stack.Push(current.Left);
                            break;
                        case Operator.Not:
                            if (current.Left.IsLeaf)
                            {
                                current.Value = !current.Left.Value;
                                current.IsLeaf = true;
                            }
                            else stack.Push(current.Left);
                            break;
                    }
                }
            }

            return value;
        }
    }
}