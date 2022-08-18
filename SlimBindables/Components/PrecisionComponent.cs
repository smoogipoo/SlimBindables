// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;

namespace SlimBindables.Components
{
    public class PrecisionComponent<T> : BindableComponent, IValueMutatingComponent<T>
        where T : IComparable<T>, IConvertible
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
                    valueComponent.Value = Mutate(valueComponent.Value);
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

            double dValue = value.ToDouble(NumberFormatInfo.InvariantInfo);
            double dPrecision = this.value.ToDouble(NumberFormatInfo.InvariantInfo);

            return (T)Convert.ChangeType(Math.Round(dValue / dPrecision) * dPrecision, typeof(T), CultureInfo.InvariantCulture);
        }
    }

    public static class PrecisionComponentExtensions
    {
        public static BindableBuilder<T> WithPrecision<T>(this BindableBuilder<T> builder)
            where T : IComparable<T>, IConvertible
        {
            if (typeof(T) == typeof(sbyte))
                return builder.WithPrecision((T)(object)(sbyte)1);
            if (typeof(T) == typeof(byte))
                return builder.WithPrecision((T)(object)(byte)1);
            if (typeof(T) == typeof(short))
                return builder.WithPrecision((T)(object)(short)1);
            if (typeof(T) == typeof(ushort))
                return builder.WithPrecision((T)(object)(ushort)1);
            if (typeof(T) == typeof(int))
                return builder.WithPrecision((T)(object)1);
            if (typeof(T) == typeof(uint))
                return builder.WithPrecision((T)(object)1U);
            if (typeof(T) == typeof(long))
                return builder.WithPrecision((T)(object)1L);
            if (typeof(T) == typeof(ulong))
                return builder.WithPrecision((T)(object)1UL);
            if (typeof(T) == typeof(float))
                return builder.WithPrecision((T)(object)float.Epsilon);

            return builder.WithPrecision((T)(object)double.Epsilon);
        }

        public static BindableBuilder<T> WithPrecision<T>(this BindableBuilder<T> builder, T value)
            where T : IComparable<T>, IConvertible
        {
            PrecisionComponent<T> component = builder.Requires<PrecisionComponent<T>>();
            component.Value = value;
            return builder;
        }

        public static PrecisionComponent<T> GetPrecision<T>(this Bindable<T> bindable)
            where T : IComparable<T>, IConvertible
        {
            return bindable.GetComponent<PrecisionComponent<T>>();
        }
    }
}
