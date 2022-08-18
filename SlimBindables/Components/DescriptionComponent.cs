// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace SlimBindables.Components
{
    public sealed class DescriptionComponent : BindableComponent
    {
        public string? Value { get; set; }
    }

    public static class DescriptionComponentExtensions
    {
        public static BindableBuilder<T> WithDescription<T>(this BindableBuilder<T> builder)
        {
            builder.Requires<DescriptionComponent>();
            return builder;
        }

        public static DescriptionComponent GetDescription(this IBindable bindable)
        {
            return bindable.GetComponent<DescriptionComponent>();
        }
    }
}
