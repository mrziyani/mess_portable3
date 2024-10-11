using Messenger.Shared.Models;

namespace Messenger.Client.Services
{
    public interface IMainService<T> where T : class
    {
        Task<List<T>> GetAll(string apiname);
        Task<T> GetRow(string apiname);



    }
}
