using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Application;
using Infrastructure.Repositories;
using Domain.Repositories;
using Infrastructure;

namespace CrossCutting
{
    public class DependencyRegistrar
    {
        public static void Register(IWindsorContainer container)
        {
            container.Register(Classes
                                .FromAssemblyContaining<ProductService>()
                                .InSameNamespaceAs<ProductService>()
                                .WithServiceDefaultInterfaces()
                                .LifestyleTransient());

            container.Register(Classes
                                .FromAssemblyContaining(typeof(BaseRepository<>))
                                .InSameNamespaceAs(typeof(BaseRepository<>))
                                .WithServiceDefaultInterfaces()
                                .LifestyleTransient());
            
            container.Register(Component.For<IDatabaseContext>().ImplementedBy<DatabaseContext>().LifestylePerWebRequest());
        }
    }
}
