using FriMav.Client.Models;
using FriMav.Domain;

namespace FriMav.Client.Printer
{
    public interface IPrintMode
    {
        bool Applies(string mode);
        void Print(string printer, dynamic model, string modelType);
        string Name { get; }
    }
}
