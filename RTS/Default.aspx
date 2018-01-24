<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RTS.Default" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en-us">
<head runat="server">
    <title>Merwin's Ringtone Search</title>
    <meta name="description" content="Your source for old-school nokia RTTTL ringtones" />
    <meta name="keywords" content="ringtone,nokia,rtttl" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" href="favicon.ico" type="image/x-icon" /> 
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" /> 
    <script src="https://code.jquery.com/jquery-1.12.3.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <link href="Styles/application.css?v=<%= DateTime.Now.ToString("s").Replace(":","").Replace("-","") %>" rel="stylesheet" type="text/css" />
    <link href="Styles/rts.css?v=<%= DateTime.Now.ToString("s").Replace(":","").Replace("-","") %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <a href="#<%= SearchText.ClientID %>" class="sr-only">Skip to main content</a>
    <form id="form1" runat="server">
        <div class="container t-application">
            <div class="row" style="margin-bottom:15px;">
                <div class="col-sm-12" style="text-align:center">
                    <a href="Default.aspx"><img src="Images/logo.png" width="696" height="75" class="img-responsive" style="display:block;margin:auto;" alt="Merwin's Ringtone Search logo" /></a>
                        <h1 style="font-size:1.5em;margin-top:0px;">Your source for old-school Nokia RTTTL ringtones</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"></div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-12 col-sm-8 col-md-9" style="padding-left:10px;padding-right:0px;">
                                    <label class="sr-only" for="<%=SearchText.ClientID %>">Search ringtones</label>
                                    <input runat="server" id="SearchText" class="form-control" style="width:100%;" />
                                </div>
                                <div class="col-xs-4 col-sm-4 col-md-3" style="padding-left:10px;padding-right:10px;">
                                    <asp:Button ID="SearchButton" runat="server" Text="Search Tones" style="width:100%" CssClass="btn btn-primary" OnClick="SearchButton_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body" style="padding-top:5px;">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div style="margin-top:0px;float:left;" class="categoriesHeader">Ringtone Categories</div>
                                </div>
                                <div class="col-xs-6">
                                    <div style="margin-top:0px;float:right;" class="categoriesHeader">Total Ringtones: <asp:Label ID="TotalRingtones" runat="server" Text=""></asp:Label></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div runat="server" id="Categories"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row resultsHeader">
                <div class="col-xs-6" style="padding:7px;padding-bottom:0px;">
                    <asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-xs-6" runat="server" id="SearchCountDiv" style="padding:7px;padding-bottom:0px;">
                    <div style="text-align:right;">Found <asp:Label ID="LabelSearchCount" runat="server" Text=""></asp:Label> ringtones</div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <asp:Repeater ID="ResultsRepeater" runat="server">
                        <HeaderTemplate>
                            <table class="table table-bordered table-responsive resultsTable">
                                <thead class="resultsTableHeaderRow">
                                    <th style="text-align:center;">Download</th>
                                    <th>Artist</th>
                                    <th>Title</th>
                                    <th style="text-align:center;">Downloads</th>
                                    <th>RTTTL Text</th>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "active" %>">
                                    <td style="text-align:center;"><a href="?MIDI=<%# DataBinder.Eval(Container.DataItem,"ToneId") %>">MIDI<span class='sr-only'> download of <%# DataBinder.Eval(Container.DataItem,"Artist") %> - <%# DataBinder.Eval(Container.DataItem,"Title") %> ( <%# DataBinder.Eval(Container.DataItem,"ToneId") %> )</span></a></td>
                                    <td><%# DataBinder.Eval(Container.DataItem,"Artist") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem,"Title") %></td>
                                    <td style="text-align:center;"><%# DataBinder.Eval(Container.DataItem,"Counter").ToString() %></td>
                                    <td style="word-wrap:break-word;max-width:500px;"><%# DataBinder.Eval(Container.DataItem,"Rtttl") %></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="footer" style="margin-top:15px;">
                <div class="row">
                    <div class="col-md-6">
                        <span runat="server" id="FooterProductName"></span>
                    </div>
                    <div class="col-md-6 pull-md-right pull-lg-right">
                        Counting since <span runat="server" id="FooterCountingSince"></span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <span runat="server" id="FooterCopyright"></span>&nbsp;<a href="mailto:emilysamantha80@gmail.com">Emily Heiner</a>
                    </div>
                    <div class="col-md-6 pull-md-right pull-lg-right">
                        <div>
                            Total Page Views: <asp:Label ID="TotalPageViews" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div>
                            Page generated in: <asp:Label ID="RequestTime" runat="server" Text=""></asp:Label> ms
                        </div>
                    </div>
                    <div class="col-md-6 pull-md-right pull-lg-right">
                        <div>
                            Total Unique Hits: <asp:Label ID="TotalUniqueHits" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <br />
</body>
</html>

