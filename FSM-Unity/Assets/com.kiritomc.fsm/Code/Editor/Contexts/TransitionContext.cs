using System.Collections.Generic;

namespace FSM.Editor
{
    public class TransitionContext : Context
    {
        private readonly StateTransition target;
        private readonly List<ConditionalNode> nodes = new List<ConditionalNode>();

        public TransitionContext(StateTransition target)
        {
            this.target = target;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            // this.AddManipulator(new CreateNodeManipulator(fabric));
        }
    }
}