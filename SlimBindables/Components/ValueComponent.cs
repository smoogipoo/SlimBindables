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
                if (disabledComponent?.Value == true)
                    throw new InvalidOperationException($"Can not set value to \"{value?.ToString()}\" as bindable is disabled.");

                this.value = RunMutators(value);

                SendMessage(this, (source, target) => target.Value = source.Value);
                ValueChanged?.Invoke(value);
            }
        }

        private DisabledComponent? disabledComponent;
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

            disabledComponent = builder.MayHave<DisabledComponent>();
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
