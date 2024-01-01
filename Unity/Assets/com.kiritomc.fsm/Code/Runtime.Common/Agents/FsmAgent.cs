using System.Linq;
using UnityEngine;

namespace FSM.Runtime.Common
{
    public class FsmAgent : MonoBehaviour
    {
        [SerializeField]
        protected FsmDataAsset fsmDataAsset;

        public IFsmAgent AsRunnable()
        {
            return new FsmAgentBase(fsmDataAsset.Deserialize().First());
        }
    }
}