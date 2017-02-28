using Castle.Windsor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Api.Utils.Injector
{
    public class WindsorFluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IWindsorContainer _container;

        public WindsorFluentValidatorFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (_container.Kernel.HasComponent(validatorType))
                return _container.Resolve(validatorType) as IValidator;
            return null;
        }
    }
}