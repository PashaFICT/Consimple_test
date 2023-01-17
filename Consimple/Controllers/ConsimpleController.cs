using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static Consimple.Model.Client;
using static Consimple.Model.ConsimpleResult;

namespace Consimple.Controllers
{
    public class Person
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string BirthdayDate { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
   
    public class ConsimpleController : ControllerBase
    {
        [HttpPost]
        [Route("Apex/GetBirthdatPersons")]
        public async Task<IActionResult> GetBirthdayPersons([FromBody]Person value)
        {
            DateTime dateOfBirthday;
            string login = "";
            string pass = "";
            try
            {
               
                try
                {
                    login = value.Login;
                    pass = value.Password;
                    dateOfBirthday = Convert.ToDateTime(value.BirthdayDate);
                }
                catch (Exception ex)
                {
                    throw new Exception(@"Parameterts error:" + ex.Message);
                }
                bool check = Utils.CheckUser(login, pass);
                if (!check)
                {
                    return Unauthorized();
                }
                List<ClientDto> list = GetBirthdatPersonsRows(dateOfBirthday);
                return Ok(CheckClientResult.Ok(list));
            }
            catch (Exception ex)
            {
                Utils.SaveError("GetBirthdayPersons", ex, 0, login);
                return BadRequest();
            }
        }
        private List<ClientDto> GetBirthdatPersonsRows(DateTime dateOfBirthday)
        {
            string sql = @"select * from Clients where BirthdayDate = @birthdayDate";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@birthdayDate", dateOfBirthday));
            var dt = DBWorker.ExecQueryWithParameters(sql, parameters);
            return GetBirthdatPersonsInfoFromDataTable(dt);
        }
        private static List<ClientDto> GetBirthdatPersonsInfoFromDataTable(DataTable dt)
        {
            List<ClientDto> list = new List<ClientDto>();
            foreach (DataRow row in dt.Rows)
            {
                ClientDto client = new ClientDto();
                client.ID = (int)row["ID"];
                client.Name = (string)row["Name"];
                list.Add(client);
            }
            return list;
        }
        [HttpPost]
        [Route("Apex/GetLastBuyers")]
        public async Task<IActionResult> GetLastBuyers(JObject value)
        {
            int countDay;
            string login = "";
            string pass = "";
            try
            {
                dynamic json = value;
                try
                {
                    login = json.login;
                    pass = json.password;
                    countDay = Convert.ToInt32(json.countDay);
                }
                catch (Exception ex)
                {
                    throw new Exception(@"Parameterts error:" + ex.Message);
                }
                bool check = Utils.CheckUser(login, pass);
                if (!check)
                {
                    return Unauthorized();
                }
                DateTime dateTime = DateTime.Now.AddDays(-countDay);
                List<ClientDto> list = GetLastBuyersRows(dateTime);
                return Ok(CheckClientResult.Ok(list));
            }
            catch (Exception ex)
            {
                Utils.SaveError("GetLastBuyers", ex, 0, login);
                return BadRequest();
            }
        }
        private List<ClientDto> GetLastBuyersRows(DateTime countDay)
        {
            string sql = @"select * from Purchases where Date > @date";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@date", countDay));
            var dt = DBWorker.ExecQueryWithParameters(sql, parameters);
            return GetLastBuyersInfoFromDataTable(dt);
        }
        private static List<ClientDto> GetLastBuyersInfoFromDataTable(DataTable dt)
        {
            List<ClientDto> list = new List<ClientDto>();
            foreach (DataRow row in dt.Rows)
            {
                ClientDto client = new ClientDto();
                client.ID = (int)row["ID"];
                client.Name = (string)row["Name"];
                list.Add(client);
            }
            return list;
        }

        [HttpPost]
        [Route("Apex/GetCategories")]
        public async Task<IActionResult> GetCategories(JObject value)
        {
            int clientId;
            string login = "";
            string pass = "";
            try
            {
                dynamic json = value;
                try
                {
                    login = json.login;
                    pass = json.password;
                    clientId = Convert.ToInt32(json.clientId);
                }
                catch (Exception ex)
                {
                    throw new Exception(@"Parameterts error:" + ex.Message);
                }
                bool check = Utils.CheckUser(login, pass);
                if (!check)
                {
                    return Unauthorized();
                }
                List<ClientCategoriesDto> list = GetCategoriesRows(clientId);
                return Ok(CheckClientResult.Ok(list));
            }
            catch (Exception ex)
            {
                Utils.SaveError("GetCategories", ex, 0, login);
                return BadRequest();
            }
        }
        private List<ClientCategoriesDto> GetCategoriesRows(int clientId)
        {
            string sql = @" select Category, Sum([Count]) as [Count] from Purchases
  left join Purchases_Products on Purchases_Products.PurchasesID = Purchases.ID
  left join Products on Purchases_Products.ProductID = Products.ID
  where ClientID = @clientID
  Group By Category";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@clientID", clientId));
            var dt = DBWorker.ExecQueryWithParameters(sql, parameters);
            return GetCategoriesInfoFromDataTable(dt);
        }
        private static List<ClientCategoriesDto> GetCategoriesInfoFromDataTable(DataTable dt)
        {
            List<ClientCategoriesDto> list = new List<ClientCategoriesDto>();
            foreach (DataRow row in dt.Rows)
            {
                ClientCategoriesDto clientCategories = new ClientCategoriesDto();
                clientCategories.ID = (int)row["ID"];
                clientCategories.Category = (string)row["Category"];
                clientCategories.Count = (int)row["Count"];
                list.Add(clientCategories);
            }
            return list;
        }
    }
}
