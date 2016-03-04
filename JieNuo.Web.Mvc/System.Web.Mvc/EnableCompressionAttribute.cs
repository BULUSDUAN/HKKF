namespace System.Web.Mvc {
    using System;
    using System.IO.Compression;

    public class EnableCompressionAttribute : ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.ParentActionViewContext != null) return;

            var acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (acceptEncoding.IsNullOrEmpty()) return;

            var response = filterContext.HttpContext.Response;
            if (response.ContentType.ToLower() != "text/html") return;

            if (acceptEncoding.ToLower().Contains("gzip"))
            {
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                response.AppendHeader("Content-Encoding", "gzip");
            }
            else
            {
                if (acceptEncoding.ToLower().Contains("deflate"))
                {
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    response.AppendHeader("Content-Encoding", "deflate");
                }
            }
        }
    }
}
