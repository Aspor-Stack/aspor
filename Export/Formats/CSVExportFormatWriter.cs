using System.Text;

namespace Aspor.Export.Formats
{
    public class CSVExportFormatWriter : IExportFormatWriter
    {

        private readonly StringBuilder _builder;
        private bool _first;

        public CSVExportFormatWriter()
        {
            _builder = new StringBuilder();
            _first = true;
        }

        public string GetDefaultEnding()
        {
            return "csv";
        }

        public void WriteField(object? value)
        {
            if (_first) _first = false;
            else _builder.Append(';');
            if (value != null) _builder.Append(value);
        }

        public void NextLine()
        {
            _builder.Append('\n');
            _first = true;
        }

        public byte[] ToByteArray()
        {
            return Encoding.Default.GetBytes(_builder.ToString());
        }

    }
}
