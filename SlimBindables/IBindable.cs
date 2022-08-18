// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using SlimBindables.Components;

namespace SlimBindables
{
    public interface IBindable
    {
        public bool TryGetComponent<TComponent>([NotNullWhen(true)] out TComponent? component) where TComponent : BindableComponent;

        TComponent GetComponent<TComponent>() where TComponent : BindableComponent;
    }
}
