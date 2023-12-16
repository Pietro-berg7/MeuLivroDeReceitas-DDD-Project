using System.Globalization;

namespace MeuLivroDeReceitas.Api.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IList<string> _idiomasSuportados = new List<string>
        {
            "pt",
            "en"
        };

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultura = new CultureInfo("pt");

            if (context.Request.Headers.ContainsKey("Accept-Language"))
            {
                var idioma = context.Request.Headers["Accept-Language"].ToString();

                if (_idiomasSuportados.Any(c => c.Equals(idioma)))
                {
                    cultura = new CultureInfo(idioma);
                }
            }

            CultureInfo.CurrentCulture = cultura;
            CultureInfo.CurrentUICulture = cultura;

            await _next(context);
        }
    }
}
