using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FSM.Editor.Function;
using FSM.Runtime;
using FSM.Runtime.Serialization;

namespace FSM.Editor
{
    public class ConditionNode : ConditionalNode
    {
        public ConditionNode(ConditionLayoutNode node) : base(node.LogicObject.GetType().Name)
        {
            Type objType = node.LogicObject.GetType();
            IEnumerable<FieldInfo> fields = objType
                .GetFields()
                .Where(field => field.FieldType.IsGenericType && 
                                field.FieldType.GetGenericTypeDefinition() == typeof(ParamNode<>));
            foreach (FieldInfo fieldInfo in fields)
            {
                FieldWrapper<FunctionNode> fieldWrapper = new FieldWrapper<FunctionNode>();
                // object instance = Activator.CreateInstance(fieldWrapperType.MakeGenericType(returnedType));
                BuildConnectionField(fieldInfo.Name, fieldWrapper, RequestConnection);
            }
        }

        public override string GetMetadataForSerialization()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleDeserializedMetadata(string metadata)
        {
            throw new System.NotImplementedException();
        }
    }
}