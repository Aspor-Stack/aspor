

namespace Aspor.Export.Formats
{
    public interface IExportFormatWriter
    {

        public string GetDefaultEnding();

        public int GetFieldPosition();

        public void WriteField(object value);

        public void SkipFields(int count);

        public void NextLine();

        public byte[] ToByteArray();

    }
}
