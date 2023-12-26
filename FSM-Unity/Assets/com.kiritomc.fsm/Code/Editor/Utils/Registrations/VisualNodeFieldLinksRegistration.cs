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
            Action<string, IVisualNodeWithLinkExit> gotHandler,
            Func<string, IVisualNodeWithLinkExit> currentGetter)
        {
            IEnumerable<FieldInfo> fields = fieldType
                .GetFields()
                .Where(field => field.FieldType.IsGenericType && 
                                field.FieldType.GetGenericTypeDefinition() == typeof(ParamNode<>));
            foreach (FieldInfo fieldInfo in fields)
            {
                Type returnType = fieldInfo.FieldType.GetGenericArguments().First();
                Func<Task<IVisualNodeWithLinkExit>> getter = NodeLinkRequest.NewAsync(node, LocalCheck);
                string linkName = $"{fieldInfo.Name} ({returnType.Pretty()})";
                Items.Add(linkName, new VisualNodeLinkRegistration(node, linkName, getter, gotHandler, currentGetter, LocalCheck).AddTo(disposables));
                bool LocalCheck(IVisualNodeWithLinkExit target) => target.IsVisualFunctionNodeWithReturnType(returnType);
            }
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            disposables?.Dispose();
        }

        public void Refresh(string fieldName, IVisualNodeWithLinkExit target)
        {
            Items[fieldName].SetTarget(target);
        }
    }
}