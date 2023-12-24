using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FSM.Editor.Manipulators;
using FSM.Runtime;
using FSM.Runtime.Serialization;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class VisualConditionNode : VisualNodeWithLinkExit
    {
        private readonly TransitionContext context;
        private Dictionary<string, IVisualNodeWithLinkExit> linked = new Dictionary<string, IVisualNodeWithLinkExit>();

        public VisualConditionNode(ConditionLayoutNode node, TransitionContext context) : this(node.LogicObject.GetType(), context)
        {
        }

        public VisualConditionNode(Type conditionType, TransitionContext context) : base(conditionType.Name)
        {
            this.context = context;
            this.AddManipulator(new DraggerManipulator());
            this.AddManipulator(new RouteConnectionManipulator(context));
            IEnumerable<FieldInfo> fields = conditionType
                .GetFields()
                .Where(field => field.FieldType.IsGenericType && 
                                field.FieldType.GetGenericTypeDefinition() == typeof(ParamNode<>));
            foreach (FieldInfo fieldInfo in fields)
            {
                FieldWrapper<VisualFunctionNode> fieldWrapper = new FieldWrapper<VisualFunctionNode>();
                // object instance = Activator.CreateInstance(fieldWrapperType.MakeGenericType(returnedType));
                new VisualNodeLinkRegistration(this, fieldInfo.Name, NodeLinkRequest.NewAsync(this), HandleLinked, GetCurrentLinkedNode);
            }
        }

        protected virtual void HandleLinked(string fieldName, IVisualNodeWithLinkExit target)
        {
            linked[fieldName] = target;
        }

        protected virtual IVisualNodeWithLinkExit GetCurrentLinkedNode(string fieldName)
        {
            return linked.GetValueOrDefault(fieldName);
        }
    }
}