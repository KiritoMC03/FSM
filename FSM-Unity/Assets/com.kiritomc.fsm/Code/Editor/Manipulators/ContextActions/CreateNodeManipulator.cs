using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    public delegate Dictionary<string, Func<TNode>> GetAvailableNodesDelegate<TNode>() where TNode: Node;

    public class CreateNodeManipulator<TNode> : Manipulator
        where TNode: Node
    {
        private readonly Fabric fabric;
        private readonly GetAvailableNodesDelegate<TNode> nodeTypesList;

        public CreateNodeManipulator(Fabric fabric, GetAvailableNodesDelegate<TNode> nodeTypesList)
        {
            this.fabric = fabric;
            this.nodeTypesList = nodeTypesList;
        }

        public CreateNodeManipulator(Fabric fabric)
        {
            this.fabric = fabric;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<KeyUpEvent>(HandleKeyUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<KeyUpEvent>(HandleKeyUp);
        }

        private void HandleKeyUp(KeyUpEvent e)
        {
            if (e.keyCode == Keys.CreateNode)
            {
                Dictionary<string, Func<TNode>> nodes = nodeTypesList.Invoke();
                fabric.CreateSelectNodePopup(nodes.Keys, CreateNodeLocal);
                void CreateNodeLocal(string selected) => CreateNode(nodes[selected]);
            }
        }

        private void CreateNode(Func<TNode> createFunc)
        {
            createFunc?.Invoke();
        }
    }
}