using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FSM.Runtime.Serialization
{
    public static class ConditionLayoutNodesSerializer
    {
        private static readonly Queue<(ConditionalLayoutNodeModel self, ConditionalLayoutNodeModel left, ConditionalLayoutNodeModel right)> OriginalQueue =
                            new Queue<(ConditionalLayoutNodeModel self, ConditionalLayoutNodeModel left, ConditionalLayoutNodeModel right)>(16);
        private static readonly Queue<IConditionalLayoutNode> CopyQueue = new Queue<IConditionalLayoutNode>(16);

        public static IConditionalLayoutNode DeserializeAndConvert<T>(string json)
            where T: AbstractSerializableType<ConditionalLayoutNodeModel>
        {
            return Convert(JsonConvert.DeserializeObject<T>(json).Item);
        }

        public static IConditionalLayoutNode Convert(ConditionalLayoutNodeModel root)
        {
            (IConditionalLayoutNode newNode, ConditionalLayoutNodeModel rootLeft, ConditionalLayoutNodeModel rootRight) = GetNodesFor(root);
            OriginalQueue.Enqueue((root, rootLeft, rootRight));
            CopyQueue.Enqueue(newNode);

            while (OriginalQueue.Count > 0)
            {
                (ConditionalLayoutNodeModel original, ConditionalLayoutNodeModel originalLeft, ConditionalLayoutNodeModel originalRight) = OriginalQueue.Dequeue();
                IConditionalLayoutNode copyOperationNode = CopyQueue.Dequeue();

                if (originalLeft != null)
                {
                    (IConditionalLayoutNode result, ConditionalLayoutNodeModel left, ConditionalLayoutNodeModel right) = GetNodesFor(originalLeft);
                    OriginalQueue.Enqueue((originalLeft, left, right));
                    if (copyOperationNode is IConditionalLayoutNodeWithLeftBranch nodeWithLeftBranch)
                        nodeWithLeftBranch.Left = result;
                    CopyQueue.Enqueue(result);
                }

                if (originalRight != null)
                {
                    (IConditionalLayoutNode result, ConditionalLayoutNodeModel left, ConditionalLayoutNodeModel right) = GetNodesFor(originalRight);
                    OriginalQueue.Enqueue((originalRight, left, right));
                    if (copyOperationNode is IConditionalLayoutNodeWithRightBranch nodeWithRightBranch)
                        nodeWithRightBranch.Right = result;
                    CopyQueue.Enqueue(result);
                }
            }

            OriginalQueue.Clear();
            CopyQueue.Clear();
            return newNode;
        }

        private static (IConditionalLayoutNode result, ConditionalLayoutNodeModel left, ConditionalLayoutNodeModel right) GetNodesFor(ConditionalLayoutNodeModel layoutNode)
        {
            return layoutNode switch
            {
                NotLayoutNodeModel not   => (new NotLayoutNode(), not.LeftModel?.Item, default),
                OrLayoutNodeModel or     => (new OrLayoutNode(), or.LeftModel?.Item, or.RightModel?.Item),
                AndLayoutNodeModel and   => (new AndLayoutNode(), and.LeftModel?.Item, and.RightModel?.Item),
                ConditionLayoutNodeModel condition => (new ConditionLayoutNode(condition.Condition.Item), default, default),
                _ => throw new Exception($"{layoutNode.GetType()} is not {typeof(ConditionalLayoutNodeModel)}")
            };
        }
    }
}