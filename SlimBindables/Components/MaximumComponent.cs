// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public class MaximumComponent<T> : BindableComponent, IValueMutatingComponent<T>
        where T : IComparable<T>
    {
        private T value = default!;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                SendMessage(this, (source, target) => target.Value = source.Value);

                if (valueComponent != null)
                    valueComponent.Value = valueComponent.RunMutators(valueComponent.Value);
            }
        }

        private ValueComponent<T>? valueComponent;

        public override void Resolve(IBindable bindable, IBindableBuilder builder)
        {
            base.Resolve(bindable, builder);

            valueComponent = builder.Requires<ValueComponent<T>>();
            valueComponent.AddMutator(this);
        }

        public T? Mutate(T? value)
        {
            if (value == null)
                return value;

            if (value.CompareTo(this.value) == 1)
                value = this.value;

            return value;
        }
    }

    public static class MaximumComponentExtensions
    {
        public static BindableBuilder<T> WithMaximum<T>(this BindableBuilder<T> builder)
            where T : IComparable<T>
        {
            if (typeof(T) == typeof(sbyte))
                return builder.WithMaximum((T)(object)(sbyte)1);
            if (typeof(T) == typeof(byte))
                return builder.WithMaximum((T)(object)(byte)1);
            if (typeof(T) == typeof(short))
                return builder.WithMaximum((T)(object)(short)1);
            if (typeof(T) == typeof(ushort))
                return builder.WithMaximum((T)(object)(ushort)1);
            if (typeof(T) == typeof(int))
                return builder.WithMaximum((T)(object)1);
            if (typeof(T) == typeof(uint))
                return builder.WithMaximum((T)(object)1U);
            if (typeof(T) == typeof(long))
                return builder.WithMaximum((T)(object)1L);
            if (typeof(T) == typeof(ulong))
                return builder.WithMaximum((T)(object)1UL);
            if (typeof(T) == typeof(float))
                return builder.WithMaximum((T)(object)float.Epsilon);

            return builder.WithMaximum((T)(object)double.Epsilon);
        }

        public static BindableBuilder<T> WithMaximum<T>(this BindableBuilder<T> builder, T value)
            where T : IComparable<T>
        {
            MaximumComponent<T> component = builder.Requires<MaximumComponent<T>>();
            component.Value = value;
            return builder;
        }

        public static MaximumComponent<T> GetMaximum<T>(this Bindable<T> bindable)
            where T : IComparable<T>
        {
            return bindable.GetComponent<MaximumComponent<T>>();
        }
    }
}
