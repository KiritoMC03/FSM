using System;
using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualFunctionNode : VisualNodeWithLinkExit
    {
        public VisualFunctionNode(Type type) : base(type.Name)
        {
            this.AddManipulator(new DraggerManipulator());
        }
    }

    public class VisualFunctionNode<T> : VisualFunctionNode
    {
        public VisualFunctionNode(FunctionLayoutNode<bool> node) : this(node.LogicObject.GetType())
        {
        }

        public VisualFunctionNode(Type type) : base(type)
        {
        }
    }
}