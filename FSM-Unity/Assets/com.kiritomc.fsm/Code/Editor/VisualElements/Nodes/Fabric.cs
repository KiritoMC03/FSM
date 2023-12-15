﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FSM.Editor.Extensions;
using FSM.Editor.Manipulators;
using FSM.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    public class Fabric
    {
        private VisualElement root;
        private EditorState EditorState => ServiceLocator.Instance.Get<EditorState>();

        public Fabric(VisualElement root)
        {
            this.root = root;
        }

        public static (Fabric Fabric, VisualElement Root) WithRoot(VisualElement rootVisualElement)
        {
            VisualElement result = new VisualElement()
            {
                focusable = true,
                style =
                {
                    width = new StyleLength(new Length(100, LengthUnit.Percent)),
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                },
            };
            rootVisualElement.Add(result);
            result.focusable = true;
            result.style.flexDirection = FlexDirection.Row;
            result.AddManipulator(new SaveWorldPointerPositionManipulator());
            return (new Fabric(result), result);
        }

        public LeftPanel LeftPanel()
        {
            LeftPanel result = new LeftPanel();
            root.Add(result);
            return result;
        }

        public NavigationPanel NavigationPanel()
        {
            NavigationPanel result = new NavigationPanel();
            root.Add(result);
            return result;
        }

        public OrNode TestConditional(TransitionContext transitionContext)
        {
            OrNode node = new OrNode(new OrLayoutNode());
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            node.AddManipulator(new RouteConnectionManipulator(EditorState, transitionContext));
            return node;
        }

        public SelectNodePopup CreateSelectNodePopup(IEnumerable<string> availableNodes, Action<string> selectedHandler)
        {
            SelectNodePopup result = new SelectNodePopup(availableNodes, selectedHandler);
            root.Add(result);
            Vector2 position = result.WorldToLocal(EditorState.PointerPosition.Value);
            result.style.position = Position.Absolute;
            result.style.left = position.x;
            result.style.top = position.y;
            return result;
        }

        public StateNode CreateStateNode(string name, StatesContext statesContext, Vector2 position = default)
        {
            StateNode node = new StateNode(name, position);
            node.AddManipulator(new StateNodeLabelManipulator(node, EditorState.DraggingLocked, changed =>
            {
                string newName = changed.newValue;
                int num = 1;
                if (!statesContext.StateNodes.Exists(i => i.StateName == newName)) return newName;
                while (statesContext.StateNodes.Exists(i => i.StateName == $"{newName} {num}")) num++;
                return $"{newName} {num}";
            }));
            node.AddManipulator(new DraggerManipulator(EditorState.DraggingLocked));
            node.AddManipulator(new CreateTransitionManipulator(EditorState, statesContext));
            return node;
        }

        #region Transitions

        public async Task<StateTransition> RouteTransitionAsync<T>(T source, Func<Task<T>> asyncTargetGetter, Predicate<T> checkValid)
            where T : StateNode
        {
            T target = await asyncTargetGetter.Invoke();
            if (target == null || target == source || !checkValid.Invoke(target)) return default;
            return CreateTransition(source, target);
        }

        public StateTransition CreateTransition<T>(T source, T target)
            where T : StateNode
        {
            StateTransition transition = new StateTransition(source, target);
            source.Add(transition);
            source.Disposables.Add(transition);
            source.Disposables.Add(target.ListenChanges(RepaintTransition));
            source.ChildrenRepaintHandler.Add(transition);
            return transition;
            void RepaintTransition() => transition.Repaint();
        }

        #endregion

        #region Contexts

        public StatesContext CreateRootContext()
        {
            Context current = EditorState.CurrentContext.Value;
            current?.parent.Remove(current);
            EditorState.RootContext.Value ??= new StatesContext("Root");
            EditorState.RootContext.Value.style.display = DisplayStyle.Flex;
            EditorState.CurrentContext.Value = EditorState.RootContext.Value;
            root.Add(EditorState.RootContext.Value);
            return EditorState.RootContext.Value;
        }

        public StatesContext CreateStateContext(StateNode target)
        {
            EditorState.RootContext.Value.style.display = DisplayStyle.None;
            Context current = EditorState.CurrentContext.Value;
            current?.parent.Remove(current);
            StatesContext result;
            root.Add(result = new StatesContext(target.StateName));
            EditorState.CurrentContext.Value = result;
            return result;
        }

        public TransitionContext CreateTransitionContext(StateTransition target)
        {
            Context current = EditorState.CurrentContext.Value;
            current?.parent.Remove(current);
            TransitionContext result;
            root.Add(result = new TransitionContext(target, $"{target.Source.StateName} -> {target.Target.StateName}"));
            EditorState.CurrentContext.Value = result;
            return result;
        }

        #endregion
    }
}