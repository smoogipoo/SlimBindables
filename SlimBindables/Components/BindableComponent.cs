// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public class BindableComponent
    {
        private BindingsComponent? bindings;

        public virtual void Resolve(IBindable bindable, IBindableBuilder builder)
        {
            bindings = builder.MayHave<BindingsComponent>();
        }

        public virtual void SendMessage<TComponent>(TComponent source, SendMessageCallback<TComponent> callback)
            where TComponent : BindableComponent
        {
            bindings?.SendMessage(source, callback);
        }
    }

    public delegate void SendMessageCallback<in TComponent>(TComponent source, TComponent target);
}
