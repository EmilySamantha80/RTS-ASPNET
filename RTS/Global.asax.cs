using System;
using System.Collections.Generic;
using System.IO;
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
            // Create the [DbProviderName].ConnectionString.txt file with the connection string as the only line and include
            // it in the base folder of the project, setting "Copy Always" for the "Copy to output directory" property.
            // Example filename: System.Data.SqlClient.ConnectionString.txt
            var path = Path.Combine(AssemblyInfo.GetCallingAssemblyPath(), Properties.Settings.Default.DbProviderName + ".ConnectionString.txt");
            var connectionString = File.ReadAllLines(path);
            SqlExec.DbConnectionString = connectionString[0];

            //RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapPageRoute("", "Default", "~/Default.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            Response.Expires = -1500;
            Response.BufferOutput = true;
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}