﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public enum ConnectionPosition
    {
        Left,
        Right,
    }

    public static class NodeBuilder
    {
        public static (VisualElement Header, VisualElement Label) DefaultHeader<T>(this T node, string nodeName)
            where T : Node
        {
            VisualElement header = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            Label label;
            header.Add(label = new Label(nodeName)
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            });
            node.Add(header);
            node.Header = header;
            node.Label = label;
            return (header, label);
        }

        public static TextInputBaseField<string> WithInputLabel(this VisualElement parent, string nodeName)
        {
            TextField field = new TextField()
            {
                value = nodeName,
                style =
                {
                    alignContent = Align.Center,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    width = new StyleLength(new Length(90, LengthUnit.Percent)),
                },
            };
            parent.Add(field);
            return field;
        }
    }
}