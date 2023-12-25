using System;
using System.Collections.Generic;
using System.Linq;
using FSM.Editor.Extensions;
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
                stateNode.Transitions.Select(transition => WriteTransitionContext(stateNode, transition)).ToArray()
            );
        }

        private StateTransitionModel WriteTransitionContext(VisualStateNode sourceNode, VisualStateTransition transition)
        {
            TransitionContextEntryNodeModel[] contextEntryNodeModels = new TransitionContextEntryNodeModel[transition.Context.Nodes.Count];
            for (int i = 0; i < transition.Context.Nodes.Count; i++)
            {
                VisualNodeWithLinkExit node = transition.Context.Nodes[i];
                (Type type, Dictionary<string, int> linked) = node switch
                {
                    VisualConditionNode conditionNode => (conditionNode.ConditionType, LinkedToIndexes(conditionNode.Linked)),
                    VisualFunctionNode functionNode => (functionNode.FunctionType, LinkedToIndexes(functionNode.Linked)),
                    _ => throw new Exception(),
                };

                contextEntryNodeModels[i] = new TransitionContextEntryNodeModel(type, (Vector2Model)node.ResolvedPlacement, linked);
            }

            TransitionContextModel contextModel = new TransitionContextModel(contextEntryNodeModels);
            return new StateTransitionModel(sourceNode.Name, transition.Target.Name, contextModel);

            Dictionary<string, int> LinkedToIndexes(Dictionary<string, IVisualNodeWithLinkExit> linked)
            {
                return linked.ToDictionary(
                    pair => pair.Key, 
                    pair => transition.Context.Nodes.IndexOf((VisualNodeWithLinkExit)pair.Value));
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
            for (int i = 0; i < states.Count; i++) AppendTransitionsToStateNode(states[i], statesContextModel.StateNodeModels[i], states);

            return statesContext;
        }

        private VisualStateNode ReadStateNode(StatesContext context, StateNodeModel stateNodeModel)
        {
            VisualStateNode node = new VisualStateNode(stateNodeModel.Name, context, stateNodeModel.Position);
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
                TransitionContextEntryNodeModel contextEntryNodeModels = transitionContextModel.ConditionalNodeModels[i];
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
                TransitionContextEntryNodeModel contextEntryNodeModels = transitionContextModel.ConditionalNodeModels[i];
                foreach ((string fieldName, int linkIndex) in contextEntryNodeModels.Linked)
                {
                    UnityEngine.Debug.Log($"{fieldName} - {linkIndex} - {transition.Context.Nodes.Count}");
                    nodes[i].ForceLinkTo(fieldName, transition.Context.Nodes[linkIndex]);
                }
            }
        }

        #endregion
    }
}