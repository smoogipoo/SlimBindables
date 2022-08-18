// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public class DisabledComponent : BindableComponent
    {
        public event Action<bool>? ValueChanged;

        private bool value;

        public bool Value
        {
            get => value;
            set
            {
                this.value = value;

                SendMessage(this, (source, target) => target.Value = source.Value);
                ValueChanged?.Invoke(value);
            }
        }
    }

    public static class DisabledComponentExtensions
    {
        public static BindableBuilder<T> WithDisable<T>(this BindableBuilder<T> builder)
            where T : IComparable<T>
        {
            return builder.WithDisable(false);
        }

        public static BindableBuilder<T> WithDisable<T>(this BindableBuilder<T> builder, bool value)
            where T : IComparable<T>
        {
            DisabledComponent component = builder.Requires<DisabledComponent>();
            component.Value = value;
            return builder;
        }

        public static DisabledComponent GetDisabled<T>(this Bindable<T> bindable)
            where T : IComparable<T>
        {
            return bindable.GetComponent<DisabledComponent>();
        }
    }
}
