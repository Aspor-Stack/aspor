
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;

namespace Aspor.Common
{
    public class VirtualSelectExpandQueryValidator : SelectExpandQueryValidator
    {

        private readonly IDictionary<IEdmStructuredType, string[]> _virtual;

        public VirtualSelectExpandQueryValidator(IDictionary<IEdmStructuredType, string[]> virtual0)
        {
            _virtual = virtual0;
        }

        public override void Validate(SelectExpandQueryOption options, ODataValidationSettings settings)
        {
            base.Validate(options, settings);
            ValidateClause(options.SelectExpandClause);
        }

        private void ValidateClause(SelectExpandClause clause)
        {
            if(clause != null && clause.SelectedItems != null)
            {
                foreach(var item in clause.SelectedItems)
                {
                    if(item is ExpandedNavigationSelectItem expand)
                    {
                        var property = expand.PathToNavigationProperty.LastSegment as NavigationPropertySegment;
                        var declaringType = property.NavigationProperty.DeclaringType;
                        var name = property.Identifier;

                        if(_virtual.TryGetValue(declaringType,out var result))
                        {
                            foreach(string virtualItem in result)
                            {
                                if (virtualItem.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    throw new ODataException("Virtual navigation property "+name+" can not be expanded on "+declaringType.FullTypeName());
                                }
                            }
                        }
                        ValidateClause(expand.SelectAndExpand);
                    }
                }
            }
        }

    }
}
