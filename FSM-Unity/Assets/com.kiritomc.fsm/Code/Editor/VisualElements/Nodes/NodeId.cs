using System;
using UnityEngine;

namespace FSM.Editor
{
    [Serializable]
    public struct NodeId
    {
        [SerializeField]
        private int id;
        public int Id => id;

        public NodeId(int id)
        {
            this.id = id;
        }
    }
}