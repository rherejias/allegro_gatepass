using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GatePass.Library
{
    public class ConnectionString
    {
        private static DataLayerLibrary.DataLayer dataLayer = null;

        static ConnectionString()
        {
            if (dataLayer == null)
            {
                dataLayer = new DataLayerLibrary.DataLayer(ConfigurationManager.AppSettings["env"].ToString() + "_gatepass");
            }
        }

        public static DataLayerLibrary.DataLayer returnCon
        {
            get
            {
                return dataLayer;
            }
        }
    }
}