<%@ Page Title="" Language="C#" MasterPageFile="~/RTS.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RTS.Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-sm-1 col-lg-2"></div>
    </div>
    <div class="row">
        <div class="col-lg-2"></div>
        <div class="col-sm-12 col-lg-8">
            <div class="card">
                <div class="card-header card-header-blue resultsPanelHeading">
                    <div class="row">
                        <div class="col-sm-6 resultsHeader">
                            <asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-sm-6 text-sm-right resultsHeader" runat="server" id="SearchCountDiv">
                            <div><asp:Label ID="LabelSearchCount" runat="server" Text=""></asp:Label></div>
                        </div>
                    </div>
                </div>
                <div class="card-body" style="padding:0px;">
                    <asp:Repeater ID="ResultsRepeater" runat="server">
                        <HeaderTemplate>
                            <table class="table table-sm table-striped">
                                <thead>
                                    <tr class="thead-dark" style="border:0;">
                                        <th style="border-color:#265A87;">Artist</th>
                                        <th style="border-color:#265A87;">Title</th>
                                        <th style="border-color:#265A87;text-align:center;">Views</th>
                                        <th style="border-color:#265A87;text-align:center;">Preview</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td><a href="Default.aspx?Search=<%# HttpUtility.UrlEncode(DataBinder.Eval(Container.DataItem,"Artist").ToString())%>"><%# DataBinder.Eval(Container.DataItem,"Artist") %></a></td>
                                    <td><a href="ViewTone.aspx?ToneId=<%# DataBinder.Eval(Container.DataItem,"ToneId") %>"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></td>
                                    <td style="text-align:center;"><%# string.Format("{0:n0}", DataBinder.Eval(Container.DataItem,"Counter")) %></td>
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
