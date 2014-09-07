using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeR.WPApp.DataModel
{
    public class NfcField
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "friendlyname")]
        public string FriendlyName { get; set; }

        [JsonProperty(PropertyName = "tagcontent")]
        public string TagContent { get; set; }


        [JsonProperty(PropertyName = "tagstatus")]
        public string TagStatus { get; set; }


        [JsonProperty(PropertyName = "tagkind")]
        public string TagKind { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }


        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }


        [JsonProperty(PropertyName = "lastlat")]
        public string LastLat { get; set; }


        [JsonProperty(PropertyName = "lastlon")]
        public string LastLon { get; set; }


        [JsonProperty(PropertyName = "lastaddress")]
        public string LastAddress { get; set; }
    }
}
