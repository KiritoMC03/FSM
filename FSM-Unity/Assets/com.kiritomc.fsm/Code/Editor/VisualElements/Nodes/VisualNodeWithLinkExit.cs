using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public abstract class VisualNodeWithLinkExit : VisualNode, IVisualNodeWithLinkExit
    {
        public enum VisualNodeLinkPosition
        {
            Left,
            Right,
        }

        protected NodeLinkPoint Link;

        protected VisualNodeWithLinkExit(string name) : base(name)
        {
            Link = LinkPointRelativeTo(Header, Label, VisualNodeLinkPosition.Right);
        }

        public virtual Vector2 GetAbsoluteLinkPointPos()
        {
            return new Vector2(Link.worldBound.x + Sizes.ConnectionNodePoint / 2f, Link.worldBound.y + Sizes.ConnectionNodePoint / 2f);
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