﻿using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FSM.Editor.Manipulators
{
    /// <summary>
    /// Action - createRequestedCallback
    /// </summary>
    public delegate Dictionary<string, Action> GetAvailableNodesAndCallbacksDelegate();

    public class CreateNodeManipulator<TNode> : Manipulator
        where TNode: Node
    {
        private readonly GetAvailableNodesAndCallbacksDelegate nodeAndCallbacksList;
        private static Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public CreateNodeManipulator()
        {
        }

        public CreateNodeManipulator(GetAvailableNodesAndCallbacksDelegate nodeAndCallbacksList)
        {
            this.nodeAndCallbacksList = nodeAndCallbacksList;
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
                Dictionary<string, Action> nodesCreating = nodeAndCallbacksList.Invoke();
                Fabric.CreateSelectNodePopup(nodesCreating.Keys, CreateNodeLocal);
                void CreateNodeLocal(string selected) => RequestCreate(nodesCreating[selected]);
            }
        }

        private void RequestCreate(Action createRequestedCallback)
        {
            createRequestedCallback?.Invoke();
        }
    }
}