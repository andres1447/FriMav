using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    /*public class EmployeeCreateValidator : AbstractValidator<EmployeeCreate>
    {
        private IContainerContext _container;

        public EmployeeCreateValidator(IContainerContext container)
        {
            _container = container;

            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(c => c.Code).NotEmpty().WithMessage("El codigo es requerido.");
            
            RuleFor(c => c.Code)
                .Must(EmployeeCodeNotExists).WithMessage("El codigo de usuario ya está registrado.")
                .Unless(c => string.IsNullOrEmpty(c.Code));
        }

        public bool EmployeeCodeNotExists(string code)
        {
            return !_container.Resolve<IEmployeeService>().Exists(code);
        }
    }*/
}