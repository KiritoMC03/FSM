using System;
using System.Reactive.Disposables;

namespace FSM.Editor
{
    public static class DisposableExtensions
    {
        public static T AddTo<T>(this T disposable, CompositeDisposable target) where T: IDisposable
        {
            target.Add(disposable);
            return disposable;
        }
    }
}