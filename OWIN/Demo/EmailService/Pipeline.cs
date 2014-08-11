using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Owin;

namespace EmailService
{
    class Pipeline
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi",
                                       "api/{controller}/{id}",
                                       new { id = RouteParameter.Optional });

            app.UseWebApi(config);
            app.MapSignalR();
            app.UseNancy();
        }
    }

    public class EmailHub : Hub
    {
        public void EmailSent(Email message)
        {
            Clients.All.emailSent(message);
        }
    }

    public class EmailModule : NancyModule
    {
        public EmailModule()
        {
            Get["/"] = _ => View["Index.html"];
        }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            ResourceViewLocationProvider
                .RootNamespaces
                .Add(GetType().Assembly, "EmailService.Views");
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get { return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder); }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }
    }
}