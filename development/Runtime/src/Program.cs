using System;
using System.Diagnostics;

namespace FSM.Runtime
{
    public class Program
    {
        public static void Main()
        {
            DecisionSolver solver = new DecisionSolver();
            ILogicLayoutNode root = new AndLayoutNode(Create2(), Create2());
            
            var a = solver.Solve(new DecisionsBlock(root));
            Console.WriteLine($"Result: {a}");
        }

        private static ILogicLayoutNode Create2()
        {
            return new AndLayoutNode(Create(), Create());
        }
        
        private static ILogicLayoutNode Create()
        {
            return new AndLayoutNode(
                new NotLayoutNode(new BaseLogicGateNode(default, new FalseDecision())),
                new OrLayoutNode(
                    new BaseLogicGateNode(default, new FalseDecision()),
                    new BaseLogicGateNode(default, new TrueDecision())));
        }
    }
}