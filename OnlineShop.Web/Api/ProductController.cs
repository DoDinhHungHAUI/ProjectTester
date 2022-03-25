using AutoMapper;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.infrastructure.Core;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineShop.Web.infrastructure.Extensions;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using OfficeOpenXml;
using OnlineShop.Common;
using System.Threading.Tasks;

namespace OnlineShop.Web.Api
{
  
    [RoutePrefix("api/product")]
    
    public class ProductController : ApiControllerBase
    {
        #region Initialize
        private IProductService _productService;
        IReportOrderUserService _reportOrderUserService;
        public ProductController(IErrorService errorService , IProductService productService , IReportOrderUserService reportOrderUserService) : base(errorService)
        {
            this._productService = productService;
            this._reportOrderUserService = reportOrderUserService;
        }

        #endregion

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }

     
        [Route("getbyid/{id:int}")]
        [HttpGet]

        public HttpResponseMessage GetById(HttpRequestMessage request , int id)
        {
            return CreateHttpResponse(request, () =>
           {
               var model = _productService.GetById(id);

               var responseData = Mapper.Map<Product, ProductViewModel>(model);

               var response = request.CreateResponse(HttpStatusCode.OK, responseData);

               return response;
           });
        }

        [Route("create")]
        [HttpPost]
        
        [AllowAnonymous]

