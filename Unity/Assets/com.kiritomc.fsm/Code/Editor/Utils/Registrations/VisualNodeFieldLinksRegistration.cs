using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading.Tasks;
using FSM.Runtime.Serialization;
using UnityEngine;

namespace FSM.Editor
{
    public class VisualNodeFieldLinksRegistration : ICancelable
    {
        public readonly Dictionary<string, VisualNodeLinkRegistration> Items = new Dictionary<string, VisualNodeLinkRegistration>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        public bool IsDisposed { get; private set; }

        public VisualNodeFieldLinksRegistration(
            VisualNode node, 
            Type fieldType,
            Action<string, VisualNodeWithLinkExit> gotHandler,
            Func<string, VisualNodeWithLinkExit> currentGetter)
        {
            IEnumerable<FieldInfo> fields = fieldType
                .GetFields()
                .Where(field => field.FieldType.IsGenericType && 
                                field.FieldType.GetGenericTypeDefinition() == typeof(ParamNode<>));
            foreach (FieldInfo fieldInfo in fields)
            {
                Type returnType = fieldInfo.FieldType.GetGenericArguments().First();
                Func<Task<VisualNodeWithLinkExit>> getter = NodeLinkRequest.NewAsync(node, LocalCheck);
                string prettyLinkName = $"{fieldInfo.Name} ({returnType.Pretty()})";
                Items.Add(fieldInfo.Name, new VisualNodeLinkRegistration(node, fieldInfo.Name, prettyLinkName, getter, gotHandler, currentGetter, LocalCheck).AddTo(disposables));
                bool LocalCheck(VisualNodeWithLinkExit target) => target.IsVisualFunctionNodeWithReturnType(returnType);
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            disposables?.Dispose();
        }

        public void Refresh(string fieldName, VisualNodeWithLinkExit target)
        {
            Items[fieldName].SetTarget(target);
        }
    }
}