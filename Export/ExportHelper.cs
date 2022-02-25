using Aspor.Export.Formats;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Collections;
using System.Linq;

namespace Aspor.Export
{
    public class ExportHelper
    {
        public static byte[] WriteExport(IExportFormatWriter writer, IEdmModel model, IEdmType type, SelectExpandClause clause, IQueryable queryable)
        {
            WriteHead(writer, model, type, clause);
            foreach (object row in queryable)
            {
                writer.NextLine();
                WriteRow(writer, model, type, clause, row);
            }
            return writer.ToByteArray();
        }

        private static void WriteHead(IExportFormatWriter writer, IEdmModel model, IEdmType type, SelectExpandClause clause, string prefix = "")
        {
            if (type is IEdmCollectionType collectionType) type = collectionType.ElementType.Definition;
            if (clause == null || clause.AllSelected)
            {
                if (type is IEdmStructuredType)
                {
                    WriteEdmStructuredTypeHead(writer, model, (IEdmStructuredType)type, prefix);
                }
            }

            if (clause != null && clause.SelectedItems != null)
            {
                foreach (SelectItem item in clause.SelectedItems)
                {
                    if (item is PathSelectItem)
                    {
                        foreach (PropertySegment segment in ((PathSelectItem)item).SelectedPath)
                        {
                            writer.WriteField(prefix + segment.Property.Name);
                        }
                    }
                }

                foreach (SelectItem item in clause.SelectedItems)
                {
                    ExpandedNavigationSelectItem navigationItem = item as ExpandedNavigationSelectItem;
                    if (navigationItem != null)
                    {
                        SelectExpandClause subClause = navigationItem.SelectAndExpand;
                        string newPrefix = prefix + navigationItem.PathToNavigationProperty.FirstSegment.Identifier + ".";
                        WriteHead(writer, model, navigationItem.PathToNavigationProperty.FirstSegment.EdmType, subClause, newPrefix);
                    }
                }
            }
        }

        private static void WriteEdmStructuredTypeHead(IExportFormatWriter writer, IEdmModel model, IEdmStructuredType type, string prefix)
        {
            foreach (IEdmProperty property in type.DeclaredProperties)
            {
                if (property.PropertyKind == EdmPropertyKind.Structural)
                {
                    writer.WriteField(prefix + property.Name);
                }
            }
            if (type.BaseType != null)
            {
                WriteEdmStructuredTypeHead(writer, model, type, prefix);
            }
        }

        private static void WriteRow(IExportFormatWriter writer, IEdmModel model, IEdmType type, SelectExpandClause clause, object row)
        {
            if (row is ISelectExpandWrapper) row = ((ISelectExpandWrapper)row).ToDictionary();

            if (type is IEdmCollectionType collectionType)
            {
                bool first = true;
                int position = writer.GetFieldPosition();
                foreach(var item in (IEnumerable)row)
                {
                    if (first) first = false;
                    else
                    {
                        writer.NextLine();
                        writer.SkipFields(position);
                    }
                    WriteRow(writer,model,collectionType.ElementType.Definition, clause, item);
                }
            }
            else
            {
                if (clause == null || clause.AllSelected)
                {
                    if (type is IEdmStructuredType)
                    {
                        WriteEdmStructuredTypeRow(writer, model, (IEdmStructuredType)type, row);
                    }
                }

                if (clause != null && clause.SelectedItems != null)
                {
                    IDictionary dictionary = row as IDictionary;
                    foreach (SelectItem item in clause.SelectedItems)
                    {
                        if (item is PathSelectItem && row is IDictionary)
                        {
                            foreach (PropertySegment segment in ((PathSelectItem)item).SelectedPath)
                            {
                                writer.WriteField(dictionary[segment.Property.Name]);
                            }
                        }
                    }

                    foreach (SelectItem item in clause.SelectedItems)
                    {
                        ExpandedNavigationSelectItem navigationItem = item as ExpandedNavigationSelectItem;
                        if (navigationItem != null)
                        {
                            SelectExpandClause subClause = navigationItem.SelectAndExpand;
                            string name = navigationItem.PathToNavigationProperty.FirstSegment.Identifier;
                            var newRow = dictionary[name];
                            WriteRow(writer, model, navigationItem.PathToNavigationProperty.FirstSegment.EdmType, subClause, newRow);
                        }
                    }
                }
            }
        }

        private static void WriteEdmStructuredTypeRow(IExportFormatWriter writer, IEdmModel model, IEdmStructuredType type, object row)
        {
            foreach (IEdmProperty property in type.DeclaredProperties)
            {
                if (property.PropertyKind == EdmPropertyKind.Structural)
                {
                    if (row is IDictionary)
                    {
                        writer.WriteField(((IDictionary)row)[property.Name]);
                    }
                    else
                    {
                        string originalName = model.GetClrPropertyName(property);
                        writer.WriteField(row.GetType().GetProperty(originalName).GetValue(row));
                    }
                }
            }
            if (type.BaseType != null)
            {
                WriteEdmStructuredTypeRow(writer, model, type, row);
            }
        }
    }
}
