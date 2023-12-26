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
                List<ITransition> transitions = new List<ITransition>(models.Count);
                StateModel stateModel = models[i];
                foreach ((string targetName, AbstractSerializableType<ConditionalLayoutNodeModel> conditionModel) in stateModel.Transitions)
                {
                    IConditionalLayoutNode condition = ConditionLayoutNodesSerializer.Convert(conditionModel.Item);
                    transitions.Add(new BaseTransition(results.Find(i => i.Name == targetName), condition));
                }
                results[i].SetTransitions(transitions);
            }

            return results;
        }
    }
}