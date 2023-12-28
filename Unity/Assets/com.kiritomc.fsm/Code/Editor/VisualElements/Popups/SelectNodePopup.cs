using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class SelectNodePopup : VisualElement
    {
        private Vector3 lastPointerDownPosition;
        private bool isSelected;

        public SelectNodePopup(IEnumerable<string> availableNodes, Action<string> selectedHandler)
        {
            style.width = 130;
            style.height = 250;
            ScrollView scrollView = new ScrollView(ScrollViewMode.Vertical)
            {
                style =
                {
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                    backgroundColor = Colors.CreateNodePopupBackground,
                },
            };
            Add(scrollView);
            foreach (string availableNode in availableNodes)
            {
                SelectNodePopupItem item = new SelectNodePopupItem(availableNode)
                {
                    style =
                    {
                        backgroundColor = Colors.CreateNodePopupItemBackground,
                        marginBottom = Sizes.CreateNodePopupItemsSpacing,
                    },
                };
                item.RegisterCallback<PointerDownEvent>(HandlePointerDown);
                item.RegisterCallback<PointerUpEvent>(HandlePointerUp);
                scrollView.Add(item);

                continue;
                void HandlePointerDown(PointerDownEvent e) => lastPointerDownPosition = e.position;
                void HandlePointerUp(PointerUpEvent e)
                {
                    if (isSelected) return;
                    if (e.position != lastPointerDownPosition) return;
                    isSelected = true;
                    selectedHandler?.Invoke(availableNode);
                    parent.Remove(this);
                }
            }
        } 
    }

    public class SelectNodePopupItem : VisualElement
    {
        public SelectNodePopupItem(string name)
        {
            Add(new Label(name)
            {
                style =
                {
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                },
            });
        }
    }
}