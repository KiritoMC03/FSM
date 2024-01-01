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

        public VisualElement Content { get; protected set; }

        public abstract int GetFreeId();
        public abstract void ReserveId<T>(int id, T node) where T: VisualNode;
        public abstract void MoveNodes(Vector2 offset);
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
            Content.Add(node);
            Nodes.Add(node);
        }

        public virtual void Remove(T node)
        {
            Content.Remove(node);
            Nodes.Remove(node);
            SelectedNodes.Remove(node);
            NodesIds.Remove(node.Id);
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
            int id = 0;
            while (NodesIds.ContainsKey(id)) id++;
            return id;
        }

        public override void MoveNodes(Vector2 offset)
        {
            foreach (T node in Nodes) node.Placement += offset;
            foreach (T node in Nodes) node.Repaint();
        }

        protected VisualElement BuildContentContainer()
        {
            Content = new VisualElement()
            {
                style =
                {
                    width = new StyleLength(new Length(100f, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100f, LengthUnit.Percent)),
                    position = Position.Absolute,
                },
            };
            Add(Content);
            return Content;
        }

        public void ScaleContent(float delta)
        {
            Vector3 scale = Content.resolvedStyle.scale.value;
            float axisValue = scale.x + delta;
            axisValue = Mathf.Clamp(axisValue, 0.1f, 3f);
            Content.style.scale = new Vector2(axisValue, axisValue);
            foreach (T node in Nodes) node.Repaint();
        }
    }
}