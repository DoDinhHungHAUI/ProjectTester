using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Common
{
    public class ConfigHelper
    {
        public const int pageSize = 8;
        public const int MaxPage = 5;


        public const string SMTPHost = "smtp.gmail.com";
        public const string SMTPPort = "587";
        public const string FromEmailAddress = "hungonlineshop@gmail.com";

        public const string FromEmailPassword = "dinhhung2k";
        public const string FromName = "HungMien";

        public const string AdminEmail = "hungmien0411@gmail.com";

        public const string currentLink = "";

        //public static string GetByKey(string key)
        //{
        //    return ConfigurationManager.AppSettings[key].ToString();
        //}

    }
}
