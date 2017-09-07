<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RTS.Default" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Merwin's Ringtone Search</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-1.12.3.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-select/1.12.2/css/bootstrap-select.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-select/1.12.2/js/bootstrap-select.min.js"></script>
    <link href="Styles/application.css?v=<%= DateTime.Now.ToString("s").Replace(":","").Replace("-","") %>" rel="stylesheet" type="text/css" />
    <link href="Styles/rts.css?v=<%= DateTime.Now.ToString("s").Replace(":","").Replace("-","") %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <a href="#<%= SearchText.ClientID %>" class="sr-only">Skip to main content</a>
    <form id="form1" runat="server">
        <div class="container t-application">
            <div class="row" style="margin-bottom:15px;">
                <div class="col-sm-12" style="text-align:center">
                    <center><h1 style="margin-bottom:0px;"><a href="Default.aspx"><img src="Images/logo.png" class="img-responsive" /></a></h1>
                        <div style="font-size:1.5em">Your source for old-school Nokia RTTTL ringtones</div>
                    </center>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"></div>
                <div class="col-sm-8">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-sm-12 col-md-9">
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
                    <div class="panel panel-info" style="margin-bottom:0px;">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="categoriesHeader"><asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label></div>
                                </div>
                                <div class="col-xs-6">
                                    <div style="text-align:right;" class="categoriesHeader">Found <asp:Label ID="LabelSearchCount" runat="server" Text=""></asp:Label> ringtones</div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body" style="padding:0px">
                            <asp:Table ID="ResultsTable" runat="server" style="margin:0px;" CssClass="table table-bordered table-responsive"></asp:Table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="margin-top:15px;">
                <div class="col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div style="float:left">
                                        Merwin's Ringtone Search (c)2000-2017 <a href="mailto:emilysamantha80@gmail.com">Emily Heiner</a>
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div style="float:right">
                                        Counting since December 4, 2001
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="footerText">
                                    <div class="col-sm-12">
                                        <div>
                                            Total Page Views: <asp:Label ID="TotalPageViews" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="footerText">
                                    <div class="col-sm-12">
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
        </div>
    </form>
</body>
</html>

