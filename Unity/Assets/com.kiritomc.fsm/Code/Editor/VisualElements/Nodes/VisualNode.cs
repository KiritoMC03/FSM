﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNode : VisualElement, ICustomRepaintHandler
    {
        public string Name;
        public VisualElement Header;
        public TextElement Label;
        public readonly int Id;
        private readonly List<VisualNodeStyleRegistration> styleRegistrations = new List<VisualNodeStyleRegistration>();
        private bool isResolved;

        protected VisualNodeStyleRegistration BaseStyleRegistration;

        public Vector2 ResolvedPlacement => isResolved ? new Vector2(resolvedStyle.left, resolvedStyle.top) : Placement;

        public Vector2 Placement
        {
            get => new Vector2(style.left.value.value, style.top.value.value);
            set
            {
                style.left = value.x;
                style.top = value.y;
            }
        }

        protected Fabric Fabric => ServiceLocator.Instance.Get<Fabric>();
        protected EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public void AppendStyleRegistration(VisualNodeStyleRegistration registration)
        {
            styleRegistrations.Add(registration);
            Repaint();
        }

        public VisualNode(int id)
        {
            Id = id;
            BaseStyleRegistration = new VisualNodeStyleRegistration(this, () =>
            {
                style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = Sizes.NodePadding;
                style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = Colors.NodeBorderColor;
                style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = Sizes.NodeBorderWidth;
                style.borderTopLeftRadius = style.borderTopRightRadius = style.borderBottomLeftRadius = style.borderBottomRightRadius = Sizes.NodeBorderRadius;
                style.position = new StyleEnum<Position>(Position.Absolute);
                style.minWidth = 200;
                style.minHeight = 50;
                style.backgroundColor = Colors.NodeBackground;
            });
            RegisterCallback<GeometryChangedEvent>(HandleFirstDraw);
            void HandleFirstDraw(GeometryChangedEvent e)
            {
                UnregisterCallback<GeometryChangedEvent>(HandleFirstDraw);
                isResolved = true;
            }
        }

        public VisualNode(string nodeName, int id) : this(id)
        {
            Name = nodeName;
            Header = this.NodeHeader();
            Label = Header.NodeLabel(nodeName);
        }

        public virtual void Repaint()
        {
            for (int i = 0; i < styleRegistrations.Count; i++)
            {
                VisualNodeStyleRegistration current = styleRegistrations[i];
                if (current.IsDisposed)
                {
                    styleRegistrations.RemoveAt(i);
                    i--;
                }
                else current.Apply();
            }
        }

        public NodeChangingListeningRegistration OnChanged(Action action) => new NodeChangingListeningRegistration(this, action);
    }
}