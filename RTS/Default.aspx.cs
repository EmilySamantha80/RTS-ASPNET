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
                var searchText = Truncate(Request["Category"], 25);
                var category = SqlExec.GetCategoryByCategoryCode(HttpUtility.HtmlEncode(searchText));
                if (category == null)
                {
                    // Category not found, go back
                    Response.Redirect("Default.aspx", true);
                    return;
                }
                listTitle = "Category: " + category.CategoryName;
                Page.Title = Properties.Settings.Default.PageTitle + " - " + listTitle;
                results = SqlExec.GetTonesByCategory(searchText);
            }
            else if (!String.IsNullOrEmpty(Request["Search"]))
            {
                var searchText = Truncate(Request["Search"], 25);
                string search = HttpUtility.UrlDecode(searchText);
                listTitle = "Search Results: " + search;
                Page.Title = Properties.Settings.Default.PageTitle + " - " + listTitle;
                results = SqlExec.GetTonesBySearch(searchText);
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
                    //Response.Clear();
                    //Response.Write(ex.Message);
                    //Response.End();
                    Response.Redirect("Default.aspx");
                    return;
                }
                SqlExec.IncrementDownloadCount(toneId);
                var result = SqlExec.GetToneById(toneId);
                if (result == null)
                {
                    //Response.Clear();
                    //Response.Write("Invalid Tone ID!");
                    //Response.End();
                    Response.Redirect("Default.aspx");
                    return;
                }
                var rtttl = Rtttl.ParseRtttl(result.Rtttl);
                if (rtttl.HasParseError)
                {
                    Console.WriteLine("Parsing error! Switching to Error tone");
                    rtttl = Rtttl.ParseRtttl(Rtttl.ErrorRtttl);
                }
                var midiBytes = Rtttl.ConvertRtttlToMidi(rtttl, Properties.Settings.Default.MidiProgram);
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

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
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