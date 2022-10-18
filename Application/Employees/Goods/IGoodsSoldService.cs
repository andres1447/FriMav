using FriMav.Domain;

namespace FriMav.Application
{
    public interface IGoodsSoldService
    {
        [Transactional]
        void Create(GoodsSoldCreate request);

        [Transactional]
        void Delete(int id);

        GoodsSoldResponse Get(int id);
    }
}