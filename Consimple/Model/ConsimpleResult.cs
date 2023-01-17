using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Consimple.Model.Client;

namespace Consimple.Model
{
    public class ConsimpleResult
    {
        public enum ResultType
        {
            Ok = 0,
            Error = 5,
            ErrorDB = 1,
            Dublicate_TxnID = 215,
            WrongAmount = 275

        }

        public class ClientResult
        {
            public List<ClientDto> InfoClient { get; set; }
            public List<ClientCategoriesDto> InfoCategory { get; set; }
            public int Result { get; set; }
            public string Comment { get; set; }

            public static ClientResult Ok()
            => new ClientResult()
            {

            };
        }

        [XmlRoot("response")]
        public class CheckClientResult : ClientResult
        {
            
            public static CheckClientResult Ok(List<ClientDto> clients)
            {
                CheckClientResult checkClient = new CheckClientResult();
                if (clients.Count != 0)
                {
                    checkClient.InfoClient = clients;
                    checkClient.Result = (int)ResultType.Ok;
                    checkClient.Comment = ResultCode.code[0];
                }
                else
                {
                    checkClient.Result = (int)ResultType.Error;
                    checkClient.Comment = ResultCode.code[5];
                }

                return checkClient;
            }



            public static ClientResult Ok(List<ClientCategoriesDto> clientsCategory)
            {
            ClientResult payBilling = new ClientResult();
                if (clientsCategory.Count != 0)
                {
                    payBilling.InfoCategory = clientsCategory;
                    payBilling.Result = (int)ResultType.Ok;
                    payBilling.Comment = ResultCode.code[0];
                }
                else
                {
                    payBilling.Result = (int)ResultType.Error;
                    payBilling.Comment = ResultCode.code[5];
                }
                return payBilling;
            }
        }
    }
    public static class ResultCode
    {
        public static Dictionary<int, string> code = new Dictionary<int, string>()
{
    { 0, "OK"},
    { 1, "Temporary Database Error. Try Again Later"},
    { 4, "Incorrect User Account Format"},
    { 5, "The Account Doesn't Exist"},
    { 7, "Acceptance of Payment is Prohibited"},
    { 215, "Duplicate Transactions"},
    { 275, "Invalid Amount"},
    { 300, "Unknown Fatal Error"}
};
    }
}
