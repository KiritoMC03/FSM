using System;
using UnityEngine;

namespace FSM.Editor.Serialization
{
    public class StatesContextModel
    {
        public StateNodeModel[] StateNodeModels;

        public StatesContextModel()
        {
        }

        public StatesContextModel(StateNodeModel[] stateNodeModels)
        {
            StateNodeModels = stateNodeModels;
        }
    }

    [Serializable]
    public class StateNodeModel
    {
        public string Name;
        public Vector2Model Position;
        public StateTransitionModel[] OutgoingTransitions;

        public StateNodeModel()
        {
        }

        public StateNodeModel(string name, Vector2Model position, StateTransitionModel[] outgoingTransitions)
        {
            Name = name;
            Position = position;
            OutgoingTransitions = outgoingTransitions;
        }
    }

    [Serializable]
    public class StateTransitionModel
    {
        public string SourceName;
        public string TargetName;
        public TransitionContextModel ContextModel;

        public StateTransitionModel()
        {
        }

        public StateTransitionModel(string sourceName, string targetName, TransitionContextModel contextModel)
        {
            SourceName = sourceName;
            TargetName = targetName;
            ContextModel = contextModel;
        }
    }

    [Serializable]
    public class TransitionContextModel
    {
        public ConditionalNodeModel[] ConditionalNodeModels;

        public TransitionContextModel()
        {
        }

        public TransitionContextModel(ConditionalNodeModel[] conditionalNodeModels)
        {
            ConditionalNodeModels = conditionalNodeModels;
        }
    }

    [Serializable]
    public class ConditionalNodeModel
    {
        public string Name;
        public Vector2Model Position;
        public string NodeKind;
        public string LeftConnectionName;
        public string RightConnectionName;

        public ConditionalNodeModel()
        {
        }

        public ConditionalNodeModel(string name, Vector2Model position, string nodeKind, string leftConnectionName, string rightConnectionName)
        {
            Name = name;
            Position = position;
            NodeKind = nodeKind;
            LeftConnectionName = leftConnectionName;
            RightConnectionName = rightConnectionName;
        }
    }

    [Serializable]
    public struct Vector2Model
    {
        public float X;
        public float Y;

        public Vector2Model(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(Vector2Model v) => new Vector2(v.X, v.Y);
        public static explicit operator Vector2Model(Vector2 v) => new Vector2Model(v.x, v.y);
    }
}