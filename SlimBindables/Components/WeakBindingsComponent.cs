// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public sealed class WeakBindingsComponent : BindingsComponent
    {
        private List<WeakReference<BindingsComponent>>? bindings;

        protected override IEnumerable<BindingsComponent> GetBindings()
        {
            if (bindings == null)
                yield break;

            foreach (WeakReference<BindingsComponent> weakRef in bindings)
            {
                if (weakRef.TryGetTarget(out BindingsComponent? other))
                    yield return other;
            }
        }

        public override void BindTo(BindingsComponent other)
        {
            AddBinding(other);
            other.AddBinding(this);
        }

        public override void AddBinding(BindingsComponent other)
        {
            bindings ??= new List<WeakReference<BindingsComponent>>();
            bindings.Add(new WeakReference<BindingsComponent>(other));
        }
    }

    public static class WeakBindingsComponentExtensions
    {
        public static BindableBuilder<T> WithWeakBindings<T>(this BindableBuilder<T> builder)
        {
            if (builder.Has<BindingsComponent>())
                throw new InvalidOperationException("Bindable can only have one type of bindings.");

            builder.Requires<BindingsComponent>(new WeakBindingsComponent());
            return builder;
        }
    }
}
