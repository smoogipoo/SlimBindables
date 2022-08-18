// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using SlimBindables.Components;

namespace SlimBindables
{
    public sealed class Bindable<T> : IBindable
    {
        public event Action<T?>? ValueChanged
        {
            add => valueComponent.ValueChanged += value;
            remove => valueComponent.ValueChanged -= value;
        }

        public T? Value
        {
            get => valueComponent.Value;
            set => valueComponent.Value = value;
        }

        private readonly Dictionary<Type, BindableComponent> components;
        private readonly ValueComponent<T> valueComponent;

        public Bindable(Dictionary<Type, BindableComponent> components, ValueComponent<T> valueComponent)
        {
            this.components = components;
            this.valueComponent = valueComponent;
        }

        public bool TryGetComponent<TComponent>([NotNullWhen(true)] out TComponent? component)
            where TComponent : BindableComponent
        {
            if (components.TryGetValue(typeof(TComponent), out BindableComponent? c))
            {
                component = (TComponent)c;
                return true;
            }

            component = null;
            return false;
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : BindableComponent
        {
            return (TComponent)components[typeof(TComponent)];
        }
    }
}
