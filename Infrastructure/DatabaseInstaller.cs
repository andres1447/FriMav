using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FriMav.Domain.Repositories;
using FriMav.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace FriMav.Infrastructure
{
    public class DatabaseInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDatabaseContext>()
                                .ImplementedBy<DatabaseContext>()
                                .LifestylePerWebRequest());
            
            container.Register(Classes.FromThisAssembly()
                                .InSameNamespaceAs<ProductRepository>()
                                .WithServiceDefaultInterfaces()
                                .LifestylePerWebRequest());
        }
    }
}
