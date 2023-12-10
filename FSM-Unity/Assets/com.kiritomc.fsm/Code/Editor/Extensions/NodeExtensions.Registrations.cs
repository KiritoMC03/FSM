using System;
using UnityEngine;

namespace FSM.Editor.Extensions
{
    public static partial class NodeExtensions
    {
        public static LineDrawerRegistration AddLineDrawerForConnection<T>(this Node target, Func<Vector2?> startGetter, Func<T> targetFieldGetter) 
            where T: NodeWithConnection
        {
            LineDrawer drawer;
            target.Add(drawer = new LineDrawer());
            return new LineDrawerRegistration(drawer, target, startGetter, GetEndPosition);
            Vector2? GetEndPosition() => targetFieldGetter.Invoke()?.GetAbsoluteConnectionPos();
        }

        public static LineDrawerRegistration AddLineDrawerForTransition(this StateTransition target, Func<Vector2?> startGetter, Func<Vector2?> targetGetter)
        {
            LineDrawer drawer;
            target.Add(drawer = new LineDrawer());
            return new LineDrawerRegistration(drawer, target, startGetter, targetGetter);
        }

        public static LineDrawerRegistration AddLineDrawerFor(this Node parent, Func<Vector2?> startGetter, Func<Vector2?> endGetter)
        {
            LineDrawer drawer;
            parent.Add(drawer = new LineDrawer());
            return new LineDrawerRegistration(drawer, parent, startGetter, endGetter);
        }
    }
}