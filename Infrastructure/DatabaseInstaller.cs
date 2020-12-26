using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FriMav.Domain;
using FriMav.Infrastructure.Migrations;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FriMav.Infrastructure
{
    public class DatabaseInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<DbContext>()
                .ImplementedBy<FriMavDbContext>()
                .LifestyleScoped());

            container.Register(
                Component.For<IDocumentNumberGenerator>()
                .ImplementedBy<DocumentNumberGenerator>()
                .LifestyleScoped());

            container.Register(
                Component.For(typeof(IRepository<>))
                .ImplementedBy(typeof(EntityRepository<>))
                .LifestyleScoped());

            container.Register(Component.For<TransactionalInterceptor>().LifestyleScoped());
            
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Configuration.AutomaticMigrationDataLossAllowed = true;
            migrator.Update();
        }
    }
}
