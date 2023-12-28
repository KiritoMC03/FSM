using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FSM.Runtime.Serialization
{
    public class ActionLayoutNodesSerializer
    {
        private static readonly Queue<(ActionLayoutNodeModel self, ActionLayoutNodeModel connection)> OriginalQueue =
                            new Queue<(ActionLayoutNodeModel self, ActionLayoutNodeModel connection)>(16);
        private static readonly Queue<ActionLayoutNode> CopyQueue = new Queue<ActionLayoutNode>(16);
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver() { IgnoreSerializableAttribute = false },
        };

        public static ActionLayoutNode DeserializeAndConvert<T>(string json)
            where T: AbstractSerializableType<ActionLayoutNodeModel>
        {
            return Convert(JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings).Item);
        }

        public static ActionLayoutNode Convert(ActionLayoutNodeModel root)
        {
            (ActionLayoutNode rootCopy, ActionLayoutNodeModel rootConnection) = GetNodesFor(root);
            OriginalQueue.Enqueue((root, rootConnection));
            CopyQueue.Enqueue(rootCopy);

            while (OriginalQueue.Count > 0)
            {
                (ActionLayoutNodeModel original, ActionLayoutNodeModel originalConnection) = OriginalQueue.Dequeue();
                ActionLayoutNode copyOperationNode = CopyQueue.Dequeue();

                if (originalConnection != null)
                {
                    (ActionLayoutNode result, ActionLayoutNodeModel connection) = GetNodesFor(originalConnection);
                    OriginalQueue.Enqueue((originalConnection, connection));
                    copyOperationNode.Connection = result;
                    CopyQueue.Enqueue(result);
                }
            }

            OriginalQueue.Clear();
            CopyQueue.Clear();
            return rootCopy;
        }

        private static (ActionLayoutNode result, ActionLayoutNodeModel connection) GetNodesFor(ActionLayoutNodeModel layoutNode)
        {
            return (new ActionLayoutNode(layoutNode.Action.Item), layoutNode.Connection?.Item);
        }
    }
}