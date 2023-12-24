using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading.Tasks;
using FSM.Runtime.Serialization;

namespace FSM.Editor
{
    public class VisualNodeFieldLinksRegistration : ICancelable
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly Dictionary<string, VisualNodeLinkRegistration> items = new Dictionary<string, VisualNodeLinkRegistration>();
        public bool IsDisposed { get; private set; }

        public VisualNodeFieldLinksRegistration(
            VisualNode node, 
            Type fieldType,
            Func<Task<IVisualNodeWithLinkExit>> asyncTargetGetter,
            Action<string, IVisualNodeWithLinkExit> gotHandler,
            Func<string, IVisualNodeWithLinkExit> currentGetter)
        {
            IEnumerable<FieldInfo> fields = fieldType
                .GetFields()
                .Where(field => field.FieldType.IsGenericType && 
                                field.FieldType.GetGenericTypeDefinition() == typeof(ParamNode<>));
            foreach (FieldInfo fieldInfo in fields)
            {
                // FieldWrapper<VisualFunctionNode> fieldWrapper = new FieldWrapper<VisualFunctionNode>();
                // object instance = Activator.CreateInstance(fieldWrapperType.MakeGenericType(returnedType));
                items.Add(fieldInfo.Name, new VisualNodeLinkRegistration(node, fieldInfo.Name, asyncTargetGetter, gotHandler, currentGetter).AddTo(disposables));
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
            items[fieldName].SetTarget(target);
        }
    }
}