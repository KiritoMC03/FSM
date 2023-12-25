using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNodesContext : VisualElement
    {
        public string Name;
        protected EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();
        private Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();
        
    }
    public abstract class VisualNodesContext<T> : VisualNodesContext
        where T: VisualNode
    {
        public List<T> Nodes = new List<T>();
        public List<T> SelectedNodes = new List<T>();

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
            ((VisualElement)this).Remove(node);
        }
    }
}