using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System.Web.Http;


namespace BikeR.MobileWs.DataObjects
{
    [AuthorizeLevel(AuthorizationLevel.User)] 
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}