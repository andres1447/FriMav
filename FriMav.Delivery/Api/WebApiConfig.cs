using Castle.Facilities.AspNet.WebApi;
using Castle.Windsor;
using FriMav.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Configuration;
using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FriMav.Delivery.Api
{
    public class OwinConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(Register());

            var staticFilesPath = ConfigurationManager.AppSettings["StaticFilesPath"];
            if (!string.IsNullOrEmpty(staticFilesPath))
                HostClientWebpage(app, staticFilesPath);
        }

        private HttpConfiguration Register()
        {
            var container = new WindsorContainer();
            container.Install(
                new DatabaseInstaller(),
                new ApplicationInstaller());

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods: "*"));
            config.Filters.Add(new ModelStateValidationFilterAttribute());
            config.Filters.Add(new NotFoundExceptionFilterAttribute());
            config.Filters.Add(new ValidationExceptionFilterAttribute());

            container.AddFacility<AspNetWebApiFacility>(x => x.UsingConfiguration(config).UsingSelfHosting());

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            return config;
        }

        private void HostClientWebpage(IAppBuilder app, string staticFilesPath)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString(""),
                FileSystem = new PhysicalFileSystem(staticFilesPath)
            });
            app.UseFileServer(new FileServerOptions
            {
                RequestPath = new PathString(""),
                FileSystem = new PhysicalFileSystem(Path.Combine(staticFilesPath, "app"))
            });
        }
    }
}
