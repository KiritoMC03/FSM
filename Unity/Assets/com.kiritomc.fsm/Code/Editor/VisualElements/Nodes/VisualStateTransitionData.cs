using System.Reactive.Disposables;

namespace FSM.Editor
{
    public class VisualStateTransitionData : ICancelable
    {
        public VisualStateTransition Transition;
        public VisualNodeTransitionField Field;
        public Subscription OnFieldPriorityUpClicked;
        public Subscription OnFieldPriorityDownClicked;

        public bool IsDisposed { get; private set; }

        public VisualStateTransitionData(VisualStateTransition transition, VisualNodeTransitionField field)
        {
            Transition = transition;
            Field = field;
        }

        public void Dispose()
        {
            IsDisposed = true;
            Transition.Dispose();
            OnFieldPriorityUpClicked?.Dispose();
            OnFieldPriorityDownClicked?.Dispose();
        }
    }
}