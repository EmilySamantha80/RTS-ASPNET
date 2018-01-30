using RTS.MIDI;
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
            //If no requested toneId was sent, use the following toneId
            int toneId = 87;
            if (!String.IsNullOrEmpty(Request["ToneId"]))
            {
                
                try
                {
                    toneId = int.Parse(Request["ToneId"]);
                }
                catch (Exception ex)
                {
                    Response.Clear();
                    Response.Write(ex.Message);
                    Response.End();
                    return;
                }
            }
            SqlExec.IncrementDownloadCount(toneId);
            var result = SqlExec.GetToneById(toneId);

            Page.Title = Properties.Settings.Default.PageTitle + " (" + result.Artist + " - " + result.Title + ")";

            ToneArtist.InnerText = result.Artist;
            ToneTitle.InnerText = result.Title;
            ToneDownloads.InnerText = result.Counter.ToString();
            ToneRtttl.InnerText = result.Rtttl;
            TonePreviewLink.HRef = "Default.aspx?MIDI=" + result.ToneId.ToString();

        }
    }
}