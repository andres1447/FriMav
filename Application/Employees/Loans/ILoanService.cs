using FriMav.Domain;
using System.Collections.Generic;

namespace FriMav.Application
{
    public interface ILoanService
    {
        [Transactional]
        void Create(LoanCreate request);

        [Transactional]
        void Delete(int id);

        LoanResponse Get(int id);
        List<LoanFeeResponse> GetRemainingFees(int employeeId);
    }
}