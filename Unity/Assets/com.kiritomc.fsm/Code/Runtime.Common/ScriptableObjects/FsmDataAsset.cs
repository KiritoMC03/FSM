using System.Collections.Generic;
using FSM.Runtime.Serialization;
using UnityEngine;

namespace FSM.Runtime.Common
{
    [CreateAssetMenu(fileName = "FsmDataAsset", menuName = "Fsm/FsmDataAsset", order = 200)]
    public class FsmDataAsset : ScriptableObject
    {
        [field: SerializeField, HideInInspector]
        public string Json { get; set; }

#if UNITY_EDITOR
        [field: SerializeField, HideInInspector]
        public string EditorModel { get; set; }
#endif

        public List<StateBase> Deserialize()
        {
            return StateSerializer.DeserializeAndConvert(Json);
        }
    }
}