        public HttpResponseMessage create()
        {

            var newProduct = new Product();

            string imageName = null;
            string multiImage = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image

            var postedFile = httpRequest.Files["Image"];
            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            var val = httpRequest["Value"];


            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Assets/Images/");
          

            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];
                if(hpf.FileName == postedFile.FileName)
                {

                    //create custom filename
                    imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                    var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);
                    //multiImage = multiImage + imageName + "|";
                    postedFile.SaveAs(filePath);

                }
                else
                {

                    if (hpf.ContentLength > 0)
                    {
                        string FileName = new String(Path.GetFileNameWithoutExtension(hpf.FileName).Take(10).ToArray()).Replace(" ", "-");

                        //FileName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                        FileName = FileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(hpf.FileName);//Ten file

                        if (!File.Exists(sPath + FileName))
                        {
                            // SAVE THE FILES IN THE FOLDER.  
                            hpf.SaveAs(sPath + FileName);

                            multiImage = multiImage + FileName + "|";

                            //imagemaster obj = new imagemaster();
                            //obj.Remark = remark;
                            //obj.ImageName = FileName;
                            //wmsEN.imagemasters.Add(obj);
                            //wmsEN.SaveChanges();
                        }
                    }

                }
                    
               
            }

            //string sPath = "";
            //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/image/");
            //var request = System.Web.HttpContext.Current.Request;
            //System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            //var remark = request["remark"].ToString();
            //for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            //{
            //    System.Web.HttpPostedFile hpf = hfc[iCnt];
            //    if (hpf.ContentLength > 0)
            //    {
            //        string FileName = (Path.GetFileName(hpf.FileName));
            //        if (!File.Exists(sPath + FileName))
            //        {
            //            // SAVE THE FILES IN THE FOLDER.  
            //            hpf.SaveAs(sPath + FileName);
            //            imagemaster obj = new imagemaster();
            //            obj.Remark = remark;
            //            obj.ImageName = FileName;
            //            wmsEN.imagemasters.Add(obj);GetImage
            //            wmsEN.SaveChanges();
            //        }
            //    }
            //}


            ProductViewModel productVm = JsonConvert.DeserializeObject<ProductViewModel>(val);//convert json to Object

            //Save to DB

            productVm.Image = imageName;
            productVm.MoreImages = multiImage;
            newProduct.UpdateProduct(productVm);
            newProduct.CreatedDate = DateTime.Now;
            _productService.Add(newProduct);
            _productService.Save();

            var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);

            return Request.CreateResponse(HttpStatusCode.Created, responseData);

        }

        /*public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

               }
               else
               {
                   var newProduct = new Product();

                  *//* string imageName = null;
                   var httpRequest = HttpContext.Current.Request;
                   //Upload Image

                   var postedFile = httpRequest.Files["Image"];
                   //productVm = httpRequest["Value"];
                   //create custom filename
                   imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                   imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                   var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);
                   postedFile.SaveAs(filePath);

                   newProduct.Image = imageName;*//*

                   newProduct.UpdateProduct(productVm);
                   newProduct.CreatedDate = DateTime.Now;
                   _productService.Add(newProduct);
                   _productService.Save();

                   var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);

               }

               return response;
           });
        }*/

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
           {
                HttpResponseMessage response = null;
                var url = HttpContext.Current.Request.Url;

               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest , ModelState);
               }
               else
               {
                   var newProduct = new Product();
                   var httpRequest = HttpContext.Current.Request;
                   //Upload Image
                   var postedFile = httpRequest.Files["Image"];
                   var val = httpRequest["Value"];

                   ProductViewModel productVm = JsonConvert.DeserializeObject<ProductViewModel>(val);//convert json to Object

                   var dbProduct = _productService.GetById(productVm.ID);//product cũ

                   dbProduct.UpdateProduct(productVm);

                   dbProduct.UpdatedDate = DateTime.Now;

                   //Lấy tên hình ảnh

                   var listString = dbProduct.Image.Split('/');

                   var imageName = listString[listString.Length - 1];
                   var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);

                   if(File.Exists(filePath))
                   {
                       dbProduct.Image = imageName;
                   }
                   else
                   {
                       //Xóa bỏ image cũ
                       File.Delete(filePath);
                       //Add Image mới vào
                       //create custom filename
                       imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                       imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                       filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);

                       dbProduct.Image = imageName;
                       postedFile.SaveAs(filePath);
                   }

                   _productService.Update(dbProduct);
                   _productService.Save();

                   var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);
               }
               return response;
           });
        }
        
        [Route("delete/{id}")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request , int id)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
               }
               else
               {
                   var oldProduct = _productService.Delete(id);
                   
                   var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + oldProduct.Image);
                   if (File.Exists(filePath))
                   {
                       File.Delete(filePath);
                   }

                   _productService.Save();
                   var responseData = Mapper.Map<Product, ProductViewModel>(oldProduct);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);
               }

               return response;
           });
        }

        [HttpGet]
        [Route("GetImage")]
        //Download image file api
        public HttpResponseMessage GetImage(HttpRequestMessage request)
        {
            //Create HTTP Response

            /*HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            //Set the File Path
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Image/") + image + ".PNG";
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.  
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", image);
                throw new HttpResponseException(response);
            }
            //read the File into a Byte Array
            byte[] bytes = File.ReadAllBytes(filePath);
            //Set the Response Content.  
            response.Content = new ByteArrayContent(bytes);
            //Set the Response Content Length.  
            response.Content.Headers.ContentLength = bytes.LongLength;
            //Set the Content Disposition Header Value and FileName.  
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = image + ".PNG";
            //Set the File Content Type.  
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(image + ".PNG"));
            return response;*/

            return CreateHttpResponse(request, () =>
            {
                var url = HttpContext.Current.Request.Url;

                var responseData = url.Scheme + "://" + url.Host + ":" + url.Port + "/Assets/Images/";

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }

        [HttpGet]
        [Route("getBaseUrl")]
        //Download image file api
        public HttpResponseMessage getBaseUrl(HttpRequestMessage request)
        {
           
            return CreateHttpResponse(request, () =>
            {
                var url = HttpContext.Current.Request.Url;

                var responseData = url.Scheme + "://" + url.Host + ":" + url.Port + "/Assets/Templates/productImportTemplate.xlsx";

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }


        [HttpGet]
        [Route("getReportUrl")]
        //Download image file api
        public HttpResponseMessage getReportUrl(HttpRequestMessage request)
        {

            return CreateHttpResponse(request, () =>
            {
                var url = HttpContext.Current.Request.Url;

                var responseData = url.Scheme + "://" + url.Host + ":" + url.Port + "/Report";

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }


        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được server hỗ trợ");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            //do stuff with files if you wish

            //do stuff with files if you wish
            if (result.FormData["categoryId"] == null)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bạn chưa chọn danh mục sản phẩm.");
            }

            //Upload files
            int addedCount = 0;
            int categoryId = 0;
            int.TryParse(result.FormData["categoryId"], out categoryId);
            foreach (MultipartFileData fileData in result.FileData)
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Yêu cầu không đúng định dạng");
                }

                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var fullPath = Path.Combine(root, fileName);

                File.Copy(fileData.LocalFileName, fullPath, true);

                //insert to DB

                var listProduct = this.ReadProductFromExcel(fullPath, categoryId);
                if(listProduct.Count > 0)
                {
                    foreach(var product in listProduct)
                    {
                        _productService.Add(product);
                        addedCount++;
                    }
                    _productService.Save();
                }    

            }

            return Request.CreateResponse(HttpStatusCode.OK, "Đã nhập" + addedCount + " sản phẩm thành công.");
        }
        private List<Product> ReadProductFromExcel(string fullPath, int categoryId)
        {
            List<Product> listProduct = new List<Product>();
            try
            {
                // If you are a commercial business and have
                // purchased commercial licenses use the static property
                // LicenseContext of the ExcelPackage class:
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                // If you use EPPlus in a noncommercial context
                // according to the Polyform Noncommercial license:

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(fullPath)))
                {
                    //ExcelWorksheet workSheet = package.Workbook.Worksheets[1]; ;
                    var currentSheet = package.Workbook.Worksheets;
                    ExcelWorksheet workSheet = currentSheet.First();

                    //try
                    //{
                    //    workSheet = package.Workbook.Worksheets[1];
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}

                    //List<Product> listProduct = new List<Product>();
                    ProductViewModel productViewModel;
                    Product product;

                    decimal originalPrice = 0;
                    decimal price = 0;
                    decimal promotionPrice;

                    bool status = false;
                    int warranty;
                    int quality;

                    for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    {
                        productViewModel = new ProductViewModel();
                        product = new Product();
                        productViewModel.Name = workSheet.Cells[i, 1].Value.ToString();
                        productViewModel.Alias = StringHelper.ToUnsignString(productViewModel.Name);
                        productViewModel.Description = workSheet.Cells[i, 2].Value.ToString();
                        if (int.TryParse(workSheet.Cells[i, 3].Value.ToString(), out warranty))
                        {
                            productViewModel.Warranty = warranty;
                        }

                        decimal.TryParse(workSheet.Cells[i, 4].Value.ToString().Replace(",", ""), out originalPrice);
                        productViewModel.OriginalPrice = originalPrice;

                        decimal.TryParse(workSheet.Cells[i, 5].Value.ToString().Replace(",", ""), out price);
                        productViewModel.Price = price;

                        if (decimal.TryParse(workSheet.Cells[i, 6].Value.ToString(), out promotionPrice))
                        {
                            productViewModel.PromotionPrice = promotionPrice;

                        }

                        if (int.TryParse(workSheet.Cells[i, 7].Value.ToString(), out quality))
                        {
                            productViewModel.Quantity = quality;
                        }

                        productViewModel.Content = workSheet.Cells[i, 8].Value.ToString();
                        productViewModel.MetaKeyword = workSheet.Cells[i, 9].Value.ToString();
                        productViewModel.MetaDescription = workSheet.Cells[i, 10].Value.ToString();

                        productViewModel.CategoryID = categoryId;

                        bool.TryParse(workSheet.Cells[i, 11].Value.ToString(), out status);
                        productViewModel.Status = status;

                        productViewModel.Color = workSheet.Cells[i, 12].Value.ToString();
                        productViewModel.Model = workSheet.Cells[i, 13].Value.ToString();
                        productViewModel.whereProduct = workSheet.Cells[i, 14].Value.ToString();

                        product.UpdateProduct(productViewModel);
                        listProduct.Add(product);

                    }
                    return listProduct;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listProduct;
           
        }

        [HttpGet]
        [Route("ExportXls/{filter?}")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request , string filter = null)
        {
            string fileName = string.Concat("Product_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");

            string filePath = HttpContext.Current.Server.MapPath("~/Report");
            //var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);//~/folderReport/_product
            try
            {
                var data = _productService.GetListProduct(filter).ToList();
                await ReportHelper.GenerateXls(data, fullPath);
               
                return request.CreateResponse(HttpStatusCode.OK, fileName);
            }
            catch(Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //[HttpGet]
        //[Route("ExportPdf")]
        //public async Task<HttpResponseMessage> ExportPdf(HttpRequestMessage request, string userId)
        //{
        //    string fileName = string.Concat("Product" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf");
        //    string filePath = HttpContext.Current.Server.MapPath("~/Report");
        //    if (!Directory.Exists(filePath))
        //    {
        //        Directory.CreateDirectory(filePath);
        //    }

        //    string fullPath = Path.Combine(filePath, fileName);
        //    try
        //    {
        //        var template = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Assets/Templates/OrderDetails.html"));

        //        //var replaces = new Dictionary<string, string>();
        //        var listReportOrderUser = _reportOrderUserService.GetReportOrder(userId).ToList();

        //        template = template.Parse(listReportOrderUser);
        //    }

        //}







    }
}
