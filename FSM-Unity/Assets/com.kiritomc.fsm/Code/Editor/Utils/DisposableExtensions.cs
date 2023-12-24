using System;
using System.Reactive.Disposables;

namespace FSM.Editor
{
    public static class DisposableExtensions
    {
        public static void AddTo(this IDisposable disposable, CompositeDisposable target)
        {
            target.Add(disposable);
        }
    }
}