using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BikeR.Web.Configuration
{
    public class GoogleApiKey : ConfigurationElement
    {


        // Create a "font" element.
        [ConfigurationProperty("ClientId")]
        public String ClientId
        {
            get
            {
                return (String)this["ClientId"];
            }
            set
            {
                this["ClientId"] = value;
            }
        }

        [ConfigurationProperty("EmailAddress")]
        public String EmailAddress
        {
            get
            {
                return (String)this["EmailAddress"];
            }
            set
            {
                this["EmailAddress"] = value;
            }
        }

        [ConfigurationProperty("ClientSecret")]
        public String ClientSecret
        {
            get
            {
                return (String)this["ClientSecret"];
            }
            set
            {
                this["ClientSecret"] = value;
            }
        }

        [ConfigurationProperty("RedirectUris")]
        public String RedirectUris
        {
            get
            {
                return (String)this["RedirectUris"];
            }
            set
            {
                this["RedirectUris"] = value;
            }
        }

        [ConfigurationProperty("JavascriptOrigins")]
        public String JavascriptOrigins
        {
            get
            {
                return (String)this["JavascriptOrigins"];
            }
            set
            {
                this["JavascriptOrigins"] = value;
            }
        }




    }
}