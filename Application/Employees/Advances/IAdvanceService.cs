using FriMav.Domain;

namespace FriMav.Application.Employees
{
    public interface IAdvanceService
    {
        [Transactional]
        void Create(AdvanceCreate request);

        [Transactional]
        void Delete(int id);
    }
}