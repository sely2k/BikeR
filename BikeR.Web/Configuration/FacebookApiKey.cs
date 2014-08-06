using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BikeR.Web.Configuration
{
    public class FacebookApiKey : ConfigurationElement
    {

        [ConfigurationProperty("AppId")]
        public String AppId
        {
            get
            {
                return (String)this["AppId"];
            }
            set
            {
                this["AppId"] = value;
            }
        }


        [ConfigurationProperty("AppSecret")]
        public String AppSecret
        {
            get
            {
                return (String)this["AppSecret"];
            }
            set
            {
                this["AppSecret"] = value;
            }
        }
    }
}