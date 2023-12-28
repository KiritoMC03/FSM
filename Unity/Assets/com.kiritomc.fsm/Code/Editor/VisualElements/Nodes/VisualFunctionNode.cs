using System;
using FSM.Editor.Manipulators;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualFunctionNode : VisualNodeWithLinkFields
    {
        public readonly Type FunctionType;

        public VisualFunctionNode(Type functionType, VisualNodesContext context, Vector2 position = default) : 
            this(functionType, context.GetFreeId(), context, position)
        {
        }

        public VisualFunctionNode(Type functionType, int id, VisualNodesContext context, Vector2 position = default) : base(functionType, id)
        {
            context.ReserveId(id, this);
            FunctionType = functionType;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteVisualNodeLinkManipulator(context));
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