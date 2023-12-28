using System;

namespace FSM.Editor
{
    public class FieldWrapper<T>
    {
        private T value;
        private event Action<T> OnChanged;

        public T Value
        {
            get => value;
            set
            {
                if (object.Equals(this.value, value)) return;
                this.value = value;
                OnChanged?.Invoke(value);
            }
        }

        public Subscription Subscribe(Action<T> action)
        {
            OnChanged += action;
            return new Subscription(() => OnChanged -= action);
        }
    }
}