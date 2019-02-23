using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GNB.Web.Utilities
{
    public static class HtmlHelper
    {
        public static IHtmlContent NameApplication(this IHtmlHelper htmlHelper)
            => new HtmlString("Gloiath National Bank");
    }
}
