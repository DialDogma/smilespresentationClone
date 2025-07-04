<%@ Page Title="หน้าแรก" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SmileMotorV1.Default" EnableEventValidation="false" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucMonitorForBranch.ascx" TagPrefix="uc1" TagName="ucMonitorForBranch" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucMonitorForMotor.ascx" TagPrefix="uc1" TagName="ucMonitorForMotor" %>
<%@ Register Src="CommonUserControls/ucProgressbar.ascx" TagName="ucProgressbar" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            $(window).scroll(function () {
                if ($(this).scrollTop() >= 50) {        // If page is scrolled more than 50px
                    $('#myBtn').fadeIn(200);    // Fade in the arrow
                } else {
                    $('#myBtn').fadeOut(200);   // Else fade out the arrow
                }
            });

            $('#myBtn').click(function () {      // When arrow is clicked
                $('body,html').animate({
                    scrollTop: 0                       // Scroll to top of body
                }, 500);
            });

            //-------------------TEST------------------------//

            //-------------------END TEST--------------------//
        });

        //function??????
        var submit = 0;
        function CheckDouble() {
            if (++submit > 1) {
                //alert('This sometimes takes a few seconds - please be patient.');
                return false;
            }
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:ucProgressbar ID="ucProgressbar1" runat="server" />
            <div class="container" id="dashboardBranch">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading input-md">
                            <h4>Dashboard Branch
                            </h4>
                        </div>
                        <div class="panel-body" id="panelBranch">
                            <uc1:ucMonitorForBranch runat="server" ID="ucMonitorForBranch" />
                        </div>
                        <div class="panel-footer"></div>
                    </div>
                </div>
            </div>
            <div class="container" runat="server" id="dashboardMotor" visible="false">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading input-md">
                            <h4>Dashboard Motor</h4>
                        </div>
                        <div class="panel-body" id="panelMotor">
                            <uc1:ucMonitorForMotor runat="server" ID="ucMonitorForMotor1" />
                        </div>
                        <div class="panel-footer"></div>
                    </div>
                </div>
            </div>
            <input type="button" value="Top" id="myBtn" class="myBtnTop" title="Go to top" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>