using FriMav.Domain;

namespace FriMav.Application.Employees
{
    public interface ITransferenceService
    {
        [Transactional]
        void Create(TransferenceCreate request);

        [Transactional]
        void Delete(int id);
    }
}