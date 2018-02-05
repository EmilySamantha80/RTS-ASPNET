using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RTS.T4A;

namespace RTS
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var results = new List<Model.Tone>();
            string listTitle = "";

            SearchCountDiv.Visible = true;

            if (!String.IsNullOrEmpty(Request["Category"]))
            {
                var category = SqlExec.GetCategoryByCategoryCode(HttpUtility.HtmlEncode(Request["Category"]));
                if (category == null)
                {
                    // Category not found, go back
                    Response.Redirect("Default.aspx", true);
                    return;
                }
                listTitle = "Category: " + category.CategoryName;
                Page.Title = Properties.Settings.Default.PageTitle + " - " + listTitle;
                results = SqlExec.GetTonesByCategory(Request["Category"]);
            }
            else if (!String.IsNullOrEmpty(Request["Search"]))
            {
                string search = HttpUtility.UrlDecode((string)Request["Search"]);
                listTitle = "Search Results: " + search;
                Page.Title = Properties.Settings.Default.PageTitle + " - " + listTitle;
                results = SqlExec.GetTonesBySearch(Request["Search"]);
            }
            else if (!String.IsNullOrEmpty(Request["MIDI"]))
            {
                int toneId = -1;
                try
                {
                    toneId = int.Parse(Request["MIDI"]);
                }
                catch (Exception ex)
                {
                    Response.Clear();
                    Response.Write(ex.Message);
                    Response.End();
                    return;
                }
                SqlExec.IncrementDownloadCount(toneId);
                var result = SqlExec.GetToneById(toneId);
                var rtttl = Rtttl.ParseRtttl(result.Rtttl);
                if (rtttl.HasParseError)
                {
                    Console.WriteLine("Parsing error! Switching to Error tone");
                    rtttl = Rtttl.ParseRtttl(Rtttl.ErrorRtttl);
                }
                var midiChars = Rtttl.ConvertRtttlToMidi(rtttl, Properties.Settings.Default.MidiProgram);
                var midiBytes = midiChars.Select(c => (byte)c).ToArray();
                Response.Clear();
                Response.Headers.Add("Content-disposition", "attachment;filename=" + result.Artist + " - " + result.Title + ".mid");
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(midiBytes);
                Response.End();
                return;
            }
            else
            {
                listTitle = "Top " + Properties.Settings.Default.TopResultsCount.ToString() + " Ringtones";
                results = SqlExec.GetTopTones();
                SearchCountDiv.Visible = false;
            }

            LabelTitle.Text = listTitle;
            PopulateResults(results);

        }

        private void PopulateResults(List<Model.Tone> results)
        {
            if (results == null)
                return;

            LabelSearchCount.Text = results.Count.ToString();

            ResultsRepeater.DataSource = results;
            ResultsRepeater.DataBind();
        }

    }
}