using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace OnlineShop.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);
        bool checkParentCategory(int categoryId);

        
        IEnumerable<Product> GetListProductByParentCategry(int categoryId);
        
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }




        public IEnumerable<Product> GetListProductByParentCategry(int categoryId)
        {
            //var query = from p in DbContext.Products where (from pc in DbContext.ProductCategories where pc.ParentID == categoryId && pc.Status select pc.ID).Contains(p.CategoryID) select p;
            var query1 = DbContext.ProductCategories.Where(pc => pc.ParentID == categoryId && pc.Status).Select(pc => pc.ID).ToList();
            var query = DbContext.Products.Where(p => query1.Contains(p.CategoryID)).Select(p => p).ToList();

            return query;
        }

        public bool checkParentCategory(int categoryId)
        {
            var query = DbContext.ProductCategories.Where(p => p.ParentID == categoryId && p.Status).ToList();
            if(query.Count() > 0)
            {
                return true;
            }    

            return false;
        }


        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Products
                        join pt in DbContext.ProductTag
                        on p.ID equals pt.ProductID
                        where pt.TagID.Trim() == tagId
                        select p;

            //var que = query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}