namespace QueryMultiDbGui
{
    using System;
    using System.IO;
    
    public class TargetFileEntry
    {
        public string Filepath { get; }
        public string Filename { get; }
        public long Length { get; }
        public DateTime LastWriteTime { get; }

        public TargetFileEntry(string filepath)
        {
            Filepath = filepath;
            Filename = Path.GetFileName(filepath);
            var fileInfo = new FileInfo(filepath);
            Length = fileInfo.Length;
            LastWriteTime = fileInfo.LastWriteTime;
        }

        public override string ToString()
        {
            return Filename;
        }
    }
}
