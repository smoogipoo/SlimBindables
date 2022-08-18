// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public sealed class StrongBindingsComponent : BindingsComponent
    {
        private List<BindingsComponent>? bindings;

        protected override IEnumerable<BindingsComponent> GetBindings()
        {
            return bindings ?? Enumerable.Empty<BindingsComponent>();
        }

        public override void BindTo(BindingsComponent other)
        {
            AddBinding(other);
            other.AddBinding(this);
        }

        public override void AddBinding(BindingsComponent other)
        {
            bindings ??= new List<BindingsComponent>();
            bindings.Add(other);
        }
    }

    public static class StrongBindingsComponentExtensions
    {
        public static BindableBuilder<T> WithStrongBindings<T>(this BindableBuilder<T> builder)
        {
            if (builder.Has<BindingsComponent>())
                throw new InvalidOperationException("Bindable can only have one type of bindings.");

            builder.Requires<BindingsComponent>(new StrongBindingsComponent());
            return builder;
        }
    }
}
