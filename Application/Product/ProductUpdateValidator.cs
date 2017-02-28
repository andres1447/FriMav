using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdate>
    {
        private IContainerContext _container;

        public ProductUpdateValidator(IContainerContext container)
        {
            _container = container;

            RuleFor(c => c.Name).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(c => c.Code).NotEmpty().WithMessage("El codigo es requerido.");
            
            RuleFor(c => c.Code)
                .Must((c, code) => { return ProductCodeNotExists(c.ProductId, code); }).WithMessage("Ya existe un producto activo con ese codigo.")
                .Unless(c => string.IsNullOrEmpty(c.Code));
        }

        public bool ProductCodeNotExists(int productId, string code)
        {
            var service = _container.Resolve<IProductService>();
            var saved = service.Get(productId);
            return saved.Code == code || !service.ExistsActiveCode(code);
        }
    }
}