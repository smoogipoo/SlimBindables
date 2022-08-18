// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;

namespace SlimBindables.Components
{
    public abstract class BindingsComponent : BindableComponent
    {
        public IBindable? Source { get; private set; }

        private BindingsComponent? lastSendTarget;

        public sealed override void SendMessage<TComponent>(TComponent source, SendMessageCallback<TComponent> callback)
        {
            if (lastSendTarget != null)
                return;

            foreach (BindingsComponent binding in GetBindings())
            {
                lastSendTarget = binding;

                Debug.Assert(binding.Source != null);

                if (binding.Source.TryGetComponent(out TComponent? target))
                    callback(source, target);

                lastSendTarget = null;
            }
        }

        protected abstract IEnumerable<BindingsComponent> GetBindings();

        public abstract void BindTo(BindingsComponent other);

        public abstract void AddBinding(BindingsComponent other);

        public override void Resolve(IBindable bindable, IBindableBuilder builder)
        {
            base.Resolve(bindable, builder);
            Source = bindable;
        }
    }

    public static class BindingsComponentExtensions
    {
        public static void BindTo<T>(this Bindable<T> source, Bindable<T> target)
        {
            source.GetComponent<BindingsComponent>().BindTo(target.GetComponent<BindingsComponent>());
        }
    }
}
