using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdate>
    {
        private IContainerContext _container;

        public CustomerUpdateValidator(IContainerContext container)
        {
            _container = container;

            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(c => c.Code).NotEmpty().WithMessage("El codigo es requerido.");
            
            RuleFor(c => c.Code)
                .Must((c, code) => { return CustomerCodeNotExists(c.PersonId, code); }).WithMessage("El codigo de usuario ya está registrado.")
                .Unless(c => string.IsNullOrEmpty(c.Code));
        }

        public bool CustomerCodeNotExists(int personId, string code)
        {
            var service = _container.Resolve<ICustomerService>();
            var saved = service.Get(personId);
            return saved.Code == code || !service.Exists(code);
        }
    }
}