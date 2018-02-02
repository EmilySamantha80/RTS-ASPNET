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
                RtttlText.Value = Properties.Settings.Default.DefaultConvertRtttl;
                Page.Title = Properties.Settings.Default.PageTitle + " - Convert your own RTTTL to MIDI";
            }
        }

        protected void ConvertToMidi_Click(object sender, EventArgs e)
        {
            var rtttlText = RtttlText.Value;
            var rtttl = new Rtttl.RtttlTone();
            var parseResult = Rtttl.ParseRtttl(rtttlText, ref rtttl);
            if (!parseResult)
            {
                Console.WriteLine("Parsing error! Switching to Error tone");
                Rtttl.ParseRtttl(Rtttl.ErrorRtttl, ref rtttl);
            }
            var midiChars = Rtttl.ConvertRtttlToMidi(rtttl, Properties.Settings.Default.MidiProgram);
            var midiBytes = midiChars.Select(c => (byte)c).ToArray();
            Response.Clear();
            Response.Headers.Add("Content-disposition", "attachment;filename=" + rtttl.Name + ".mid");
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(midiBytes);
            Response.End();
            return;

        }
    }
}