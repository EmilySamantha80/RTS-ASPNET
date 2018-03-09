<%@ Page Title="Merwin's Ringtone Search" Language="C#" MasterPageFile="~/RTS.Master" AutoEventWireup="true" CodeBehind="ViewTone.aspx.cs" Inherits="RTS.ViewTone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-sm-1 col-lg-2"></div>
        <div class="col-sm-10 col-lg-8">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-sm-12">
                            <h2 class="o-application--panel-heading o-application--white" style="font-size:2em;"><span runat="server" id="ToneArtist"></span> - <span runat="server" id="ToneTitle"></span></h2>
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="padding-left:0px;padding-right:0px;padding-bottom:0px;padding-top:5px;">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group form-tight o-application--form-group-slim">
                                    <div class="control-label col-sm-3 col-md-3 col-lg-3">Categories</div>
                                    <div class="col-sm-3 o-application--form-span-slim">
                                        <div runat="server" id="ToneCategories" aria-label="Categories"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group form-tight o-application--form-group-slim">
                                    <div class="control-label col-sm-3 col-md-3 col-lg-3">Downloads</div>
                                    <div class="col-sm-3 o-application--form-span-slim">
                                        <div runat="server" id="ToneDownloads" aria-label="Downloads"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group form-tight o-application--form-group-slim">
                                    <div class="control-label col-sm-3 col-md-3 col-lg-3">Preview</div>
                                    <div class="col-sm-3 o-application--form-span-slim">
                                        <a runat="server" id="TonePreviewLink" aria-label="Preview">MIDI</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group form-tight o-application--form-group-slim">
                                    <div class="control-label col-sm-3 col-md-3 col-lg-3">RTTTL</div>
                                    <div class="col-sm-9 o-application--form-span-slim" style="word-wrap:break-word;">
                                        <div runat="server" id="ToneRtttl" aria-label="RTTTL"></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
