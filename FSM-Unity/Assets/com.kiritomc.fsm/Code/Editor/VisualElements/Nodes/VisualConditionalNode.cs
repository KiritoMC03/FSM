using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FSM.Runtime;
using FSM.Runtime.Serialization;

namespace FSM.Editor
{
    public class VisualConditionalNode : VisualNodeWithLinkExit
    {
        public VisualConditionalNode(ConditionLayoutNode node) : this(node.LogicObject.GetType())
        {
        }

        public VisualConditionalNode(Type conditionType)
        {
            IEnumerable<FieldInfo> fields = conditionType
                .GetFields()
                .Where(field => field.FieldType.IsGenericType && 
                                field.FieldType.GetGenericTypeDefinition() == typeof(ParamNode<>));
            foreach (FieldInfo fieldInfo in fields)
            {
                // FieldWrapper<FunctionNode> fieldWrapper = new FieldWrapper<FunctionNode>();
                // // object instance = Activator.CreateInstance(fieldWrapperType.MakeGenericType(returnedType));
                // new VisualNodeLinkRegistration(this, fieldInfo.Name, NodeLinkRequest.NewAsync(this), HandleLinked, GetCurrentLinkedNode);
                // NodeLinkFieldView(fieldInfo.Name, fieldWrapper, RequestConnection);
            }
        }

        protected virtual void HandleLinked(IVisualNodeWithLinkExit target)
        {
            
        }

        protected virtual IVisualNodeWithLinkExit GetCurrentLinkedNode()
        {
            return default;
        }
    }
}