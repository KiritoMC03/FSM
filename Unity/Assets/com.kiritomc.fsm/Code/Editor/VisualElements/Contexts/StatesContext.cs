﻿using System;
using System.Collections.Generic;
using FSM.Editor.Manipulators;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class StatesContext : VisualNodesContext<VisualStateNode>
    {
        public StatesContext(string name)
        {
            Name = name;
            this.DefaultLayout()
                .DefaultColors()
                .DefaultInteractions();
            style.overflow = Overflow.Hidden;
            this.AddManipulator(new ContextDraggingManipulator<VisualStateNode>(this));
            this.AddManipulator(new ScaleContextManipulator<VisualStateNode>(this));
            this.AddManipulator(new CreateVisualNodeManipulator<VisualStateNode>(GetAvailableNodes));
            this.AddManipulator(new SelectVisualNodesManipulator<VisualStateNode>(this));
            this.AddManipulator(new DeleteVisualStateNodeManipulator<VisualStateNode>(this));
            this.AddManipulator(new OpenTransitionInContextManipulator(this));
            BuildContentContainer();
        }

        public Dictionary<string, Action> GetAvailableNodes()
        {
            return new Dictionary<string, Action>
            {
                {"Simple State", () =>
                    {
                        const string nodeName = "Simple state";
                        int num = 0;
                        while (Nodes.Exists(i => i.Name == $"{nodeName} {num}")) num++;
                        VisualStateNode node = new VisualStateNode($"{nodeName} {num}", this, this.WorldToLocal(EditorState.PointerPosition.Value));
                        ProcessNewNode(node);
                    }
                },
            };
        }

        public override void Remove(VisualStateNode node)
        {
            base.Remove(node);
            foreach (VisualStateNode other in Nodes)
            {
                for (int i = other.Transitions.Count - 1; i >= 0; i--)
                {
                    VisualStateTransitionData transitionData = other.Transitions[i];
                    if (transitionData.Transition.Target == node || transitionData.Transition.Source == node) other.RemoveTransitionAt(i);
                }
            }
        }
    }
}