﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNodeWithLinkExit : VisualNode
    {
        public enum VisualNodeLinkPosition
        {
            Left,
            Right,
        }

        protected NodeLinkPoint Link;

        protected VisualNodeWithLinkExit(string name, int id) : base(name, id)
        {
            Link = LinkPointRelativeTo(Header, Label, VisualNodeLinkPosition.Right);
        }

        public virtual Vector2 GetAbsoluteLinkPointPos()
        {
            Vector2 position = Link.worldTransform.GetPosition();
            float toCenter = Sizes.ConnectionNodePoint / 2f * worldTransform.lossyScale.x;
            position.x += toCenter;
            position.y += toCenter;
            return position;
        }

        public static NodeLinkPoint LinkPointRelativeTo(VisualElement parent, VisualElement relative, VisualNodeLinkPosition position)
        {
            NodeLinkPoint link;
            parent.Add(link = new NodeLinkPoint());
            switch (position)
            {
                case VisualNodeLinkPosition.Left:
                    link.PlaceBehind(relative);
                    break;
                case VisualNodeLinkPosition.Right:
                    link.PlaceInFront(relative);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
            return link;
        }
    }
}