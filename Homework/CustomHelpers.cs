using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Blackboard
{
    public static class CustomHelpers
    {
        private const string urlRegEx = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";

        public static HtmlString DisplayWithLinksFor(string content)
        {
            string result = ReplaceUrlsWithLinks(content);
            TagBuilder tb = new TagBuilder("p");
            tb.Attributes.Add("style", "white-space: pre-wrap;");
            return new HtmlString(tb.ToString(TagRenderMode.StartTag)+result+ tb.ToString(TagRenderMode.EndTag));
        }

        private static string ReplaceUrlsWithLinks(string input)
        {
            Regex rx = new Regex(urlRegEx);
            string result = rx.Replace(input, delegate (Match match)
            {
                string url = match.ToString();
                return String.Format("<a target=\"_blank\" href=\"{0}\">{0}</a>", url);
            });
            return result;
        }

        private static string GetContent<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            Func<TModel, TProperty> func = expression.Compile();
            return func(htmlHelper.ViewData.Model).ToString();
        }
        
    }
}
