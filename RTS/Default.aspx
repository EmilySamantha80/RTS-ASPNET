<%@ Page Title="" Language="C#" MasterPageFile="~/RTS.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RTS.Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <th>Artist</th>
                            <th>Title</th>
                            <th style="text-align:center;">Downloads</th>
                            <th style="text-align:center;">Preview</th>
                            <th>RTTTL Text</th>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "active" %>">
                            <td><%# DataBinder.Eval(Container.DataItem,"Artist") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem,"Title") %></td>
                            <td style="text-align:center;"><%# DataBinder.Eval(Container.DataItem,"Counter").ToString() %></td>
                            <td style="text-align:center;"><a href="?MIDI=<%# DataBinder.Eval(Container.DataItem,"ToneId") %>">MIDI<span class='sr-only'> download of <%# DataBinder.Eval(Container.DataItem,"Artist") %> - <%# DataBinder.Eval(Container.DataItem,"Title") %> ( <%# DataBinder.Eval(Container.DataItem,"ToneId") %> )</span></a></td>
                            <td style="word-wrap:break-word;max-width:500px;"><%# DataBinder.Eval(Container.DataItem,"Rtttl") %></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>

</asp:Content>
