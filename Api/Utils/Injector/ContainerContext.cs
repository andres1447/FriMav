using Castle.Windsor;
using FriMav.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Api.Utils.Injector
{
    public class ContainerContext : IContainerContext
    {
        private IWindsorContainer _container;

        public ContainerContext(IWindsorContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}