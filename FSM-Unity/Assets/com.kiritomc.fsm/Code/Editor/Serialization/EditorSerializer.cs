using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FSM.Editor.Serialization
{
    public class EditorSerializer
    {
        private readonly EditorState editorState;
        private readonly Fabric fabric;

        public EditorSerializer(EditorState editorState, Fabric fabric)
        {
            this.editorState = editorState;
            this.fabric = fabric;
        }

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
                stateNode.StateName,
                new Vector2Model(stateNode.resolvedStyle.left, stateNode.resolvedStyle.top),
                stateNode.Transitions.Select(transition => WriteStateTransition(stateNode, transition)).ToArray()
            );
        }

        private StateTransitionModel WriteStateTransition(StateNode sourceNode, StateTransition transition)
        {
            return new StateTransitionModel(sourceNode.StateName, transition.Target.StateName, default); // ToDo
        }

        #endregion

        #region Deserialization

        public FsmContext Deserialize(string json)
        {
            FsmContextModel fsmContextModel = JsonConvert.DeserializeObject<FsmContextModel>(json);
            FsmContext fsmContext = new FsmContext
            {
                StatesContext = ReadStatesContext(fsmContextModel.StatesContextModel, isRoot: true),
            };
            return fsmContext;
        }

        private StatesContext ReadStatesContext(StatesContextModel statesContextModel, StateNode target = default, bool isRoot = false)
        {            
            if (statesContextModel == null) return fabric.CreateRootContext();
            StatesContext statesContext = isRoot ? fabric.CreateRootContext() : fabric.CreateStateContext(target);
            List<StateNode> states = statesContextModel.StateNodeModels.Select(nodeModel => ReadStateNode(statesContext, nodeModel)).ToList();
            statesContext.StateNodes = states;
            for (int i = 0; i < states.Count; i++) AppendTransitionsToStateNode(states[i], statesContextModel.StateNodeModels[i], states);

            return statesContext;
        }

        private StateNode ReadStateNode(StatesContext context, StateNodeModel stateNodeModel)
        {
            StateNode node = fabric.CreateStateNode(stateNodeModel.Name, context, stateNodeModel.Position);
            context.Add(node);
            return node;
        }

        private void AppendTransitionsToStateNode(StateNode node, StateNodeModel nodeModel, IEnumerable<StateNode> otherStates)
        {
            node.Transitions = nodeModel.OutgoingTransitions.Select(transitionModel =>
            {
                StateNode target = otherStates.First(targetState => targetState.StateName == transitionModel.TargetName);
                StateTransition transition = fabric.CreateTransition(node, target);
                return transition;
            }).ToList();
        }

        #endregion
    }
}