using System.Threading.Tasks;
using RedocDemo.Entities;

namespace RedocDemo.Services
{
    public interface ICategoryService
    {
        Task<Category[]> GetAll();

        Task<Category> Get(string id);

        Task<Category> Post(Category category);

        Task<Category> Put(Category category);

        Task Delete(string id);
    }
}