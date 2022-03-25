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

    public interface IPostCategoryService
    {
        PostCategories Add(PostCategories postCategory);

        void Update(PostCategories postCategory);

        PostCategories Delete(int id);

        IEnumerable<PostCategories> GetAll();

        IEnumerable<PostCategories> GetAllByParentId(int parentId);

        PostCategories GetById(int id);

        void Save();
    }

    public class PostCategoryService : IPostCategoryService
    {
        private IPostCategoryRepository _postCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public PostCategoryService(IPostCategoryRepository postCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._postCategoryRepository = postCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public PostCategories Add(PostCategories postCategory)
        {
            return _postCategoryRepository.Add(postCategory);
        }

        public PostCategories Delete(int id)
        {
            return _postCategoryRepository.Delete(id);
        }

        public IEnumerable<PostCategories> GetAll()
        {
            return _postCategoryRepository.GetAll();
        }

        public IEnumerable<PostCategories> GetAllByParentId(int parentId)
        {
            return _postCategoryRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        }

        public PostCategories GetById(int id)
        {
            return _postCategoryRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(PostCategories postCategory)
        {
            _postCategoryRepository.Update(postCategory);
        }
    }
}
