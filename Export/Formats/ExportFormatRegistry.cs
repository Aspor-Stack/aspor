using System;
using System.Collections.Generic;

namespace Aspor.Export.Formats
{
    public class ExportFormatRegistry
    {

        private static readonly Dictionary<string, Type> _formats = new Dictionary<string, Type>();

        static ExportFormatRegistry()
        {
            RegisterFormat("csv", typeof(CSVExportFormatWriter));
            RegisterFormat("excel", typeof(ExcelExportFormatWriter));
        }

        public static void RegisterFormat(string name, Type writer)
        {
            _formats.Add(name, writer);
        }

        public static IExportFormatWriter GetFormatWriter(string name)
        {
            Type type = _formats.GetValueOrDefault(name);
            if (type != null)
            {
                return (IExportFormatWriter)Activator.CreateInstance(type);
            }
            return null;
        }

    }
}
