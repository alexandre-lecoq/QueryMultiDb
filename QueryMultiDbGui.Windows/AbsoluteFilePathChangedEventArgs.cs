using System;

namespace QueryMultiDbGui
{
    public class AbsoluteFilePathChangedEventArgs : EventArgs
    {
        public string AbsoluteFilePath { get; }

        public AbsoluteFilePathChangedEventArgs(string absoluteFilePath)
        {
            AbsoluteFilePath = absoluteFilePath;
        }

        public override string ToString()
        {
            return AbsoluteFilePath;
        }
    }
}
