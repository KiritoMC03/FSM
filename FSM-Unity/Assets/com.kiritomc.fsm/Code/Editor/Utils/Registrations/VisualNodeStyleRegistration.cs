using System;
using System.Reactive.Disposables;

namespace FSM.Editor
{
    public class VisualNodeStyleRegistration : ICancelable
    {
        private readonly VisualNode target;
        private readonly Action applyStyle;

        public bool IsDisposed { get; private set; }

        public VisualNodeStyleRegistration(VisualNode target, Action applyStyle)
        {
            this.target = target;
            this.applyStyle = applyStyle;
        }

        public void Dispose() => IsDisposed = true;

        public void Apply()
        {
            if (!IsDisposed) applyStyle();
        }
    }
}