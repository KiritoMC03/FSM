using System;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualConditionNode : VisualNodeWithLinkFields
    {
        public readonly Type ConditionType;
        private readonly TransitionContext context;

        public VisualConditionNode(Type conditionType, TransitionContext context, Vector2 position = default) : base(conditionType)
        {
            this.ConditionType = conditionType;
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteConnectionManipulator(context));
            style.left = position.x;
            style.top = position.y;
        }
    }
}