using AutoMapper;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Web.infrastructure.Extensions;
using BotDetect.Web.Mvc;
using OnlineShop.Common;
using OnlineShop.Web.infrastructure.Custom;
using CaptchaMvc.HtmlHelpers;
using System.Threading.Tasks;

namespace OnlineShop.Web.Controllers
{
    public class ContactController : Controller
    {

        IContactDetailService _contactDetailService;
        IFeedBackService _feedBackService;
        IUnitOfWork _iUnitOfWork;

        public ContactController(IContactDetailService contactDetailService , IUnitOfWork iUnitOfWork , IFeedBackService feebBackService)
        {
            this._contactDetailService = contactDetailService;
            this._iUnitOfWork = iUnitOfWork;
            this._feedBackService = feebBackService;
        }

        // GET: Contact
        public ActionResult Index()
        {
           
            FeedBackViewModel viewmodel = new FeedBackViewModel();

            viewmodel.ContactDetail = getContactDetail();
      
            return View(viewmodel);
        }

        [HttpPost]
        //[CaptchaValidation("CaptchaCode", "contactCaptcha", "Mã xác nhận không đúng")]
        [ValidateGoogleCaptcha]
        public async Task<ActionResult> Index(FeedBackViewModel feedBackVM)
        {

            if (ModelState.IsValid)
            {
                FeedBack newFeedBack = new FeedBack();
                newFeedBack.UpdateFeedback(feedBackVM);
                newFeedBack.CreatedDate = DateTime.Now;
                _feedBackService.Create(newFeedBack);
                _feedBackService.Save();

                ViewData["SuccessMsg"] = "Gửi phản hồi thành công";

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/Contact_template.html"));
                content = content.Replace("{{Name}}", feedBackVM.Name);
                content = content.Replace("{{Email}}", feedBackVM.Email);
                content = content.Replace("{{Message}}", feedBackVM.Message);
                var adminEmail = ConfigHelper.AdminEmail;
                MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

                feedBackVM.Name = "";
                feedBackVM.Message = "";
                feedBackVM.Email = "";

            }

            feedBackVM.ContactDetail = getContactDetail();

            return View(feedBackVM);
        }


        private ContactDetailViewModel getContactDetail()
        {
            var model = _contactDetailService.GetDefaultContact();

            var contactViewModel = Mapper.Map<ContactDetails, ContactDetailViewModel>(model);
            return contactViewModel;
        }



    }
}