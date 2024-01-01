using System;
using FSM.Runtime;
using FSM.Runtime.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FSM.Example
{
    public class EmulatorBehaviour : FsmAgent
    {
        public bool moving;
        public bool seeEnemy;
        public string[] weapons;
        private FsmData data;

        public static EmulatorBehaviour Instance { get; private set; }
        public static string SelectedWeapon { get; set; }

        private void Start()
        {
            Instance = this;
            data = new FsmData(new[] { AsRunnable() });
        }

        private void Update()
        {
            new FsmUpdater().Update(data);
        }
    }

    [Serializable]
    public class IsSeeEnemyCondition : ICondition
    {
        public bool Execute()
        {
            return EmulatorBehaviour.Instance.seeEnemy;
        }
    }

    [Serializable]
    public class IsMovingCondition : ICondition
    {
        public bool Execute()
        {
            return EmulatorBehaviour.Instance.moving;
        }
    }

    [Serializable]
    public class RunAction : IAction
    {
        public void Execute()
        {
            Debug.Log($"Moved next");
        }
    }

    [Serializable]
    public class WaitingAction : IAction
    {
        public void Execute()
        {
            Debug.Log($"Waiting...");
        }
    }

    [Serializable]
    public class SelectWeaponAction : IAction
    {
        public void Execute()
        {
            EmulatorBehaviour.SelectedWeapon = EmulatorBehaviour.Instance.weapons[Random.Range(0, EmulatorBehaviour.Instance.weapons.Length)];
        }
    }

    [Serializable]
    public class UseWeaponAction : IAction
    {
        public void Execute()
        {
            Debug.Log($"Weapon used: {EmulatorBehaviour.SelectedWeapon}");
        }
    }
}