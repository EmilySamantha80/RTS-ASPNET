<%@ Page Title="" Language="C#" MasterPageFile="~/RTS.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RTS.Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-sm-1 col-lg-2"></div>
    </div>
    <div class="row">
        <div class="col-sm-1 col-lg-2"></div>
        <div class="col-sm-10 col-lg-8">
            <div class="panel panel-primary" style="border:0;">
                <div class="panel-heading" style="border-top-left-radius:2px;border-top-right-radius:2px;border-bottom:0;padding-top:5px;padding-bottom:0">
                    <div class="row">
                        <div class="col-sm-6 resultsHeader resultsHeaderLeft">
                            <asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-sm-6 resultsHeader resultsHeaderRight" runat="server" id="SearchCountDiv">
                            <div>Found <asp:Label ID="LabelSearchCount" runat="server" Text=""></asp:Label> ringtones</div>
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="padding:0px;">
                    <asp:Repeater ID="ResultsRepeater" runat="server">
                        <HeaderTemplate>
                            <table class="table table-bordered table-responsive resultsTable">
                                <thead class="resultsTableHeaderRow">
                                    <tr>
                                        <th>Artist</th>
                                        <th>Title</th>
                                        <th style="text-align:center;">Downloads</th>
                                        <th style="text-align:center;">Preview</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "active" %>">
                                    <td><a href="Default.aspx?Search=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem,"Artist").ToString())%>"><%# DataBinder.Eval(Container.DataItem,"Artist") %></a></td>
                                    <td><a href="ViewTone.aspx?ToneId=<%# DataBinder.Eval(Container.DataItem,"ToneId") %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></td>
                                    <td style="text-align:center;"><%# DataBinder.Eval(Container.DataItem,"Counter").ToString() %></td>
                                    <td style="text-align:center;"><a href="?MIDI=<%# DataBinder.Eval(Container.DataItem,"ToneId") %>">MIDI<span class='sr-only'> download of <%# DataBinder.Eval(Container.DataItem,"Artist") %> - <%# DataBinder.Eval(Container.DataItem,"Title") %> ( <%# DataBinder.Eval(Container.DataItem,"ToneId") %> )</span></a></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
