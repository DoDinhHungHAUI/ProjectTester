using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Repositories;
using OnlineShop.Model.Models;

namespace OnlineShop.Service
{
    public interface IFeedBackService
    {
        FeedBack Create(FeedBack feedback);

        void Save();
    }

    public class FeedBackService : IFeedBackService
    {
        private IFeedBackRepository _feedBackRepository;
        private IUnitOfWork _unitOfWork;

        public FeedBackService(IFeedBackRepository feedbackRepository, IUnitOfWork unitOfWork)
        {
            this._feedBackRepository = feedbackRepository;
            this._unitOfWork = unitOfWork;
        }

        public FeedBack Create(FeedBack feedback)
        {
            return this._feedBackRepository.Add(feedback);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}