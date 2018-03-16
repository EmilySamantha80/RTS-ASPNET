<%@ Page Title="Merwin's Ringtone Search" Language="C#" MasterPageFile="~/RTS.Master" AutoEventWireup="true" CodeBehind="ViewTone.aspx.cs" Inherits="RTS.ViewTone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-2"></div>
        <div class="col-sm-12 col-lg-8">
            <div class="card">
                <div class="card-header card-header-blue">
                    <div class="row">
                        <div class="col-sm-12">
                            <h2><span runat="server" id="ToneArtist"></span> - <span runat="server" id="ToneTitle"></span></h2>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row form-group">
                        <div class="col-md-3 col-xl-2 bold text-md-right">Categories:</div>
                        <div class="col-md-9 col-xl-10">
                            <div runat="server" id="ToneCategories" aria-label="Categories"></div>
                        </div>
                        <div class="col-md-3 col-xl-2 bold text-md-right">Downloads:</div>
                        <div class="col-md-9 col-xl-10">
                            <div runat="server" id="ToneDownloads" aria-label="Downloads"></div>
                        </div>
                        <div class="col-md-3 col-xl-2 bold text-md-right">Preview:</div>
                        <div class="col-md-9 col-xl-10">
                            <a runat="server" id="TonePreviewLink" aria-label="Preview">MIDI</a>
                        </div>
                        <div class="col-md-3 col-xl-2 bold text-md-right">RTTTL:</div>
                        <div class="col-md-9 col-xl-10" style="word-wrap:break-word;">
                            <div runat="server" id="ToneRtttl" aria-label="RTTTL"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
