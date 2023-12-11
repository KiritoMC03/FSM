using System;
using UnityEngine;

namespace FSM.Editor.Extensions
{
    public static partial class NodeExtensions
    {
        public static LineDrawerRegistration AddLineDrawerForConnection<T>(this Node target, Func<Vector2?> startGetter, Func<T> targetFieldGetter) 
            where T: NodeWithConnection
        {
            NodeConnectionDrawer drawer;
            target.Add(drawer = new NodeConnectionDrawer());
            return new LineDrawerRegistration(drawer, target, startGetter, GetEndPosition);
            Vector2? GetEndPosition() => targetFieldGetter.Invoke()?.GetAbsoluteConnectionPos();
        }

        public static LineDrawerRegistration AddLineDrawerFor(this Node parent, Func<Vector2?> startGetter, Func<Vector2?> endGetter)
        {
            NodeConnectionDrawer drawer;
            parent.Add(drawer = new NodeConnectionDrawer());
            return new LineDrawerRegistration(drawer, parent, startGetter, endGetter);
        }
    }
}