using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FSM.Runtime;
using FSM.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

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
                stateNode.Id,
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

                contextEntryNodeModels[i] = new VisualNodeModel(type, node.Id, (Vector2Model)node.ResolvedPlacement, linked);
            }

            TransitionContextModel contextModel = new TransitionContextModel(transition.Context.AnchorNode.ConditionLink?.Id ?? -1, contextEntryNodeModels);
            return new StateTransitionModel(sourceNode.Name, transition.Target.Name, contextModel);

            Dictionary<string, int> LinkedToIndexes(Dictionary<string, VisualNodeWithLinkExit> linked)
            {
                return linked.ToDictionary(
                    pair => pair.Key, 
                    pair => transition.Context.GetIdOf(pair.Value));
            }
        }

        private StateNodeLifecycleModel WriteStateNodeLifecycle(VisualStateNode stateNode)
        {
            StateNodeLifecycleModel result = new StateNodeLifecycleModel
            {
                AnchorNodePosition = (Vector2Model)stateNode.Context.AnchorNode.ResolvedPlacement,
                OnEnterId = stateNode.Context.GetIdOf(stateNode.Context.AnchorNode.OnEnterLink),
                OnUpdateId = stateNode.Context.GetIdOf(stateNode.Context.AnchorNode.OnUpdateLink),
                OnExitId = stateNode.Context.GetIdOf(stateNode.Context.AnchorNode.OnExitLink),
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
                result.Nodes[i] = new VisualNodeModel(type, node.Id, (Vector2Model)node.ResolvedPlacement, linked);
            }
            return result;

            Dictionary<string, int> LinkedToIndexes(Dictionary<string, VisualNodeWithLinkExit> linked)
            {
                return linked.ToDictionary(
                    pair => pair.Key, 
                    pair => stateNode.Context.GetIdOf(pair.Value));
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
            VisualStateNode node = new VisualStateNode(stateNodeModel.Name, stateNodeModel.Id, context, stateNodeModel.Position, stateNodeModel.Lifecycle?.AnchorNodePosition ?? default);
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

            transition.Context.AnchorNode.ConditionLinkRegistration.SetTarget(transition.Context.GetById(transitionContextModel.ConditionAnchorId));
            for (int i = 0; i < transitionContextModel.ConditionalNodeModels.Length; i++)
            {
                VisualNodeModel contextEntryNodeModels = transitionContextModel.ConditionalNodeModels[i];
                foreach ((string fieldName, int linkIndex) in contextEntryNodeModels.Linked)
                {
                    nodes[i].ForceLinkTo(fieldName, transition.Context.GetById(linkIndex));
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
                    context.ProcessNewNode(nodes[i] = new VisualActionNode(nodeModel.Type, nodeModel.Id, context, nodeModel.Position));
                }
                else if (nodeModel.Type.IsIFunction())
                {
                    context.ProcessNewNode(nodes[i] = new VisualFunctionNode(nodeModel.Type, nodeModel.Id, context, nodeModel.Position));
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
                    nodes[i].ForceLinkTo(fieldName, node.Context.GetById(linkIndex));
                }
            }

            void Link(int index, string name)
            {
                if (index != -1) context.AnchorNode.ForceLinkTo(name, node.Context.GetById(index));
            }
        }

        #endregion
    }

    public class LogicSerializer
    {
        public static List<StateModel> Serialize(FsmContext rootContext)
        {
            List<StateModel> results = new List<StateModel>();
            foreach (VisualStateNode node in rootContext.StatesContext.Nodes)
            {
                Dictionary<string, AbstractSerializableType<ConditionalLayoutNodeModel>> transitions = new Dictionary<string, AbstractSerializableType<ConditionalLayoutNodeModel>>();
                foreach (VisualStateTransition transition in node.Transitions)
                {
                    ConditionalLayoutNodeModel root = SerializeConditionalNode(transition.Context.AnchorNode.ConditionLink);
                    if (root != null)
                        transitions.Add(
                            transition.Target.Name,
                            new AbstractSerializableType<ConditionalLayoutNodeModel>(root));
                }

                AbstractSerializableType<ActionLayoutNodeModel> onEnter = new AbstractSerializableType<ActionLayoutNodeModel>(SerializeActionNode(node.Context.AnchorNode.OnEnterLink));
                AbstractSerializableType<ActionLayoutNodeModel> onUpdate = new AbstractSerializableType<ActionLayoutNodeModel>(SerializeActionNode(node.Context.AnchorNode.OnEnterLink));
                AbstractSerializableType<ActionLayoutNodeModel> onExit = new AbstractSerializableType<ActionLayoutNodeModel>(SerializeActionNode(node.Context.AnchorNode.OnEnterLink));
                results.Add(new StateModel(node.Name, transitions, onEnter, onUpdate, onExit));
            }
            return results;
        }

        public static ConditionalLayoutNodeModel SerializeConditionalNode(VisualNodeWithLinkExit visualNode)
        {
            if (visualNode == null) return default;
            if (visualNode is VisualFunctionNode functionNode)
            {
                if (functionNode.FunctionType.IsNot())
                {
                    functionNode.Linked.TryGetValue(nameof(Not.Value), out VisualNodeWithLinkExit linked);
                    return new NotLayoutNodeModel(SerializeConditionalNode(linked));
                } 
                if (functionNode.FunctionType.IsOr())
                {
                    functionNode.Linked.TryGetValue(nameof(Or.Left), out VisualNodeWithLinkExit leftLinked);
                    functionNode.Linked.TryGetValue(nameof(Or.Right), out VisualNodeWithLinkExit rightLinked);
                    return new OrLayoutNodeModel(SerializeConditionalNode(leftLinked), SerializeConditionalNode(rightLinked));
                }
                if (functionNode.FunctionType.IsAnd())
                {
                    functionNode.Linked.TryGetValue(nameof(And.Left), out VisualNodeWithLinkExit leftLinked);
                    functionNode.Linked.TryGetValue(nameof(And.Right), out VisualNodeWithLinkExit rightLinked);
                    return new AndLayoutNodeModel(SerializeConditionalNode(leftLinked), SerializeConditionalNode(rightLinked));
                }
            }
            else if (visualNode is VisualConditionNode conditionNode)
            {
                return new ConditionLayoutNodeModel((ICondition)Activator.CreateInstance(conditionNode.ConditionType));
            }

            Debug.LogError("");
            return default;
        }

        public static ActionLayoutNodeModel SerializeActionNode(VisualActionNode visualNode)
        {
            ActionLayoutNodeModel root = new ActionLayoutNodeModel();
            ActionLayoutNodeModel current = root;
            while (visualNode != null)
            {
                object nodeObject = Activator.CreateInstance(visualNode.ActionType);
                SerializeFields(nodeObject, visualNode.ActionType, visualNode);
                current.Action = new AbstractSerializableType<IAction>((IAction)nodeObject);
                visualNode = visualNode.DependentAction;
                if (visualNode != null)
                {
                    ActionLayoutNodeModel prev = current;
                    current = new ActionLayoutNodeModel();
                    prev.Connection = new AbstractSerializableType<ActionLayoutNodeModel>(current);
                }
            }

            return root;
        }

        public static void SerializeFields(object nodeObject, Type nodeType, VisualNodeWithLinkFields visualNodeWithLinkFields)
        {
            foreach ((string fieldName, VisualNodeWithLinkExit linkedNode) in visualNodeWithLinkFields.Linked)
            {
                FieldInfo fieldInfo = nodeType.GetField(fieldName);
                // Type fieldType = typeof(ParamNode<>);
                // Type fieldTypeArg = fieldInfo.FieldType.GetGenericArguments().First();
                // fieldType = fieldType.MakeGenericType(fieldTypeArg);
                // object fieldObject = Activator.CreateInstance(fieldType);
                object fieldValueObject = Activator.CreateInstance(((VisualFunctionNode)linkedNode).FunctionType);
                fieldInfo.SetValue(nodeObject, fieldValueObject);
            }
        }
    }
}