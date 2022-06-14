using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.OData.UriParser;
using System.Linq.Expressions;

namespace Aspor.Authorization.Policy
{
    public class DeletedEntitiesFilterSelectExpandBinder : SelectExpandBinder
    {

        public DeletedEntitiesFilterSelectExpandBinder(IFilterBinder filterBinder, IOrderByBinder orderByBinder) 
            : base(filterBinder, orderByBinder) {}

        public override Expression BindSelectExpand(SelectExpandClause selectExpandClause, QueryBinderContext context)
        {
            return QueryVisitor.INSTANCE.Visit(base.BindSelectExpand(selectExpandClause, context));
        }
    }
}
