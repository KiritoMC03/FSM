using System;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualNodeTransitionField : VisualElement
    {
        private const int HorizontalMargin = 10;
        public const int VerticalMargin = 0;

        private event Action PriorityUpClicked;
        private event Action PriorityDownClicked;
        private VisualNodeTransitionPriorityButton priorityUp;
        private VisualNodeTransitionPriorityButton priorityDown;

        public VisualNodeTransitionField(string targetName, VisualStateTransition transition)
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.marginTop = VerticalMargin;
            style.marginBottom = VerticalMargin;
            style.marginRight = HorizontalMargin;
            style.marginLeft = HorizontalMargin;
            style.justifyContent = Justify.SpaceBetween;
            Add(new Label($"-> {targetName}")
            {
                style =
                {
                    marginLeft = HorizontalMargin,
                    marginRight = HorizontalMargin,
                },
            });
            VisualElement priorityButtons = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            Add(priorityButtons);
            priorityButtons.Add(priorityUp = new VisualNodeTransitionPriorityButton(true));
            priorityButtons.Add(priorityDown = new VisualNodeTransitionPriorityButton(false));
            Add(transition);

            priorityUp.clicked += () => PriorityUpClicked?.Invoke();
            priorityDown.clicked += () => PriorityDownClicked?.Invoke();
        }

        public Subscription SubscribePriorityUpClicked(Action handler)
        {
            PriorityUpClicked += handler;
            return new Subscription(() => PriorityUpClicked -= handler);
        }

        public Subscription SubscribePriorityDownClicked(Action handler)
        {
            PriorityDownClicked += handler;
            return new Subscription(() => PriorityDownClicked -= handler);
        }
    }
}