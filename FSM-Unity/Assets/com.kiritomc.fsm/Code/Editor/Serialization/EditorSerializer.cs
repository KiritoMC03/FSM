using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FSM.Editor.Serialization
{
    public class EditorSerializer
    {
        private EditorState EditorState;
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
            return new StatesContextModel(statesContext.StateNodes.Select(WriteStateNode).ToArray(), statesContext.Name);
        }

        private StateNodeModel WriteStateNode(StateNode stateNode)
        {
            return new StateNodeModel
            (
                stateNode.Name,
                new Vector2Model(stateNode.resolvedStyle.left, stateNode.resolvedStyle.top),
                stateNode.Transitions.Select(transition => WriteStateTransition(stateNode, transition)).ToArray()
            );
        }

        private StateTransitionModel WriteStateTransition(StateNode sourceNode, StateTransition transition)
        {
            ConditionalNodeModel[] conditionalNodeModels = new ConditionalNodeModel[transition.Context.Nodes.Count];
            for (int i = 0; i < transition.Context.Nodes.Count; i++)
            {
                ConditionalNode conditionalNode = transition.Context.Nodes[i];
                (string kind, string left, string right) = conditionalNode switch
                {
                    NotNode not => (nameof(NotNode), not.Input.Value?.Name, default),
                    OrNode or => (nameof(OrNode), or.Left.Value?.Name, or.Right.Value?.Name),
                    AndNode and => (nameof(AndNode), and.Left.Value?.Name, and.Right.Value?.Name),
                    _ => (nameof(ConditionNode), default, default),
                };

                Vector2Model position = new Vector2Model(conditionalNode.resolvedStyle.left, conditionalNode.resolvedStyle.top);
                conditionalNodeModels[i] = new ConditionalNodeModel(conditionalNode.Name, position, kind, left, right);
            }

            TransitionContextModel contextModel = new TransitionContextModel(conditionalNodeModels);
            return new StateTransitionModel(sourceNode.Name, transition.Target.Name, contextModel);
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

        private StatesContext ReadStatesContext(StatesContextModel statesContextModel, StateNode target = default, bool isRoot = false)
        {            
            if (statesContextModel == null) return Fabric.Contexts.CreateRootContext();
            StatesContext statesContext = isRoot ? Fabric.Contexts.CreateRootContext() : Fabric.Contexts.CreateStateContext(target);
            List<StateNode> states = statesContextModel.StateNodeModels.Select(nodeModel => ReadStateNode(statesContext, nodeModel)).ToList();
            statesContext.StateNodes = states;
            for (int i = 0; i < states.Count; i++) AppendTransitionsToStateNode(states[i], statesContextModel.StateNodeModels[i], states);

            return statesContext;
        }

        private StateNode ReadStateNode(StatesContext context, StateNodeModel stateNodeModel)
        {
            StateNode node = Fabric.Nodes.CreateStateNode(stateNodeModel.Name, context, stateNodeModel.Position);
            context.Add(node);
            return node;
        }

        private void AppendTransitionsToStateNode(StateNode node, StateNodeModel nodeModel, IEnumerable<StateNode> otherStates)
        {
            node.Transitions = nodeModel.OutgoingTransitions.Select(transitionModel =>
            {
                StateNode target = otherStates.First(targetState => targetState.Name == transitionModel.TargetName);
                StateTransition transition = Fabric.Transitions.CreateTransition(node, target);
                ReadTransitionContext(transition, transitionModel.ContextModel);
                return transition;
            }).ToList();
        }

        private void ReadTransitionContext(StateTransition transition, TransitionContextModel transitionContextModel)
        {
            foreach (ConditionalNodeModel conditionalNodeModel in transitionContextModel.ConditionalNodeModels)
            {
                ConditionalNode node = conditionalNodeModel.NodeKind switch
                {
                    nameof(NotNode) => Fabric.Nodes.ConditionalNotNode(transition.Context, conditionalNodeModel.Position),
                    nameof(OrNode) => Fabric.Nodes.ConditionalOrNode(transition.Context, conditionalNodeModel.Position),
                    nameof(AndNode) => Fabric.Nodes.ConditionalAndNode(transition.Context, conditionalNodeModel.Position),
                    nameof(ConditionNode) => Fabric.Nodes.ConditionalConditionNode(transition.Context, default), // ToDo
                    _ => throw new ArgumentOutOfRangeException(),
                };
                transition.Context.ProcessNewNode(node);
            }
        }

        #endregion
    }
}