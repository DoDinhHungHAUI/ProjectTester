using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;

namespace OnlineShop.Data.Repositories
{
    public interface IPostCategoryRepository : IRepository<PostCategories>//Nó sẽ kết thừa hết các cái phương thức của IRepository
    {
      
    }

    public class PostCategoryRepository : RepositoryBase<PostCategories>, IPostCategoryRepository
    {
        public PostCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        

    }
}