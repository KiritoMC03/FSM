using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor.Extensions
{
    public static class IPanelExtensions
    {
        private static readonly List<VisualElement> DefaultBuffer = new List<VisualElement>(30);

        public static T Pick<T>(this IPanel panel, Vector2 point, Predicate<T> predicate, List<VisualElement> buffer = default)
            where T: VisualElement
        {
            buffer ??= DefaultBuffer;
            buffer.Clear();
            panel.PickAll(point, buffer);
            foreach (VisualElement element in buffer)
                if (element is T t && predicate(t))
                    return t;
            return default;
        }

        public static T Pick<T>(this IPanel panel, Vector2 point, List<VisualElement> buffer = default)
            where T: VisualElement
        {
            buffer ??= DefaultBuffer;
            buffer.Clear();
            panel.PickAll(point, buffer);
            foreach (VisualElement element in buffer)
                if (element is T t)
                    return t;
            return default;
        }
    }
}