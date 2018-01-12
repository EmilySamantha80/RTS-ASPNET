<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RTS.Default" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en-us">
<head runat="server">
    <title></title>
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
                    <center><a href="Default.aspx"><img src="Images/logo.png" class="img-responsive" alt="Merwin's Ringtone Search logo" /></a>
                        <h1 style="font-size:1.5em;margin-top:0px;">Your source for old-school Nokia RTTTL ringtones</h1>
                    </center>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"></div>
                <div class="col-sm-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-sm-12 col-md-9">
                                    <label class="sr-only" for="<%=SearchText.ClientID %>">Search ringtones</label>
                                    <input runat="server" id="SearchText" class="form-control" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button ID="SearchButton" runat="server" Text="Search Tones" CssClass="btn btn-primary" OnClick="SearchButton_Click"></asp:Button>
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
            <div class="row">
                <div class="col-sm-12">
                    <div class="panel panel-primary" style="margin-bottom:0px;border:none">
                        <div class="panel-heading" style="border:none;padding:7px;padding-bottom:0px;">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="resultsHeader"><asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label></div>
                                </div>
                                <div class="col-xs-6" runat="server" id="SearchCountDiv">
                                    <div style="text-align:right;" class="resultsHeader">Found <asp:Label ID="LabelSearchCount" runat="server" Text=""></asp:Label> ringtones</div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body" style="padding:0px">
                            <asp:Table ID="ResultsTable" runat="server" style="margin:0px;border:none;" CssClass="table table-bordered table-responsive resultsTable"></asp:Table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="margin-top:15px;">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
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
                </div>
            </div>
        </div>
    </form>
</body>
</html>

