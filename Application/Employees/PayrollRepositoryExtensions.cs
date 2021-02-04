using FriMav.Domain;
using FriMav.Domain.Entities.Payrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Application.Employees
{
    public static class PayrollRepositoryExtensions
    {
        public static bool IsAlreadyLiquidated<T>(this IRepository<Payroll> payrollRepository, T document)
            where T : LiquidationDocument
        {
            return payrollRepository.Query()
                .Where(x => !x.DeleteDate.HasValue && x.EmployeeId == document.EmployeeId)
                .SelectMany(x => x.Liquidation)
                .OfType<T>()
                .Any(x => x.Id == document.Id);
        }
    }
}
