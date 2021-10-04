using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspor.Common.Extensions
{

    public static class HttpContextExtensions
    {

        public static ETag GetETag(this HttpContext context, string headKey)
        {
            StringValues values;
            if (context.Request.Headers.TryGetValue(headKey, out values))
            {
                EntityTagHeaderValue etagValue;
                if (EntityTagHeaderValue.TryParse(values.SingleOrDefault(), out etagValue))
                {
                    try
                    {
                        ETag etag = context.Request.GetETag(etagValue);
                        if (etag != null && etag.IsWellFormed) return etag;
                    }
                    catch (Exception) { }
                }

            }
            return null;
        }

        public static bool IFMatch<TEntity>(this HttpContext context, IQueryable<TEntity> queryable) where TEntity : class
        {
            ETag etag = GetETag(context, "If-Match");
            if (etag != null && !etag.IsAny)
            {
                etag.EntityType = queryable.ElementType;
                etag.ApplyTo(queryable);
                return !queryable.Any();
            }
            return false;
        }

        public static bool IFMatch(this HttpContext context, object entity)
        {
            ETag etag = GetETag(context, "If-Match");
            if (etag != null)
            {
                return etag.IsAny || Matches(context, etag, entity);
            }
            return false;
        }

        public static bool IFNonMatch<TEntity>(this HttpContext context, IQueryable<TEntity> queryable) where TEntity : class
        {
            ETag etag = GetETag(context, "If-None-Match");
            if (etag != null)
            {
                etag.EntityType = queryable.ElementType;
                etag.ApplyTo(queryable);
                return queryable.Any();
            }
            return false;
        }

        public static bool IFNonMatch(this HttpContext context, object entity)
        {
            ETag etag = GetETag(context, "If-None-Match");
            if (etag != null)
            {
                return etag.IsAny || Matches(context, etag, entity);
            }
            return false;
        }

        private static bool Matches(HttpContext context, ETag etag, object entity)
        {
            IEdmModel model = context.Request.GetModel();
            ODataPath path = context.Request.ODataFeature().Path;
            if (model == null || path == null) throw new InvalidOperationException("OData feature not available");

            IEnumerable<IEdmStructuralProperty> properties = model.GetConcurrencyProperties(path.GetNavigationSource()).OrderBy(c => c.Name);

            foreach (IEdmStructuralProperty property in properties)
            {
                string originalName = model.GetClrPropertyName(property);
                object value = entity.GetType().GetProperty(originalName).GetValue(entity);
                object concurrentValue = etag[property.Name];
                if (concurrentValue != null)
                {
                    if (value == null)
                    {
                        return false;
                    }
                    else if (!value.Equals(concurrentValue))
                    {
                        return false;
                    }
                }
                else if (value != null)
                {
                    return false;
                }

            }

            return true;
        }

    }

}
