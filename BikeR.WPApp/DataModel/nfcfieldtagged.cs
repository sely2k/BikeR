using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeR.WPApp.DataModel
{
    public class NfcFieldTagged
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nfcfieldid")]
        public string NfcFieldId { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Lat { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public string Lon { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "scannedtime")]
        public DateTime ScannedTime { get; set; }


        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "tagownerid")]
        public string TagOwnerId { get; set; }

        
    }
}
