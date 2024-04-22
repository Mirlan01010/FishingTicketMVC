namespace Ticket.MiddleWares
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["jwtToken"]; // Получение токена из cookies

            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }
            //else
            //{
            //    if (context.Request.Path != "/Login")
            //    {
            //        // Установка схемы запроса (может быть необязательно)
            //        context.Request.Scheme = "https";

            //        // Перенаправление на страницу входа
            //        context.Response.Redirect("/Login");
            //        return;
            //    }
            //}

            // Вызов следующего middleware в конвейере
            await _next(context);
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                if (context.Request.Path != "/Swagger/index.html")
                {
                    // Установка схемы запроса (может быть необязательно)
                    context.Request.Scheme = "https";

                    // Перенаправление на страницу входа
                    context.Response.Redirect("/Swagger/index.html");
                    return;
                }

            }
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                if (context.Request.Path != "/Login")
                {
                    // Установка схемы запроса (может быть необязательно)
                    context.Request.Scheme = "https";

                    // Перенаправление на страницу входа
                    context.Response.Redirect("/Login");
                    return;
                }

            }

        }

    }
}
