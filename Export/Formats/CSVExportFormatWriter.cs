using System.Text;

namespace Aspor.Export.Formats
{
    public class CSVExportFormatWriter : IExportFormatWriter
    {

        private readonly StringBuilder _builder;
        private int _position;

        public CSVExportFormatWriter()
        {
            _builder = new StringBuilder();
            _position = 0;
        }

        public string GetDefaultEnding()
        {
            return "csv";
        }

        public int GetFieldPosition()
        {
            return _position;
        }

        public void SkipFields(int count)
        {
            for (int i = 0; i < count; i++) WriteField("");
        }

        public void WriteField(object? value)
        {
            if(_position > 0) _builder.Append(';');
            if (value != null) _builder.Append(value);
            _position++;
        }

        public void NextLine()
        {
            _builder.Append('\n');
            _position = 0;
        }

        public byte[] ToByteArray()
        {
            return Encoding.Default.GetBytes(_builder.ToString());
        }

    }
}
