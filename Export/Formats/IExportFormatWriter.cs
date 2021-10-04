

namespace Aspor.Export.Formats
{
    public interface IExportFormatWriter
    {

        public string GetDefaultEnding();

        public void WriteField(object value);

        public void NextLine();

        public byte[] ToByteArray();

    }
}
