<%@ Page Title="Merwin's Ringtone Search" Language="C#" MasterPageFile="~/RTS.Master" AutoEventWireup="true" CodeBehind="ConvertRtttlToMidi.aspx.cs" Inherits="RTS.ConvertRtttlToMidi" %>
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
                            <h2 class="o-application--panel-heading o-application--white" style="font-size:2em;">Convert your own RTTTL to MIDI</h2>
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="">
                    <div class="row">
                        <div class="col-sm-12">
                            <label for="<%=RtttlText.ClientID %>">RTTTL Text</label><br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <textarea id="RtttlText" class="form-control" style="height:100px;width:100%;" runat="server"></textarea>
                        </div>
                    </div>
                    <div class="row" style="margin-top:5px;">
                        <div class="col-sm-12">
                            <a href="#" onclick="post_rtttl();return false;" class="btn btn-primary">Convert to MIDI</a>
                        </div>
                    </div>
                    <%--<div class="row" style="margin-top:10px;">
                        <div class="col-sm-12">
                            <span style="font-weight:600">Note:</span> If you hear 3 rising tones instead of your ringtone, it means that your RTTTL text has errors in it.
                        </div>
                    </div>--%>
                    <div class="row" style="margin-top:10px;" runat="server" id="ErrorDiv" visible="false">
                        <div class="col-sm-12">
                            <span style="font-weight:600" class="o-application--red" runat="server" id="ErrorMessage" role="alert" aria-live="assertive" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function post_rtttl() {
            var rtttl = document.getElementById('<%= RtttlText.ClientID %>').value;
            //alert(rtttl);
            var params = { rtttl: rtttl };
            post("ConvertRtttlToMidi.aspx", params, "post");
        }

    </script>
</asp:Content>
