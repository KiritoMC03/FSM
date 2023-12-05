namespace FSM.Runtime
{
    public interface IConditionalLayoutNode
    {
        
    }

    public interface IConditionalLayoutNodeWithLeftBranch
    {
        IConditionalLayoutNode Left { get; set; }
    }

    public interface IConditionalLayoutNodeWithRightBranch
    {
        IConditionalLayoutNode Right { get; set; }
    }
}