using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class NodesContext : VisualElement
    {
        
    }
    public abstract class NodesContext<T> : NodesContext
        where T: Node
    {
        public string Name;
        public List<T> Nodes = new List<T>();
        public List<T> SelectedNodes = new List<T>();

        public virtual void Remove(T node)
        {
            Nodes.Remove(node);
            SelectedNodes.Remove(node);
            ((VisualElement)this).Remove(node);
        }
    }
}