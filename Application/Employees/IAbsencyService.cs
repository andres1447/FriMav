using FriMav.Domain;

namespace FriMav.Application.Employees
{
    public interface IAbsencyService
    {
        [Transactional]
        void Create(AbsencyCreate request);

        [Transactional]
        void Delete(int id);
    }
}