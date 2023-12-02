using System;
using FSM.Runtime;

public class Program
{
    public static void Main()
    {
        DecisionSolver solver = new DecisionSolver();
        ILogicLayoutNode root = new AndLayoutNode(
                new NotLayoutNode(new BaseLogicGateNode(default, new TrueDecision())),
                new OrLayoutNode(
                    new BaseLogicGateNode(default, new FalseDecision()),
                    new BaseLogicGateNode(default, new TrueDecision())));
        
        var a = solver.Solve(new DecisionsBlock(root));
        Console.WriteLine($"Result: {a}");
    }

    public class TrueDecision : IDecision
    {
        public bool Decide() => true;
    }

    public class FalseDecision : IDecision
    {
        public bool Decide() => false;
    }
}