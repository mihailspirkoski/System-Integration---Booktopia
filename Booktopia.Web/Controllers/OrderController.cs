using Booktopia.Domain.DomainModels;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Booktopia.Web.Controllers
{
    [Authorize(Roles = "Administrator, StandardUser")]
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();


            string URI = "https://localhost:44339/api/AdminOrder/GetOrders";

            HttpResponseMessage responseMessage = client.GetAsync(URI).Result;

            var result = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

            return View(result);
        }

        
        public IActionResult Details(Guid id)
        {
            HttpClient client = new HttpClient();


            string URI = "https://localhost:44339/api/AdminOrder/GetDetailsForOrder";

            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URI, content).Result;


            var result = responseMessage.Content.ReadAsAsync<Order>().Result;


            /*
             
            BaseEntity baseEntity = new BaseEntity();
            baseEntity.Id = id;

            Order result = await this._orderService.getOrderDetails(baseEntity);


            return View(result);
             */


            return View(result);
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            HttpClient client = new HttpClient();


            string URI = "https://localhost:44339/api/AdminOrder/GetDetailsForOrder";

            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URI, content).Result;


            var result = responseMessage.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);


            document.Content.Replace("{{OrderId}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);
            document.Content.Replace("{{Date}}", DateTime.Now.ToString());

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.BooksInOrder)
            {
                totalPrice += (item.Quantity * item.OrderedBook.BookPrice);
                sb.AppendLine("\"" + item.OrderedBook.BookName + "\"" + " by author: " + item.OrderedBook.Author + ", with quantity of: " + item.Quantity + " and price of: " + item.OrderedBook.BookPrice + "$");
            }


            document.Content.Replace("{{BookList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "AllOrders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Customer Email";


                HttpClient client = new HttpClient();


                string URI = "https://localhost:44339/api/AdminOrder/GetOrders";

                HttpResponseMessage responseMessage = client.GetAsync(URI).Result;

                var result = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.BooksInOrder.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Book-" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.BooksInOrder.ElementAt(p).OrderedBook.BookName + "by: " + item.BooksInOrder.ElementAt(p).OrderedBook.Author;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }
    }
}
