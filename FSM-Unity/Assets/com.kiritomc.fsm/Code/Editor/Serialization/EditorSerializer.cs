using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FSM.Editor.Serialization
{
    public class EditorSerializer
    {
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        #region Seriaization

        public string Serialize(FsmContext rootContext)
        {
            StatesContextModel context = WriteStateContext(rootContext.StatesContext);
            return JsonConvert.SerializeObject(new FsmContextModel()
            {
                StatesContextModel = context,
            });
        }

        private StatesContextModel WriteStateContext(StatesContext statesContext)
        {
            return new StatesContextModel(statesContext.Nodes.Select(WriteStateNode).ToArray(), statesContext.Name);
        }

        private StateNodeModel WriteStateNode(VisualStateNode stateNode)
        {
            return new StateNodeModel
            (
                stateNode.Name,
                (Vector2Model)stateNode.ResolvedPlacement,
                stateNode.Transitions.Select(transition => WriteTransitionContext(stateNode, transition)).ToArray(),
                WriteStateNodeLifecycle(stateNode)
            );
        }

        private StateTransitionModel WriteTransitionContext(VisualStateNode sourceNode, VisualStateTransition transition)
        {
            VisualNodeModel[] contextEntryNodeModels = new VisualNodeModel[transition.Context.Nodes.Count];
            for (int i = 0; i < transition.Context.Nodes.Count; i++)
            {
                VisualNodeWithLinkExit node = transition.Context.Nodes[i];
                (Type type, Dictionary<string, int> linked) = node switch
                {
                    VisualConditionNode conditionNode => (conditionNode.ConditionType, LinkedToIndexes(conditionNode.Linked)),
                    VisualFunctionNode functionNode => (functionNode.FunctionType, LinkedToIndexes(functionNode.Linked)),
                    _ => throw new Exception(),
                };

                contextEntryNodeModels[i] = new VisualNodeModel(type, (Vector2Model)node.ResolvedPlacement, linked);
            }

            TransitionContextModel contextModel = new TransitionContextModel(contextEntryNodeModels);
            return new StateTransitionModel(sourceNode.Name, transition.Target.Name, contextModel);

            Dictionary<string, int> LinkedToIndexes(Dictionary<string, VisualNodeWithLinkExit> linked)
            {
                return linked.ToDictionary(
                    pair => pair.Key, 
                    pair => transition.Context.Nodes.IndexOf(pair.Value));
            }
        }

        private StateNodeLifecycleModel WriteStateNodeLifecycle(VisualStateNode stateNode)
        {
            StateNodeLifecycleModel result = new StateNodeLifecycleModel
            {
                AnchorNodePosition = (Vector2Model)stateNode.Context.AnchorNode.ResolvedPlacement,
                OnEnterId = stateNode.Context.Nodes.IndexOf(stateNode.Context.AnchorNode.OnEnterLink),
                OnUpdateId = stateNode.Context.Nodes.IndexOf(stateNode.Context.AnchorNode.OnUpdateLink),
                OnExitId = stateNode.Context.Nodes.IndexOf(stateNode.Context.AnchorNode.OnExitLink),
                Nodes = new VisualNodeModel[stateNode.Context.Nodes.Count],
            };
            for (int i = 0; i < stateNode.Context.Nodes.Count; i++)
            {
                VisualNodeWithLinkExit node = stateNode.Context.Nodes[i];
                (Type type, Dictionary<string, int> linked) = node switch
                {
                    VisualActionNode actionNode => (actionNode.ActionType, LinkedToIndexes(actionNode.Linked)),
                    VisualFunctionNode functionNode => (functionNode.FunctionType, LinkedToIndexes(functionNode.Linked)),
                    _ => throw new Exception(),
                };
                result.Nodes[i] = new VisualNodeModel(type, (Vector2Model)node.ResolvedPlacement, linked);
            }
            return result;

            Dictionary<string, int> LinkedToIndexes(Dictionary<string, VisualNodeWithLinkExit> linked)
            {
                return linked.ToDictionary(
                    pair => pair.Key, 
                    pair => stateNode.Context.Nodes.IndexOf(pair.Value));
            }
        }

        #endregion

        #region Deserialization

        public FsmContext Deserialize(string json)
        {
            FsmContextModel fsmContextModel = JsonConvert.DeserializeObject<FsmContextModel>(json);
            FsmContext fsmContext = new FsmContext
            {
                StatesContext = fsmContextModel == null ? Fabric.Contexts.CreateRootContext() : ReadStatesContext(fsmContextModel.StatesContextModel, isRoot: true),
            };
            return fsmContext;
        }

        private StatesContext ReadStatesContext(StatesContextModel statesContextModel, VisualStateNode target = default, bool isRoot = false)
        {            
            if (statesContextModel == null) return Fabric.Contexts.CreateRootContext();
            StatesContext statesContext = isRoot ? Fabric.Contexts.CreateRootContext() : Fabric.Contexts.CreateStateContext(target);
            List<VisualStateNode> states = statesContextModel.StateNodeModels.Select(nodeModel => ReadStateNode(statesContext, nodeModel)).ToList();
            statesContext.Nodes = states;
            for (int i = 0; i < states.Count; i++)
            {
                ReadStateNodeLifecycle(states[i], statesContextModel.StateNodeModels[i].Lifecycle);
                AppendTransitionsToStateNode(states[i], statesContextModel.StateNodeModels[i], states);
            }

            return statesContext;
        }

        private VisualStateNode ReadStateNode(StatesContext context, StateNodeModel stateNodeModel)
        {
            VisualStateNode node = new VisualStateNode(stateNodeModel.Name, context, stateNodeModel.Position, stateNodeModel.Lifecycle?.AnchorNodePosition ?? default);
            context.Add(node);
            return node;
        }

        private void AppendTransitionsToStateNode(VisualStateNode node, StateNodeModel nodeModel, IEnumerable<VisualStateNode> otherStates)
        {
            node.Transitions = nodeModel.OutgoingTransitions.Select(transitionModel =>
            {
                VisualStateNode target = otherStates.First(targetState => targetState.Name == transitionModel.TargetName);
                VisualStateTransition transition = node.AddTransition(target);
                ReadTransitionContext(transition, transitionModel.ContextModel);
                return transition;
            }).ToList();
        }

        private void ReadTransitionContext(VisualStateTransition transition, TransitionContextModel transitionContextModel)
        {
            VisualNodeWithLinkFields[] nodes = new VisualNodeWithLinkFields[transitionContextModel.ConditionalNodeModels.Length];
            for (int i = 0; i < transitionContextModel.ConditionalNodeModels.Length; i++)
            {
                VisualNodeModel contextEntryNodeModels = transitionContextModel.ConditionalNodeModels[i];
                if (contextEntryNodeModels.Type.IsICondition())
                {
                    transition.Context.ProcessNewNode((VisualConditionNode)(nodes[i] = new VisualConditionNode(contextEntryNodeModels.Type, transition.Context, contextEntryNodeModels.Position)));
                }
                else if (contextEntryNodeModels.Type.IsIFunctionBool())
                {
                    transition.Context.ProcessNewNode((VisualFunctionNode<bool>)(nodes[i] = new VisualFunctionNode<bool>(contextEntryNodeModels.Type, transition.Context, contextEntryNodeModels.Position)));
                }
            }

            for (int i = 0; i < transitionContextModel.ConditionalNodeModels.Length; i++)
            {
                VisualNodeModel contextEntryNodeModels = transitionContextModel.ConditionalNodeModels[i];
                foreach ((string fieldName, int linkIndex) in contextEntryNodeModels.Linked)
                {
                    nodes[i].ForceLinkTo(fieldName, transition.Context.Nodes[linkIndex]);
                }
            }
        }

        private void ReadStateNodeLifecycle(VisualStateNode node, StateNodeLifecycleModel lifecycleModel)
        {
            if (lifecycleModel == null) return;
            VisualNodeWithLinkFields[] nodes = new VisualNodeWithLinkFields[lifecycleModel.Nodes.Length];
            StateContext context = node.Context;
            for (int i = 0; i < lifecycleModel.Nodes.Length; i++)
            {
                VisualNodeModel nodeModel = lifecycleModel.Nodes[i];
                if (nodeModel.Type.IsIAction())
                {
                    context.ProcessNewNode(nodes[i] = new VisualActionNode(nodeModel.Type, context, nodeModel.Position));
                }
                else if (nodeModel.Type.IsIFunction())
                {
                    context.ProcessNewNode(nodes[i] = new VisualFunctionNode(nodeModel.Type, context, nodeModel.Position));
                }
            }

            Link(lifecycleModel.OnEnterId, VisualActionAnchorNode.OnEnter);
            Link(lifecycleModel.OnUpdateId, VisualActionAnchorNode.OnUpdate);
            Link(lifecycleModel.OnExitId, VisualActionAnchorNode.OnExit);
            for (int i = 0; i < lifecycleModel.Nodes.Length; i++)
            {
                VisualNodeModel nodeModel = lifecycleModel.Nodes[i];
                foreach ((string fieldName, int linkIndex) in nodeModel.Linked)
                {
                    nodes[i].ForceLinkTo(fieldName, node.Context.Nodes[linkIndex]);
                }
            }

            void Link(int index, string name)
            {
                if (index != -1) context.AnchorNode.ForceLinkTo(name, node.Context.Nodes[index]);
            }
        }

        #endregion
    }
}