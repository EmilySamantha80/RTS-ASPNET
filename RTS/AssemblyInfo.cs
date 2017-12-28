using System;
using System.IO;
using System.Reflection;

namespace RTS
{
    public static class AssemblyInfo
    {
        #region Assembly Attribute Accessors

        //Gets the entry assembly for web apps
        //http://stackoverflow.com/questions/4277692/getentryassembly-for-web-applications
        //Note: On App_Start System.Web.HttpContext.Current.ApplicationInstance is NULL
        private static Assembly GetWebEntryAssembly()
        {
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                return null;
            }
            var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }
            return type == null ? null : type.Assembly;
        }

        public static string GetCallingAssemblyPath()
        {
            var path = Path.GetDirectoryName(new Uri(GetCallingAssembly().CodeBase).LocalPath);
            return path;
        }

        //Try as hard as possible to get the proper entry assembly
        public static Assembly GetCallingAssembly()
        {
            Assembly assembly = null;
            assembly = GetWebEntryAssembly();
            if (assembly == null)
                assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            return assembly;
        }

        public static string AssemblyFileName
        {
            get
            {
                string location = GetCallingAssembly().Location;
                return location.Substring(location.LastIndexOf(@"\") + 1);
            }
        }

        public static string AssemblyLocation
        {
            get
            {
                //Assembly.Location is not always accurate, especially when used in Web Applications and NUnit. Assembly.CodeBase is, but must be manipulated.
                //http://stackoverflow.com/questions/52797/how-do-i-get-the-path-of-the-assembly-the-code-is-in
                //The Assembly.Location property sometimes gives you some funny results when using NUnit (where assemblies run from a temporary folder), so I prefer to use CodeBase
                //which gives you the path in URI format, then UriBuild.UnescapeDataString removes the File:// at the beginning, and GetDirectoryName changes it to the normal windows format.
                string codeBase = GetCallingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return String.Empty;
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return GetCallingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
