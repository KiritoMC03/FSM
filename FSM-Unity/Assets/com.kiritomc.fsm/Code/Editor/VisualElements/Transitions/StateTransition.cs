using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StateTransition : VisualElement, IDisposable, ICustomRepaintHandler
    {
        public readonly StateNode Target;
        private LineDrawerRegistration lineDrawerRegistration;

        public StateTransition(StateNode target)
        {
            this.Target = target;
            style.width = 0;
            style.height = 0;
            style.top = 0;
            style.left = 0;
            style.position = Position.Absolute;
        }

        public void SetLineDrawerRegistrationLink(LineDrawerRegistration registration)
        {
            lineDrawerRegistration = registration;
        }

        public void Dispose()
        {
            lineDrawerRegistration?.Dispose();
        }

        public void Repaint()
        {
            MarkDirtyRepaint();
            lineDrawerRegistration?.Repaint();
        }
    }
}