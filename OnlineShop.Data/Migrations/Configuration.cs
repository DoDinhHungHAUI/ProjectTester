namespace OnlineShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OnlineShop.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineShop.Data.OnlineShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnlineShop.Data.OnlineShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            CreateProductCategorySample(context);
            CreateSlide(context);

            CreateContactDetail(context);
            CreateConfigTitle(context);

            /*var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new OnlineShopDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new OnlineShopDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "onlineShop",
                Email = "hungmien0411@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Technology Education"
            };

            manager.Create(user, "123456$");
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("hungmien0411@gmail.com");

            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });*/



        }

        private void CreateConfigTitle(OnlineShopDbContext context)
        {
            if (!context.SystemConfigs.Any(x => x.Code == "HomeTitle"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeTitle",
                    ValueString = "Trang chủ HungOnlineShop",

                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaKeyword"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaKeyword",
                    ValueString = "Trang chủ HungOnlineShop",

                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaDescription"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaDescription",
                    ValueString = "Trang chủ HungOnlineShop",

                });
            }
        }

        private void CreateProductCategorySample(OnlineShop.Data.OnlineShopDbContext context)
        {
            if(context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name = "Điện lạnh", Alias = "dien-lanh", Status = true },
                    new ProductCategory() { Name = "Viễn thông", Alias = "vien-thong", Status = true },
                    new ProductCategory() { Name = "Đồ gia dụng", Alias = "do-gia-dung", Status = true },
                    new ProductCategory() { Name = "Mỹ phẩm", Alias = "my-pham", Status = true }
                };

                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }    
        }

        private void CreateSlide(OnlineShopDbContext context)
        {
            if (context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                    new Slide()
                    {
                        Name = "Slide 1",
                        DisplayOrder = 1,
                        Status = true,
                        Url = "#",
                        Image = "~/Assets/client/images/1.png"
                    },
                    new Slide()
                    {
                        Name = "Slide 2",
                        DisplayOrder = 2,
                        Status = true,
                        Url = "#",
                        Image = "~/Assets/client/images/2.png"
                    }
                };
                context.Slides.AddRange(listSlide);
                context.SaveChanges();
            }
        }

        private void CreateContactDetail(OnlineShopDbContext context)
        {
            if(context.ContactDetails.Count() == 0)
            {
                try
                {
                    var contactDetail = new OnlineShop.Model.Models.ContactDetails()
                    {
                        Name = "Hung onlineShop",
                        Address = "Liệp Tuyết, Quốc Oai, Hà Nội",
                        Lat = 20.9855729,
                        Lng = 105.6020397,
                        Phone = "0963514289",
                        Website = "",
                        Other = "",
                        Status = true
                    };

                    context.ContactDetails.Add(contactDetail);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }
            }    
        }



    }
}
