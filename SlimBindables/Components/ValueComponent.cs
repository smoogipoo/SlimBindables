// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public class ValueComponent<T> : BindableComponent
    {
        public event Action<T?>? ValueChanged;

        private T? value;

        public T? Value
        {
            get => value;
            set
            {
                this.value = RunMutators(value);

                ValueChanged?.Invoke(value);

                SendMessage(this, (source, target) => target.Value = source.Value);
            }
        }

        private List<IValueMutatingComponent<T>>? mutators;

        public void AddMutator(IValueMutatingComponent<T> mutatingComponent)
        {
            mutators ??= new List<IValueMutatingComponent<T>>();
            mutators.Add(mutatingComponent);
        }

        public override void Resolve(IBindable bindable, IBindableBuilder builder)
        {
            base.Resolve(bindable, builder);
            Value = RunMutators(Value);
        }

        public T? RunMutators(T? value)
        {
            if (mutators == null)
                return value;

            foreach (IValueMutatingComponent<T> m in mutators)
                value = m.Mutate(value);

            return value;
        }
    }
}
