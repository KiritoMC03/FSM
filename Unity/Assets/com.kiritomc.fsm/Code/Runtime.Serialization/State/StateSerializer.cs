using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FSM.Runtime.Serialization
{
    public class StateSerializer
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver() { IgnoreSerializableAttribute = false },
        };

        public static List<StateBase> DeserializeAndConvert(string json)
        {
            List<StateModel> models = JsonConvert.DeserializeObject<List<StateModel>>(json, JsonSerializerSettings);
            List<StateBase> results = new List<StateBase>(models.Count);
            foreach (StateModel stateModel in models)
            {
                results.Add(new StateBase(stateModel.Name, default));
            }

            for (int i = 0; i < models.Count; i++)
            {
                StateModel model = models[i];
                results[i].OutgoingTransitions = DeserializeTransitions(model, results);
                results[i].OnEnterActions = ActionLayoutNodesSerializer.Convert(model.OnEnter.Item);
                results[i].OnUpdateActions = ActionLayoutNodesSerializer.Convert(model.OnUpdate.Item);
                results[i].OnExitActions = ActionLayoutNodesSerializer.Convert(model.OnExit.Item);
            }

            return results;
        }

        private static List<ITransition> DeserializeTransitions(StateModel stateModel, List<StateBase> neighboringStates)
        {
            List<ITransition> transitions = new List<ITransition>(stateModel.Transitions.Count);
            foreach ((string targetName, AbstractSerializableType<ConditionalLayoutNodeModel> conditionModel) in stateModel.Transitions)
            {
                IConditionalLayoutNode condition = ConditionLayoutNodesSerializer.Convert(conditionModel.Item);
                transitions.Add(new BaseTransition(neighboringStates.Find(i => i.Name == targetName), condition));
            }
            return transitions;
        }
    }
}