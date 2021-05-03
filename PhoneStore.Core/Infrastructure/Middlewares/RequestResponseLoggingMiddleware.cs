using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PhoneStore.Core.Infrastructure.Logging;
using Microsoft.AspNetCore.Routing;

namespace PhoneStore.Core.Infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(ILogger logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var headers = string.Join(Environment.NewLine, context.Request.Headers.Select(x => $"{x.Key} = {x.Value}"));
            var path = UriHelper.GetDisplayUrl(context.Request);
            
            var stopWatch = Stopwatch.StartNew();
            var builder = new StringBuilder();
            var uniqieId = Guid.NewGuid();

            context.Request.EnableRewind();

            builder.AppendLine("startDate => " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture));
            builder.AppendLine("unique id => " + uniqieId.ToString("N"));
            builder.AppendLine("path => " + path);
            builder.AppendLine("method => " + method);
            builder.AppendLine("headers => " + headers);

            context.Request.Body.Seek(0, SeekOrigin.Begin);

            var requestText = await new StreamReader(context.Request.Body).ReadToEndAsync();
            builder.AppendLine();

            var requestInformation = builder.ToString();

            context.Request.Body.Seek(0, SeekOrigin.Begin);

            var originalBodyStream = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                stopWatch.Start();

                await _next(context);

                stopWatch.Stop();

                var statusCode = (HttpStatusCode)context.Response.StatusCode;
                var elapsed = stopWatch.ElapsedMilliseconds;

                builder = new StringBuilder(Environment.NewLine);
                builder.AppendLine("endDate => " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture));
                builder.AppendLine("unique id => " + uniqieId.ToString("N"));
                builder.AppendLine("response statusCode => " + statusCode.ToString());
                builder.AppendLine("elapsed time milliseconds => " + elapsed.ToString());

                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                builder.Append("response body => " + responseText + Environment.NewLine);

                builder.AppendLine();
                builder.AppendLine();

                var responseInformation = builder.ToString();

                _logger.LogInformation(requestInformation, requestText.ToLower().Contains("password") ? string.Empty : requestText, responseInformation, responseText);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await context.Response.Body.CopyToAsync(originalBodyStream);
            }
        }
    }
}