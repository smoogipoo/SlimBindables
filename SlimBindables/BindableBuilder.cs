// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using SlimBindables.Components;

namespace SlimBindables
{
    public class BindableBuilder<T> : IBindableBuilder
    {
        private readonly Dictionary<Type, BindableComponent> components = new Dictionary<Type, BindableComponent>();

        public Bindable<T> Build()
        {
            var valueComponent = Requires<ValueComponent<T>>();

            Bindable<T> bindable = new Bindable<T>(components, valueComponent);

            foreach ((_, BindableComponent component) in components)
                component.Resolve(bindable, this);

            return bindable;
        }

        public bool Has<TComponent>() where TComponent : BindableComponent
        {
            return components.ContainsKey(typeof(TComponent));
        }

        public TComponent? MayHave<TComponent>() where TComponent : BindableComponent
        {
            if (components.TryGetValue(typeof(TComponent), out BindableComponent? existingComponent))
                return (TComponent)existingComponent;

            return null;
        }

        public TComponent Requires<TComponent>() where TComponent : BindableComponent, new()
        {
            return MayHave<TComponent>() ?? Requires(new TComponent());
        }

        public TComponent Requires<TComponent>(TComponent component) where TComponent : BindableComponent
        {
            components[typeof(TComponent)] = component;
            return component;
        }
    }
}
