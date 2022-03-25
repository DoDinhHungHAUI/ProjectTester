using OnlineShop.Common;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Repositories;
using OnlineShop.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Service
{

    public interface IProductService
    {
        Product Add(Product Product);
        void Update(Product Product);
        Product Delete(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetLastestLaptop(int top);

        IEnumerable<Product> GetLastestPhone(int top);
        IEnumerable<Product> GetHotProduct(int top);
        IEnumerable<Product> GetListProductByName(string name);
        IEnumerable<Product> GetListProduct(string keyword);
        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow);
        IEnumerable<Product> GetReatedProducts(int id, int top);
        Product GetById(int id);
        void Save();

        IEnumerable<Tag> GetListTagByProductId(int id);
        Tag GetTag(string tagId);
        void IncreaseView(int id);
        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow);

        bool SellProduct(int productId, int quantity);

    }

    class ProductService: IProductService
    {
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private IUnitOfWork _unitOfWork;
        private IProductCategoryRepository _productCategoryRepository;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository,
            ITagRepository _tagRepository, IUnitOfWork unitOfWork , IProductCategoryRepository productCategoryRepository)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = _tagRepository;
            this._unitOfWork = unitOfWork;
            this._productCategoryRepository = productCategoryRepository;
        }

        public Product Add(Product Product)
        {
            var product = _productRepository.Add(Product);
            _unitOfWork.Commit();

            if(!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');

                for(var i = 0; i<tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if(_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }
            return product;
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _productRepository.GetMulti(x => x.Status).Where(x => x.PromotionPrice > 0).OrderByDescending(x => (x.Price - x.PromotionPrice)).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetLastestLaptop(int top)
        {
            var listCategoryId = _productCategoryRepository.GetMulti(x => x.Status).Where(x => x.ParentID == 44).Select(x => x.ID).ToList();

            return _productRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Where(x => listCategoryId.Contains(x.CategoryID)).Take(top);
        }

        public IEnumerable<Product> GetLastestPhone(int top)
        {

            var listCategoryId = _productCategoryRepository.GetMulti(x => x.Status).Where(x => x.ParentID == 45).Select(x => x.ID).ToList();


            return _productRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreatedDate).Where(x => listCategoryId.Contains(x.CategoryID)).Take(top);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize , string sort, out int totalRow)
        {
            IEnumerable<Product> query;
            if (_productRepository.checkParentCategory(categoryId) == true)
            {
                query = _productRepository.GetListProductByParentCategry(categoryId).ToList();
            }
            else
            {
                query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);
            }
           
            switch(sort)
            {
                case "popular" :
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }    

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetListProductByName(string name)
        {
            return _productRepository.GetMulti(x => x.Status && x.Name.Contains(name));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product Product)
        {
            _productRepository.Update(Product);
            if(!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for(var i = 0; i < tags.Length;i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if(_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.DeleteMulti(x => x.ProductID == Product.ID);
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }    
            }    
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            var product = _productRepository.GetSingleById(id);
            return _productRepository.GetMulti(x => x.Status && x.ID != id && x.CategoryID == product.CategoryID).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public Tag GetTag(string tagId)//id , name , type . Lấy ra tag dựa vào Id
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }

        public void IncreaseView(int id)
        {
            var product = _productRepository.GetSingleById(id);
            if(product.ViewCount.HasValue)
            {
                product.ViewCount += 1;
            }
            else
            {
                product.ViewCount = 1;
            }
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow)
        {
            var model = _productRepository.GetListProductByTag(tagId, page, pagesize, out totalRow);
            return model;
        }

        public bool SellProduct(int productId , int quantity)
        {
            var product = _productRepository.GetSingleById(productId);
            if(product.Quantity < quantity)
            {
                return false;
            }
            product.Quantity -= quantity;
            return true;
        }

        public IEnumerable<Product> GetListProduct(string keyword)
        {
            IEnumerable<Product> query;
            if (!string.IsNullOrEmpty(keyword))
                query = _productRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                query = _productRepository.GetAll();
            return query;
        }

    }
}
