using Booktopia.Domain.DomainModels;
using Booktopia.Domain.Identity;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly UserManager<BooktopiaAppUser> userManager;

        public UserController(UserManager<BooktopiaAppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {


            return View();
        }

        
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {

            if(file == null)
            {
               return RedirectToAction("AddUserToRole", "Account");
            }

            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            //read data from copy file

            List<User> users = getAllUsersFromFile(file.FileName);


            bool status = true;

            foreach (var item in users)
            {
                var userCheck = userManager.FindByEmailAsync(item.Email).Result;

                if (userCheck == null)
                {
                    var user = new BooktopiaAppUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCart = new ShoppingCart(),
                        Role = item.Role
                    };
                    var result = userManager.CreateAsync(user, item.Password).Result;

                    status = status && result.Succeeded;

                    object p = await userManager.AddToRoleAsync(user, item.Role);

                    
                }
                else
                {
                    continue;
                }
            }


            return RedirectToAction("AddUserToRole", "Account");
        }

        
        private List<User> getAllUsersFromFile(string fileName)
        {

            List<User> users = new List<User>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new Booktopia.Domain.Identity.User
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString(),
                            Role = reader.GetValue(3).ToString()
                        });
                      
                    }
                    
                }
                
            }


            return users;
        }

       
        [HttpGet]
        public FileContentResult ExportAllUsers()
        {
            string fileName = "ExportedUsers.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Users");

                worksheet.Cell(1, 1).Value = "User Id";
                worksheet.Cell(1, 2).Value = "User Email";
                worksheet.Cell(1, 3).Value = "User Role";


                HttpClient client = new HttpClient();


                string URI = "https://localhost:44339/api/AdminUser/getUsers";

                HttpResponseMessage responseMessage = client.GetAsync(URI).Result;

                var result = responseMessage.Content.ReadAsAsync<List<BooktopiaAppUser>>().Result;

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.Email.ToString();
                    worksheet.Cell(i + 1, 3).Value = item.Role.ToString();

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




