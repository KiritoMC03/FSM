using System.Collections.Generic;

namespace FSM.Editor
{
    public class TransitionContext : Context
    {
        private readonly StateTransition target;
        private readonly List<ConditionalNode> nodes = new List<ConditionalNode>();
        public readonly string Name;

        public TransitionContext(StateTransition target, string name)
        {
            this.target = target;
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            // this.AddManipulator(new CreateNodeManipulator(fabric));
        }
    }
}