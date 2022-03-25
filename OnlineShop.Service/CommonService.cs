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
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlides();
        SystemConfig GetSystemConfig(string code);
    }

    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;
        IUnitOfWork _unitOfWork;
        ISystemConfigRepository _systemconfigRepository;
        ISlideRepository _slideRepository;
        public CommonService(IFooterRepository footerRepository , IUnitOfWork unitOfWork , ISlideRepository slideRepository , ISystemConfigRepository systemconfigRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _slideRepository = slideRepository;
            _systemconfigRepository = systemconfigRepository;
        }

        public IEnumerable<Slide> GetSlides()
        {
            return _slideRepository.GetMulti(x => x.Status);
        }


        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }

        public SystemConfig GetSystemConfig(string code)
        {
            return _systemconfigRepository.GetSingleByCondition(x => x.Code == code);
        }
    }
}
