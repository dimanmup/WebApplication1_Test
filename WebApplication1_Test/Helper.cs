namespace WebApplication1_Test
{
    public static class Helper
    {
        public static string GetPath(this HttpRequest request)
        {
            return string.Concat(
                request.Scheme,
                "://",
                request.Host,
                request.Path,
                request.QueryString.Value);
        }

        public static Task HttpErrorCatcher(HttpContext context)
        {
            switch (context.Response.StatusCode)
            {
                case 403:
                case 404:
                case 408:
                    context.Response.Redirect($"/Error?status_code={context.Response.StatusCode}&uri={context.Request.GetPath()}");
                    break;

                default:
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
