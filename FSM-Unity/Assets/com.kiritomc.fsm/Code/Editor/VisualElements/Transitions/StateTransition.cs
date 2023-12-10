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
            style.position = Position.Absolute;
            style.top = 0;
            style.bottom = 0;
            style.alignSelf = Align.Center;
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.Center;
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