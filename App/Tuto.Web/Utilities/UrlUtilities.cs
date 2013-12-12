using System;
using System.Web;

namespace Tuto.Web.Utilities
{
    public class UrlUtilities
    {
        public static string getRootUrl()
        {
            HttpRequest request = HttpContext.Current.Request;
            return request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, String.Empty);
        }
    }
}