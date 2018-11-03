namespace QueryMultiDbGui
{
    using System;
    using System.Windows.Forms;

    public static class WindowsFormsExtensions
    {
        public static TResult InvokeEx<TControl, TResult>(this TControl control, Func<TControl, TResult> func)
            where TControl : Control
        {
            return control.InvokeRequired
                ? (TResult) control.Invoke(func, control)
                : func(control);
        }

        public static void InvokeEx<TControl>(this TControl control, Action<TControl> action)
            where TControl : Control
        {
            control.InvokeEx(c =>
            {
                action(c);
                return c;
            });
        }

        public static void InvokeEx<TControl>(this TControl control, Action action)
            where TControl : Control
        {
            control.InvokeEx(c => action());
        }
    }
}
