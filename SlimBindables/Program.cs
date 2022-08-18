using System.Diagnostics;
using SlimBindables;
using SlimBindables.Components;

Bindable<int> b1 = new BindableBuilder<int>()
                   .WithDescription()
                   .WithStrongBindings()
                   .Build();

Bindable<int> b2 = new BindableBuilder<int>()
                   .WithDescription()
                   .WithWeakBindings()
                   .Build();

Bindable<int> b3 = new BindableBuilder<int>()
                   .WithWeakBindings()
                   .WithPrecision()
                   .Build();

b1.BindTo(b2);
b1.BindTo(b3);

b3.Value = 5;
Trace.Assert(b1.Value == 5);
Trace.Assert(b2.Value == 5);
Trace.Assert(b3.Value == 5);

b1.GetDescription().Value = "A";
b2.GetDescription().Value = "B";
Trace.Assert(b1.GetDescription().Value == "A");
Trace.Assert(b2.GetDescription().Value == "B");

b3.GetPrecision().Value = 2;
Trace.Assert(b1.Value == 4);
Trace.Assert(b2.Value == 4);
Trace.Assert(b3.Value == 4);

b3.Value = 9;
Trace.Assert(b1.Value == 8);
Trace.Assert(b2.Value == 8);
Trace.Assert(b3.Value == 8);

Bindable<int> b4 = new BindableBuilder<int>()
                   .WithMinimum(2)
                   .WithMaximum(10)
                   .Build();

Trace.Assert(b4.Value == 2);

b4.GetMinimum().Value = 3;
Trace.Assert(b4.Value == 3);

b4.GetMinimum().Value = 2;
b4.GetMaximum().Value = 10;
Trace.Assert(b4.Value == 3);

b4.Value = 100;
Trace.Assert(b4.Value == 10);

b4.Value = -100;
Trace.Assert(b4.Value == 2);
