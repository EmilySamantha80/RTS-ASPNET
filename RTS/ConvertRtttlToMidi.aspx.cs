using RTS.T4A;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RTS
{
    public partial class ConvertRtttlToMidi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ErrorDiv.Visible = false;
                ErrorMessage.InnerText = "";
                RtttlText.Value = Properties.Settings.Default.DefaultConvertRtttl;
                Page.Title = Properties.Settings.Default.PageTitle + " - Convert your own RTTTL to MIDI";

                string rtttl = Request["rtttl"];
                if (rtttl != null)
                {
                    ConvertToMidi(rtttl);
                }

            }

        }

        private void ConvertToMidi(string rtttlText)
        {
            var rtttl = Rtttl.ParseRtttl(rtttlText);
            if (rtttl.HasParseError)
            {
                ErrorDiv.Visible = true;
                ErrorMessage.InnerText = rtttl.ParseErrorMessage;
                RtttlText.Value = rtttlText;
                return;

                // Parsing error! Switching to Error tone
                //rtttl = Rtttl.ParseRtttl(Rtttl.ErrorRtttl);
            }
            var midiBytes = Rtttl.ConvertRtttlToMidi(rtttl, Properties.Settings.Default.MidiProgram);
            Response.Clear();
            Response.Headers.Add("Content-disposition", "attachment;filename=\"" + rtttl.Name + ".mid\"");
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(midiBytes);
            Response.End();
            return;

        }
    }
}