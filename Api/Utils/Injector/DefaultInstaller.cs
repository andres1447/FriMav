using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentValidation;
using FluentValidation.WebApi;
using FriMav.Application;
using System.Web.Http.Validation;

namespace FriMav.Api.Utils.Injector
{
    public class DefaultInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<IContainerContext>().ImplementedBy<ContainerContext>());

            container.Register(
                Classes.FromAssemblyContaining<CustomerCreateValidator>()
                .BasedOn(typeof(IValidator<>))
                .WithServiceFirstInterface()
                .LifestylePerWebRequest());

            container.Register(
                Classes.FromAssemblyContaining<ProductService>()
                .Pick().If(x => x.Name.Contains("Service"))
                .WithServiceDefaultInterfaces()
                .LifestylePerWebRequest());
        }
    }
}