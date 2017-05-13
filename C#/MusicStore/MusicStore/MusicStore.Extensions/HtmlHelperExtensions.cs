using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MusicStore.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString SubTitle(this HtmlHelper helper, string text)   //=> Extension method
        {
            var html = $"<h2>{text}</h2>";
            return new MvcHtmlString(html);
        }
    }
}
