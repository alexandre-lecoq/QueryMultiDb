namespace QueryMultiDbGui
{
    using System;
    using System.Windows.Forms;

    public static class WindowsFormsExtensions
    {
        private static TResult InvokeEx<TControl, TResult>(this TControl control, Func<TControl, TResult> func)
            where TControl : Control
        {
            return control.InvokeRequired
                ? (TResult) control.Invoke(func, control)
                : func(control);
        }

        private static void InvokeEx<TControl>(this TControl control, Action<TControl> action)
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
