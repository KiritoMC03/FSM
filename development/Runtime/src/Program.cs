using System;
using System.Diagnostics;

namespace FSM.Runtime
{
    public class Program
    {
        public static void Main()
        {
            ConditionSolver solver = new ConditionSolver();
            Stopwatch sw = new Stopwatch();
            ILogicLayoutNode root = new AndLayoutNode(Create2(), Create2());
            sw.Start();
            bool a = solver.Solve(root);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            
            sw = new Stopwatch();
            bool r = true;
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                r &= solver.Solve(root);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks / 100);
            
            Console.WriteLine($"Result: {a} {r}");
        }

        private static ILogicLayoutNode Create2()
        {
            return new AndLayoutNode(Create(), Create());
        }
        
        private static ILogicLayoutNode Create()
        {
            return new AndLayoutNode(
                new NotLayoutNode(new BaseLogicGateNode(default, new FalseCondition())),
                new OrLayoutNode(
                    new BaseLogicGateNode(default, new FalseCondition()),
                    new BaseLogicGateNode(default, new TrueCondition())));
        }
    }
}