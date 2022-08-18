// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using SlimBindables.Components;

namespace SlimBindables
{
    public interface IBindableBuilder
    {
        bool Has<TComponent>() where TComponent : BindableComponent;

        TComponent? MayHave<TComponent>() where TComponent : BindableComponent;

        TComponent Requires<TComponent>() where TComponent : BindableComponent, new();

        TComponent Requires<TComponent>(TComponent component) where TComponent : BindableComponent;
    }
}
