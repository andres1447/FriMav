using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentValidation;
using FriMav.Application;
using FriMav.Infrastructure;
using System.Web.Http;

namespace FriMav.Delivery.Api
{
    public class ApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                .BasedOn<ApiController>()
                .LifestyleScoped());

            container.Register(
                Component.For<IWindsorContainer>().Instance(container));

            /*container.Register(
                Classes.FromAssemblyContaining<CustomerCreateValidator>()
                .BasedOn(typeof(IValidator<>))
                .WithServiceDefaultInterfaces()
                .LifestylePerWebRequest());*/

            container.Register(
                Classes.FromAssemblyContaining<ProductService>()
                .Pick().If(x => x.Name.Contains("Service"))
                .WithServiceAllInterfaces()
                .Configure(x => x.Interceptors<TransactionalInterceptor>())
                .LifestyleScoped());

            container.Register(Component.For<ITime, Time>());
        }
    }
}
