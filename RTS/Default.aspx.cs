using RTS.MIDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RTS
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var requestStart = DateTime.Now;

            FooterProductName.InnerText = AssemblyInfo.AssemblyTitle;
            FooterCopyright.InnerText = AssemblyInfo.AssemblyCopyright;

            Page.Title = Properties.Settings.Default.PageTitle;

            var uniqueHitCounter = SqlExec.GetSetting("UniqueHits");
            TotalUniqueHits.Text = long.Parse(uniqueHitCounter.SettingValue).ToString("N0");
            var pageViewHitCounter = SqlExec.GetSetting("PageViews");
            TotalPageViews.Text = long.Parse(pageViewHitCounter.SettingValue).ToString("N0");
            var countingSince = SqlExec.GetSetting("CountingSince");
            FooterCountingSince.InnerText = DateTime.Parse(countingSince.SettingValue).ToString("MMMM d, yyyy");
            TotalRingtones.Text = SqlExec.GetToneCount().ToString();
            var results = new List<Model.Tone>();
            string listTitle = "";

            SearchCountDiv.Visible = true;

            string userIPAddress = IPAddress.GetUserIP();
            SqlExec.UpdateHitCounter(userIPAddress);

            PopulateCategories();

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
                listTitle = "Search Results: " + HttpUtility.HtmlEncode(Request["Search"]);
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
                var rtttl = new RTTTL();
                var rtttlBytes = rtttl.ParseRtttl(result.Rtttl);
                Response.Clear();
                Response.Headers.Add("Content-disposition", "attachment;filename=" + result.Artist + " - " + result.Title + ".mid");
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(rtttlBytes);
                Response.End();
                return;
            }
            else
            {
                listTitle = "Top 10 Ringtones";
                results = SqlExec.GetTopTenTones();
                SearchCountDiv.Visible = false;
            }

            LabelTitle.Text = listTitle;
            PopulateResults(results);

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

        private void PopulateResults(List<Model.Tone> results)
        {
            if (results != null)
            {
                LabelSearchCount.Text = results.Count.ToString();

                var headerRow = new TableHeaderRow();
                headerRow.CssClass = "resultsTableHeaderRow";

                var headerCell = new TableHeaderCell();

                //headerCell = new TableHeaderCell();
                //headerCell.Text = "ToneId";
                //headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "Download";
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "Artist";
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "Title";
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "Downloads";
                headerRow.Cells.Add(headerCell);

                headerCell = new TableHeaderCell();
                headerCell.Text = "RTTTL Text";
                headerRow.Cells.Add(headerCell);

                ResultsTable.Rows.Add(headerRow);

                var row = new TableRow();
                var cell = new TableCell();
                int i = 0;
                foreach (var result in results)
                {
                    i++;

                    row = new TableRow();

                    if (i % 2 == 0)
                    {
                        row.CssClass = "active";
                    }

                    //cell = new TableCell();
                    //cell.Style.Add("text-align", "center");
                    //cell.Text = result.ToneId.ToString();
                    //row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style.Add("text-align", "center");
                    cell.Text = "<a href=\"?MIDI=" + result.ToneId.ToString() + "\">MIDI <span class='sr-only'> download of " + result.Artist + " - " + result.Title + " (" + result.ToneId.ToString() + ")</span>" + "</a>";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style.Add("text-align", "left");
                    cell.Text = result.Artist;
                    row.Cells.Add(cell);


                    cell = new TableCell();
                    cell.Style.Add("text-align", "left");
                    cell.Text = result.Title;
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style.Add("text-align", "center");
                    cell.Text = result.Counter.ToString();
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style.Add("text-align", "left");
                    cell.Style.Add("word-wrap", "break-word");
                    cell.Style.Add("max-width", "500px");
                    cell.Text = result.Rtttl;
                    row.Cells.Add(cell);

                    ResultsTable.Rows.Add(row);
                }

            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("?Search=" + SearchText.Value);
        }
    }
}