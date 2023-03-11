using Consimple.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static Consimple.Model.Client;
using static Consimple.Model.ConsimpleResult;

namespace Consimple.Controllers
{
    public class BirhdayPerson
    {
        public string BirthdayDate { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
   
    public class ConsimpleController : ControllerBase
    {
        private readonly ApplicationContext context;
        public ConsimpleController(ApplicationContext _context)
        {
            context = _context;
        }

        //[Authorize]
        //[HttpPut]
        //[Route("CreateClient")]
        //public async Task<IActionResult> CreateClient(ClientDto client)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    return Ok();
        //}

        [Authorize]
        [HttpPost]
        [Route("GetBirthdatPersons")]
        public async Task<IActionResult> GetBirthdayPersons([FromBody]BirhdayPerson value)
        {
            DateTime dateOfBirthday;
            try
            {
                try
                {
                    dateOfBirthday = Convert.ToDateTime(value.BirthdayDate);
                }
                catch (Exception ex)
                {
                    throw new Exception(@"Parameterts error:" + ex.Message);
                }
                List<ClientDto> list = context.Clients.Where(t => t.BirthdayDate == dateOfBirthday).ToList(); //GetBirthdatPersonsRows(dateOfBirthday);
                return Ok(CheckClientResult.Ok(list));
            }
            catch (Exception ex)
            {
                Utils.SaveError("GetBirthdayPersons", ex, 0, User.Identity.Name);
                return BadRequest();
            }
        }
        //private List<ClientDto> GetBirthdatPersonsRows(DateTime dateOfBirthday)
        //{
        //    string sql = @"select * from Clients where BirthdayDate = @birthdayDate";
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("@birthdayDate", dateOfBirthday));
        //    var dt = DBWorker.ExecQueryWithParameters(sql, parameters);
        //    return GetBirthdatPersonsInfoFromDataTable(dt);
        //}
        //private static List<ClientDto> GetBirthdatPersonsInfoFromDataTable(DataTable dt)
        //{
        //    List<ClientDto> list = new List<ClientDto>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        ClientDto client = new ClientDto();
        //        client.ID = (int)row["ID"];
        //        client.Name = (string)row["Name"];
        //        list.Add(client);
        //    }
        //    return list;
        //}
        public class LastBuyers
        {
            public string CountDay { get; set; }
        }
        [Authorize]
        [HttpPost]
        [Route("GetLastBuyers")]
        public async Task<IActionResult> GetLastBuyers([FromBody] LastBuyers value)
        {
            int countDay;
            try
            {
                try
                {
                    countDay = Convert.ToInt32(value.CountDay);
                }
                catch (Exception ex)
                {
                    throw new Exception(@"Parameterts error:" + ex.Message);
                }
                DateTime dateTime = DateTime.Now.AddDays(-countDay);
                //Убрать дубликаты
                List<LastBuyersDto> listLastBuyers = (from purchase in context.Purchases
                                    join client in context.Clients on purchase.ClientID equals client.ID
                         where purchase.Date > dateTime
                         select new LastBuyersDto{ID = client.ID, Name = client.Name, Date = purchase.Date }).ToList();
               // List<ClientDto> listClients = context.Clients.Where(c => listClientID.Contains(c.ID)).ToList();
                return Ok(CheckClientResult.Ok(listLastBuyers));
            }
            catch (Exception ex)
            {
                Utils.SaveError("GetLastBuyers", ex, 0, User.Identity.Name);
                return BadRequest();
            }
        }
        //private List<ClientDto> GetLastBuyersRows(DateTime countDay)
        //{
        //    string sql = @"select * from Purchases where Date > @date";
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("@date", countDay));
        //    var dt = DBWorker.ExecQueryWithParameters(sql, parameters);
        //    return GetLastBuyersInfoFromDataTable(dt);
        //}
        //private static List<ClientDto> GetLastBuyersInfoFromDataTable(DataTable dt)
        //{
        //    List<ClientDto> list = new List<ClientDto>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        ClientDto client = new ClientDto();
        //        client.ID = (int)row["ID"];
        //        client.Name = (string)row["Name"];
        //        list.Add(client);
        //    }
        //    return list;
        //}
        public class Categories
        {
            public string ClientID { get; set; }
        }
        [Authorize]
        [HttpPost]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories([FromBody]Categories value)
        {
            int clientId;
            try
            {
                try
                {
                    clientId = Convert.ToInt32(value.ClientID);
                }
                catch (Exception ex)
                {
                    throw new Exception(@"Parameterts error:" + ex.Message);
                }
                //List<ClientCategoriesDto> list = GetCategoriesRows(clientId);
                //Доделать суму и групировку по категории
                List<ClientCategoriesDto> list = (from p in context.Purchases
                                                  join pp in context.PurchasesProducts on p.ID equals pp.PurchaseID
                                                  join prod in context.Products on pp.ProductID equals prod.ID
                                                  join cat in context.Categories on prod.CategoryID equals cat.ID
                                                  where p.ClientID == clientId
                                                  select new ClientCategoriesDto {ID = p.ID, Category = cat.Name, Count = pp.Count }).ToList();
                return Ok(CheckClientResult.Ok(list));
            }
            catch (Exception ex)
            {
                Utils.SaveError("GetCategories", ex, 0, User.Identity.Name);
                return BadRequest();
            }
        }
//        private List<ClientCategoriesDto> GetCategoriesRows(int clientId)
//        {
//            string sql = @" select Categories.Name, Sum([Count]) as [Count] from Purchases
//  left join Purchases_Products on Purchases_Products.PurchasesID = Purchases.ID
//  left join Products on Purchases_Products.ProductID = Products.ID
//left join Categories on Products.CategoryID = Categories.ID
//  where ClientID = @clientID
//  Group By Category";
//            List<SqlParameter> parameters = new List<SqlParameter>();
//            parameters.Add(new SqlParameter("@clientID", clientId));
//            var dt = DBWorker.ExecQueryWithParameters(sql, parameters);
//            return GetCategoriesInfoFromDataTable(dt);
//        }
//        private static List<ClientCategoriesDto> GetCategoriesInfoFromDataTable(DataTable dt)
//        {
//            List<ClientCategoriesDto> list = new List<ClientCategoriesDto>();
//            foreach (DataRow row in dt.Rows)
//            {
//                ClientCategoriesDto clientCategories = new ClientCategoriesDto();
//                clientCategories.ID = (int)row["ID"];
//                clientCategories.Category = (string)row["Category"];
//                clientCategories.Count = (int)row["Count"];
//                list.Add(clientCategories);
//            }
//            return list;
//        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> CreateUser([FromBody]string user)
        {

            return Ok();
        }
    }
}
