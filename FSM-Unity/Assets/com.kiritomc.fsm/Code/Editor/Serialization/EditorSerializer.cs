﻿using System;
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
                stateNode.Transitions.Select(transition => WriteStateTransition(stateNode, transition)).ToArray()
            );
        }

        private StateTransitionModel WriteStateTransition(VisualStateNode sourceNode, VisualStateTransition transition)
        {
            ConditionalNodeModel[] conditionalNodeModels = new ConditionalNodeModel[transition.Context.Nodes.Count];
            for (int i = 0; i < transition.Context.Nodes.Count; i++)
            {
                VisualNodeWithLinkExit conditionalNode = transition.Context.Nodes[i];
                (string kind, VisualConditionNode left, VisualConditionNode right) = conditionalNode switch
                {
                    // NotNode not => (nameof(NotNode), not.Input.Value, default),
                    // OrNode or => (nameof(OrNode), or.Left.Value, or.Right.Value),
                    // AndNode and => (nameof(AndNode), and.Left.Value, and.Right.Value),
                    _ => (nameof(VisualConditionNode), default(VisualConditionNode), default(VisualConditionNode)),
                };

                int leftIndex = transition.Context.Nodes.IndexOf(left);
                int rightIndex = transition.Context.Nodes.IndexOf(right);
                conditionalNodeModels[i] = new ConditionalNodeModel(conditionalNode.Name, (Vector2Model)conditionalNode.ResolvedPlacement, kind, leftIndex, rightIndex);
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
            foreach (ConditionalNodeModel conditionalNodeModel in transitionContextModel.ConditionalNodeModels)
            {
                // VisualConditionNode node = conditionalNodeModel.NodeKind switch
                // {
                //     // nameof(NotNode) => Fabric.Nodes.ConditionalNotNode(transition.Context, conditionalNodeModel.Position),
                //     // nameof(OrNode) => Fabric.Nodes.ConditionalOrNode(transition.Context, conditionalNodeModel.Position),
                //     // nameof(AndNode) => Fabric.Nodes.ConditionalAndNode(transition.Context, conditionalNodeModel.Position),
                //     // nameof(ConditionNode) => Fabric.Nodes.ConditionalConditionNode(transition.Context, default), // ToDo
                //     // _ => throw new ArgumentOutOfRangeException(),
                // };
                // transition.Context.ProcessNewNode(node);
            }

            for (int i = 0; i < transition.Context.Nodes.Count; i++)
            {
                VisualNodeWithLinkExit node = transition.Context.Nodes[i];
                int leftId = transitionContextModel.ConditionalNodeModels[i].LeftConnectionId;
                int rightId = transitionContextModel.ConditionalNodeModels[i].RightConnectionId;
                switch (node)
                {
                    // case NotNode not:
                    //     not.Input.Value = leftId == -1 ? default : transition.Context.Nodes[leftId];
                    //     break;
                    // case ConditionGateNode gate:
                    //     gate.Left.Value = leftId == -1 ? default : transition.Context.Nodes[leftId];
                    //     gate.Right.Value = rightId == -1 ? default : transition.Context.Nodes[rightId];
                    //     break;
                }
                node.Repaint();
            }
            return;
        }

        #endregion
    }
}