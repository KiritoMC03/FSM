using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public class OpenTransitionInContextManipulator : Manipulator
    {
        private readonly StatesContext context;

        public OpenTransitionInContextManipulator(StatesContext context)
        {
            this.context = context;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            context.RegisterCallback<ClickEvent>(HandleClick);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            context.UnregisterCallback<ClickEvent>(HandleClick);
        }

        private void HandleClick(ClickEvent e)
        {
            if (e.clickCount != 2 || e.button != Keys.OpenTransitionButton) return;
            foreach (VisualStateTransitionData transitionData in context.Nodes.SelectMany(node => node.Transitions))
            {
                foreach (Vector2 point in transitionData.Transition.IterateLinkWorldPoints())
                {
                    float dist = Vector2.Distance(point, e.position);
                    if (dist > Sizes.TransitionPointClickTrackRadius) 
                        continue;
                    transitionData.Transition.Context.Open();
                    e.StopPropagation();
                    return;
                }
            }
        }
    }
}