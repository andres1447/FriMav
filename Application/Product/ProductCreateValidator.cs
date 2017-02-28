using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class ProductCreateValidator : AbstractValidator<ProductCreate>
    {
        private IContainerContext _container;

        public ProductCreateValidator(IContainerContext container)
        {
            _container = container;

            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(c => c.Code).NotEmpty().WithMessage("El codigo es requerido.");
            RuleFor(c => c.Code)
                .Must(ProductCodeNotExists).WithMessage("Ya existe un producto activo con ese codigo.")
                .Unless(c => string.IsNullOrEmpty(c.Code));
        }

        public bool ProductCodeNotExists(string code)
        {
            return !_container.Resolve<IProductService>().ExistsActiveCode(code);
        }
    }
}