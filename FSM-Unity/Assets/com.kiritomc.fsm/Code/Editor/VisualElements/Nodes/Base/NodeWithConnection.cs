using UnityEngine;

namespace FSM.Editor
{
    public abstract class NodeWithConnection: Node //ToDo: name?
    {
        protected NodeConnectionPoint Connection;

        protected NodeWithConnection(string nodeName) : base(nodeName)
        {
            Connection = Header.ConnectionPointRelativeTo(Label, ConnectionPosition.Right);
        }

        public virtual Vector2 GetAbsoluteConnectionPos()
        {
            return new Vector2(Connection.worldBound.x + Sizes.ConnectionNodePoint / 2f, Connection.worldBound.y + Sizes.ConnectionNodePoint / 2f);
        }
    }
}