using ClosedXML.Excel;
using System.IO;

namespace Aspor.Export.Formats
{
    public class ExcelExportFormatWriter : IExportFormatWriter
    {

        private readonly IXLWorkbook _workbook;
        private readonly IXLWorksheet _worksheet;
        private int _row;
        private int _column;

        public ExcelExportFormatWriter()
        {
            _workbook = new XLWorkbook();
            _worksheet = _workbook.Worksheets.Add("Export");
            _row = 1;
            _column = 1;
        }

        public string GetDefaultEnding()
        {
            return "xlsx";
        }

        public int GetFieldPosition()
        {
            return _column- 1;
        }

        public void SkipFields(int count)
        {
            for (int i = 0; i < count; i++) WriteField("");
        }

        public void WriteField(object value)
        {
            _worksheet.Cell(_row, _column).Value = value != null ? value.ToString() : "";
            _column++;
        }

        public void NextLine()
        {
            _row++;
            _column = 1;
        }

        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            {
                _workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}
