using System;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualFunctionNode : VisualNodeWithLinkFields
    {
        public readonly Type FunctionType;

        public VisualFunctionNode(Type functionType, VisualNodesContext context, Vector2 position = default) : base(functionType)
        {
            FunctionType = functionType;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteConnectionManipulator(context));
            style.left = position.x;
            style.top = position.y;
        }
    }

    public class VisualFunctionNode<T> : VisualFunctionNode
    {
        public VisualFunctionNode(Type functionType, VisualNodesContext context, Vector2 position = default) : base(functionType, context, position)
        {
        }
    }
}