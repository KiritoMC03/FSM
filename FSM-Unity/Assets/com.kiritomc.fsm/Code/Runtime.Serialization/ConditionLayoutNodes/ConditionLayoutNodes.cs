using System;

namespace FSM.Runtime.Serialization
{
    public class ConditionalLayoutNodeModel
    {
        protected ConditionalLayoutNodeModel() { }
    }

    [Serializable]
    public sealed class AndLayoutNodeModel : ConditionalLayoutNodeModel
    {
        public AbstractSerializableType<ConditionalLayoutNodeModel> LeftModel;
        public AbstractSerializableType<ConditionalLayoutNodeModel> RightModel;

        public AndLayoutNodeModel() { }

        public AndLayoutNodeModel(ConditionalLayoutNodeModel leftModel, ConditionalLayoutNodeModel rightModel)
        {
            LeftModel = new AbstractSerializableType<ConditionalLayoutNodeModel>(leftModel);
            RightModel = new AbstractSerializableType<ConditionalLayoutNodeModel>(rightModel);
        }
    }

    [Serializable]
    public sealed class OrLayoutNodeModel : ConditionalLayoutNodeModel
    {
        public AbstractSerializableType<ConditionalLayoutNodeModel> LeftModel;
        public AbstractSerializableType<ConditionalLayoutNodeModel> RightModel;

        public OrLayoutNodeModel() { }

        public OrLayoutNodeModel(ConditionalLayoutNodeModel leftModel, ConditionalLayoutNodeModel rightModel)
        {
            LeftModel = new AbstractSerializableType<ConditionalLayoutNodeModel>(leftModel);
            RightModel = new AbstractSerializableType<ConditionalLayoutNodeModel>(rightModel);
        }
    }

    [Serializable]
    public sealed class NotLayoutNodeModel : ConditionalLayoutNodeModel
    {
        public AbstractSerializableType<ConditionalLayoutNodeModel> LeftModel;

        public NotLayoutNodeModel() { }

        public NotLayoutNodeModel(ConditionalLayoutNodeModel leftModel)
        {
            LeftModel = new AbstractSerializableType<ConditionalLayoutNodeModel>(leftModel);
        }
    }

    [Serializable]
    public sealed class ConditionLayoutNodeModel : ConditionalLayoutNodeModel
    {
        public AbstractSerializableType<ICondition> Condition;
        
        public ConditionLayoutNodeModel() { }

        public ConditionLayoutNodeModel(ICondition condition)
        {
            Condition = new AbstractSerializableType<ICondition>(condition);
        }
    }
}