using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace RTS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //Set the DataDirectory path for the Application Settings
            string path = AssemblyInfo.GetCallingAssemblyPath();
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        void Application_BeginRequest(Object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            Response.Expires = -1500;
            Response.BufferOutput = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        void Application_End(object sender, EventArgs e)
        {
        }
    }
}