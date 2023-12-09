using System;
using UnityEngine.UIElements;

namespace FSM.Editor.Events
{
    public class ConnectionRequestEvent : EventBase<ConnectionRequestEvent>
    {
        public Action<Node> SetConnectionCallback { get; set; }
    }
}