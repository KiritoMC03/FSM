using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNodesContext : VisualElement
    {
        public string Name;

        protected EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();

        public abstract int GetFreeId();
        public abstract void ReserveId<T>(int id, T node) where T: VisualNode;
    }

    public abstract class VisualNodesContext<T> : VisualNodesContext
        where T: VisualNode
    {
        public List<T> Nodes = new List<T>();
        public List<T> SelectedNodes = new List<T>();
        protected Dictionary<int, T> NodesIds = new Dictionary<int, T>();

        public void Open()
        {
            EditorState.EditorRoot.Value.Remove(EditorState.CurrentContext.Value);
            EditorState.EditorRoot.Value.Add(this);
            EditorState.CurrentContext.Value = this;
        }

        public virtual void ProcessNewNode(T node)
        {
            Add(node);
            Nodes.Add(node);
        }

        public virtual void Remove(T node)
        {
            Nodes.Remove(node);
            SelectedNodes.Remove(node);
            NodesIds.Remove(node.Id);
            ((VisualElement)this).Remove(node);
        }

        public T GetById(int id)
        {
            NodesIds.TryGetValue(id, out T result);
            return result;
        }

        public int GetIdOf(T node)
        {
            foreach ((int key, T value) in NodesIds)
                if (value == node) return key;
            return -1;
        }

        public override void ReserveId<T1>(int id, T1 node) => ReserveId(id, node as T);
        public void ReserveId(int id, T node) => NodesIds.Add(id, node);

        public override int GetFreeId()
        {
            Debug.Log($"GetFreeId");
            int id = 0;
            while (NodesIds.ContainsKey(id)) id++;
            return id;
        }
    }
}