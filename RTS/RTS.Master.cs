using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RTS
{
    public partial class RTS : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var requestStart = DateTime.Now;

            string userIPAddress = IPAddress.GetUserIP();
            SqlExec.UpdateHitCounter(userIPAddress);

            PopulateCategories();

            FooterProductName.InnerText = AssemblyInfo.AssemblyTitle;
            FooterCopyright.InnerText = AssemblyInfo.AssemblyCopyright;

            var uniqueHitCounter = SqlExec.GetSetting("UniqueHits");
            TotalUniqueHits.Text = long.Parse(uniqueHitCounter.SettingValue).ToString("N0");
            var pageViewHitCounter = SqlExec.GetSetting("PageViews");
            TotalPageViews.Text = long.Parse(pageViewHitCounter.SettingValue).ToString("N0");
            var countingSince = SqlExec.GetSetting("CountingSince");
            FooterCountingSince.InnerText = DateTime.Parse(countingSince.SettingValue).ToString("MMMM d, yyyy");
            TotalRingtones.Text = SqlExec.GetToneCount().ToString();


            var requestEnd = DateTime.Now;
            TimeSpan requestTime = requestEnd - requestStart;
            RequestTime.Text = ((int)requestTime.TotalMilliseconds).ToString();
        }

        private void PopulateCategories()
        {
            var link = new HyperLink();
            link.Text = "Top 10";
            link.NavigateUrl = "Default.aspx";
            link.CssClass = "categoryLink";
            Categories.Controls.Add(link);

            var results = SqlExec.GetCategories();
            foreach (var result in results)
            {
                link = new HyperLink();
                link.Text = result.CategoryName;
                link.NavigateUrl = "Default.aspx?Category=" + result.CategoryCode;
                link.CssClass = "categoryLink";
                Categories.Controls.Add(link);

                var literal = new Literal();
                literal.Text = " ";
                Categories.Controls.Add(literal);

            }

        }

    }
}