using System;
using System.Diagnostics;
using System.Threading;
using FSM.Runtime.Common;

namespace FSM.Runtime
{
    public class Program
    {
        public static void Main()
        {
            ActionLayoutNode actions0 =
                new ActionLayoutNode(new LogAction("actions0_0"),
                    new ActionLayoutNode(new LogAction("actions0_1"), default));
            ActionLayoutNode actions1 =
                new ActionLayoutNode(new LogAction("actions1_0"),
                    new ActionLayoutNode(new LogAction("actions1_1"), default));
            ActionLayoutNode actions2 =
                new ActionLayoutNode(new LogAction("actions2_0"),
                    new ActionLayoutNode(new LogAction("actions2_1"), default));
            ActionLayoutNode actions3 =
                new ActionLayoutNode(new LogAction("actions3_0"),
                    new ActionLayoutNode(new LogAction("actions3_1"), default));
            StateBase state0 = new StateBase(actions0, default);
            StateBase state1 = new StateBase(actions1, default);
            StateBase state2 = new StateBase(actions2, default);
            StateBase state3 = new StateBase(actions3, default);
            BaseTransition transitionTo1 = new BaseTransition(state1, new ConditionLayoutNode(new TrueCondition()));
            BaseTransition transitionTo2 = new BaseTransition(state2, new ConditionLayoutNode(new TrueCondition()));
            BaseTransition transitionTo3 = new BaseTransition(state3, new OrLayoutNode(
                new ConditionLayoutNode(new FalseCondition()),
                new ConditionLayoutNode(new TrueCondition())));
            state0.SetTransitions(new []{transitionTo1});
            state1.SetTransitions(new []{transitionTo2});
            state2.SetTransitions(new []{transitionTo3});

            FsmAgentBase agent = new FsmAgentBase(state0);
            FsmData data = new FsmData(new []{agent});
            while (true)
            {
                new FsmUpdater().Update(data);
                Thread.Sleep(100);
            }
            
            
            ConditionSolver solver = new ConditionSolver();
            Stopwatch sw = new Stopwatch();
            ILayoutNode root = new AndLayoutNode(Create2(), Create2());
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

        private static ILayoutNode Create2()
        {
            return new AndLayoutNode(Create(), Create());
        }
        
        private static ILayoutNode Create()
        {
            return new AndLayoutNode(
                new NotLayoutNode(new ConditionLayoutNode(new FalseCondition())),
                new OrLayoutNode(
                    new ConditionLayoutNode(new FalseCondition()),
                    new ConditionLayoutNode(new TrueCondition())));
        }
    }
}