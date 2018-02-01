using RTS.T4A;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RTS
{
    public partial class ViewTone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int toneId;
            if (!String.IsNullOrEmpty(Request["ToneId"]))
            {
                
                try
                {
                    toneId = int.Parse(Request["ToneId"]);
                }
                catch (Exception ex)
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
            } else
            {
                Response.Redirect("Default.aspx");
                return;
            }
            SqlExec.IncrementDownloadCount(toneId);
            var result = SqlExec.GetToneById(toneId);
            if (result == null)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            Page.Title = Properties.Settings.Default.PageTitle + " (" + result.Artist + " - " + result.Title + ")";

            ToneArtist.InnerText = result.Artist;
            ToneTitle.InnerText = result.Title;
            ToneDownloads.InnerText = string.Format("{0:n0}", result.Counter);
            ToneRtttl.InnerText = result.Rtttl;
            TonePreviewLink.HRef = "Default.aspx?MIDI=" + result.ToneId.ToString();

        }
    }
}