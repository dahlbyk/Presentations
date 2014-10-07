using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Owin;
using AppFunc = System.Func<
                    System.Collections.Generic.IDictionary<string, object>,
                    System.Threading.Tasks.Task
                    >;

namespace Demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif

            app.UseWelcomePage("/");

            app.UseFunc(LoggingMiddleware);

            #region /mordor

            app.UseFunc(
                next =>
                async env =>
                {
                    if (!string.Equals("/mordor", (string)env["owin.RequestPath"], StringComparison.Ordinal))
                    {
                        await next(env);
                        return;
                    }

                    using (var sw = new StreamWriter((Stream)env["owin.ResponseBody"]))
                    {
                        var content = string.Format("One does not simply {0} into Mordor.",
                                                    env["owin.RequestMethod"]);

                        var headers = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
                        headers["Content-Length"] = new[] { content.Length.ToString() };
                        headers["Content-Type"] = new[] { "text/plain" };
                        await sw.WriteAsync(content);
                    }
                });

            #endregion

            #region /email

            app.UseFunc(
                next =>
                async env =>
                {
                    if (!string.Equals("/email", (string)env["owin.RequestPath"], StringComparison.Ordinal))
                    {
                        await next(env);
                        return;
                    }

                    using (var sw = new StreamWriter((Stream)env["owin.ResponseBody"]))
                    {
                        var content =
                            "<html><body><form action='http://localhost:23456/api/send' method='POST'><label>Subject: <input name='Subject' /></label><input type='Submit'></form></body></html>";

                        var headers = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
                        headers["Content-Length"] = new[] { content.Length.ToString() };
                        headers["Content-Type"] = new[] { "text/html" };
                        await sw.WriteAsync(content);
                    }
                });

            #endregion

            app.UseFunc(EnvironmentEndpoint);
        }

        static AppFunc LoggingMiddleware(AppFunc next)
        {
            return async env =>
                {
                    Console.WriteLine("Begin " + env["owin.RequestPath"]);
                    await next(env);
                    Console.WriteLine("End   " + env["owin.RequestPath"]);
                };
        }

        static AppFunc EnvironmentEndpoint(AppFunc ignored)
        {
            return async env =>
                {
                    using (var sw = new StreamWriter((Stream)env["owin.ResponseBody"]))
                    {
                        var sb = new StringBuilder("Environment:\n");

                        foreach (var kvp in env.OrderBy(x => x.Key))
                            sb.AppendFormat("  {0}: {1}\n", kvp.Key, kvp.Value);

                        var content = sb.ToString();

                        var headers = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
                        headers["Content-Length"] = new[] { content.Length.ToString() };
                        headers["Content-Type"] = new[] { "text/plain" };
                        await sw.WriteAsync(content);
                    }
                };
        }
    }
}