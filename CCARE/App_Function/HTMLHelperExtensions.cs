using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class HTMLHelperExtensions
    {
        public static MvcHtmlString DisplayWithBreaksFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var model = html.Encode(metadata.Model).Replace("\r\n", "<br />").Replace("\n", "<br />");

            if (String.IsNullOrEmpty(model)) return MvcHtmlString.Empty;

            return MvcHtmlString.Create(model);
        }
    }
}