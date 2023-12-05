using System;
using System.Diagnostics;
using FSM.Runtime.Common;
using FSM.Runtime.Serialization;
using Newtonsoft.Json;

namespace FSM.Runtime
{
    [Serializable]
    public class IntFunc : IFunction<int>
    {
        public int val;
        public int Execute() => val;
    }
    
    public class Program
    {
        public static void Main()
        {
            var innter = new ActionLayoutNodeModel(
                    new NothingAction(){ParamNode = new ParamNode<int>(
                        new SerTestFunc() { OtherParam = new ParamNode<int>(
                            new SerTestFunc2())})});
            var actSrc = new AbstractSerializableType<ActionLayoutNodeModel>(
                new ActionLayoutNodeModel(new NothingAction(),
                    new ActionLayoutNodeModel(new LogAction("a"),
                        new ActionLayoutNodeModel(new LogAction("b"),
                            new ActionLayoutNodeModel(new NothingAction(),
                                new ActionLayoutNodeModel(new LogAction("c"),
                                    innter)))))
                );

            var actText = JsonConvert.SerializeObject(actSrc);
            // var actRes = ActionLayoutNodesSerializer.DeserializeAndConvert<AbstractSerializableType<ActionLayoutNodeModel>>(actText);
            
            var tmp1 = ActionLayoutNodesSerializer.DeserializeAndConvert<AbstractSerializableType<ActionLayoutNodeModel>>(actText);
            new ActionsExecutor().Execute(tmp1);
            // var tmp2 = ((NothingAction) tmp1.LogicObject).ParamNode.Execute();
            
            var serSw = new Stopwatch();
            serSw.Start();
            var actRes = ActionLayoutNodesSerializer.DeserializeAndConvert<AbstractSerializableType<ActionLayoutNodeModel>>(actText);
            serSw.Stop();
            Console.WriteLine($"Ser: {serSw.ElapsedTicks} - {serSw.ElapsedMilliseconds}");
            
            serSw = new Stopwatch();
            serSw.Start();
            actRes = ActionLayoutNodesSerializer.DeserializeAndConvert<AbstractSerializableType<ActionLayoutNodeModel>>(actText);
            serSw.Stop();
            Console.WriteLine($"Ser: {serSw.ElapsedTicks} - {serSw.ElapsedMilliseconds}");
            
            
            AbstractSerializableType<ConditionalLayoutNodeModel> src = new AbstractSerializableType<ConditionalLayoutNodeModel>(
                new AndLayoutNodeModel
                (
                    new NotLayoutNodeModel(new ConditionLayoutNodeModel(new FalseCondition())),
                    new OrLayoutNodeModel
                    (
                        new ConditionLayoutNodeModel(new FalseCondition()),
                        new ConditionLayoutNodeModel(new TrueCondition()))
                )
            );
            var text = JsonConvert.SerializeObject(src, Formatting.None);

            serSw = new Stopwatch();
            serSw.Start();
            var textObj = ConditionLayoutNodesSerializer.DeserializeAndConvert<AbstractSerializableType<ConditionalLayoutNodeModel>>(text);
            serSw.Stop();
            Console.WriteLine($"Ser: {serSw.ElapsedTicks} - {serSw.ElapsedMilliseconds}");
            
            serSw = new Stopwatch();
            serSw.Start();
            textObj = ConditionLayoutNodesSerializer.DeserializeAndConvert<AbstractSerializableType<ConditionalLayoutNodeModel>>(text);
            serSw.Stop();
            Console.WriteLine($"Ser: {serSw.ElapsedTicks} - {serSw.ElapsedMilliseconds}");
            
            // FunctionLayoutNode<int> func = new FunctionLayoutNode<int>(default, new IntFunc(){val = 3});
            // Type type = func.GetType(); 
            // string ser = JsonConvert.SerializeObject(func);
            // FunctionLayoutNode deser = (FunctionLayoutNode)JsonConvert.DeserializeObject(ser, type);
            // var res = deser.ExecuteObject();
            
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
            BaseTransition transitionTo3 = new BaseTransition(state3, textObj);
            BaseTransition transitionTo0 = new BaseTransition(state0, new OrLayoutNode(
                new ConditionLayoutNode(new FalseCondition()),
                new ConditionLayoutNode(new TrueCondition())));
            state0.SetTransitions(new []{transitionTo1});
            state1.SetTransitions(new []{transitionTo2});
            state2.SetTransitions(new []{transitionTo3});
            state3.SetTransitions(new []{transitionTo0});
            
            FsmAgentBase agent = new FsmAgentBase(state0);
            FsmData data = new FsmData(new []{agent});
            FsmUpdater updater = new FsmUpdater();
            
            updater.Update(data);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                updater.Update(data);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks / 100);
            
            
            
            
            
            
            
            
            // ConditionSolver solver = new ConditionSolver();
            // Stopwatch sw = new Stopwatch();
            // ILayoutNode root = new AndLayoutNode(Create2(), Create2());
            // sw.Start();
            // bool a = solver.Solve(root);
            // sw.Stop();
            // Console.WriteLine(sw.ElapsedTicks);
            //
            //
            // sw = new Stopwatch();
            // bool r = true;
            // sw.Start();
            // for (int i = 0; i < 100; i++)
            // {
            //     r &= solver.Solve(root);
            // }
            // sw.Stop();
            // Console.WriteLine(sw.ElapsedTicks / 100);
            //
            // Console.WriteLine($"Result: {a} {r}");
        }

        private static IConditionalLayoutNode Create2()
        {
            return new AndLayoutNode(Create(), Create());
        }
        
        private static IConditionalLayoutNode Create()
        {
            return new AndLayoutNode(
                new NotLayoutNode(new ConditionLayoutNode(new FalseCondition())),
                new OrLayoutNode(
                    new ConditionLayoutNode(new FalseCondition()),
                    new ConditionLayoutNode(new TrueCondition())));
        }
    }
}