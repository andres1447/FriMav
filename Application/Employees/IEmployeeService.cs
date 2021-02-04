using FriMav.Domain;
using FriMav.Domain.Entities;
using FriMav.Domain.Entities.Payrolls;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface IEmployeeService
    {
        [Transactional]
        void Create(EmployeeCreate request);

        [Transactional]
        void Update(EmployeeUpdate request);

        [Transactional]
        void Delete(int id);

        [Transactional]
        Payroll ClosePayroll(int id);

        [Transactional]
        List<Payroll> ClosePayroll();

        List<EmployeeResponse> GetAll();
        Employee Get(int id);
        List<string> UsedCodes();
        List<UnliquidatedDocument> GetUnliquidatedDocuments(int id);
        List<PayrollResponse> GetPayrolls();
    }
}
