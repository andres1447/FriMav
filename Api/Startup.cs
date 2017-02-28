using FriMav.Api.Utils.Injector;
using Castle.Windsor;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using FriMav.Infrastructure;
using Microsoft.Owin;
using FluentValidation.WebApi;
using System.Web.Http.Validation;
using FriMav.Api.Utils.Filters;
using FriMav.Domain.Repositories;

[assembly: OwinStartup(typeof(FriMav.Api.Startup))]

namespace FriMav.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var container = new WindsorContainer().Install(
                new DatabaseInstaller(),
                new DefaultInstaller(),
                new ControllerInstaller());

            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods: "*"));
            config.Filters.Add(new ModelStateValidationFilterAttribute());
            config.DependencyResolver = new WindsorHttpDependencyResolver(container);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            FluentValidationModelValidatorProvider
                .Configure(config, provider => provider.ValidatorFactory = new WindsorFluentValidatorFactory(container));

            appBuilder.UseWebApi(config);
        }
    }
}