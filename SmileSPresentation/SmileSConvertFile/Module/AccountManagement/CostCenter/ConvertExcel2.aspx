<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ConvertExcel2.aspx.cs" Inherits="MasterTest.ConvertExcel2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="~/Bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="<%=Page.ResolveClientUrl("~/css/Covert1.css")%>" rel="stylesheet" />
    <script src="Bootstrap/jQuery/jquery-3.2.1.slim.min.js"></script>
    <script src="Bootstrap/Scripts/umd/popper.min.js"></script>
    <script src="Bootstrap/js/bootstrap.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">

        <div>
            <h1>File : จ่ายเงินสำรองสินไหม (BC)</h1>
        </div>
        <div class="browse">
            <asp:FileUpload ID="UploadFile" runat="server" /><br />
            <asp:Label ID="txtGuide" runat="server" Text="File Format : aaa.xlsx , aaa.xslm , aaa.xltx and aaa.xltm"></asp:Label><br />
        </div>

        <div class="import">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Image/retrivedata.png" OnClick="Import_Click" Height="90px" Width="316px" />
            <br />


            <asp:Label ID="txtColumn1" CssClass="item" runat="server" Text="Total Columns :"></asp:Label>&nbsp;
                <asp:Label ID="txtColumn2" runat="server" Text=""></asp:Label><br />

            <asp:Label ID="txtRow1" CssClass="item" runat="server" Text="Total Rows :"></asp:Label>&nbsp;
                <asp:Label ID="txtRow2" runat="server" Text=""></asp:Label><br />

            <asp:Label ID="txtStatus1" CssClass="item" runat="server" Text="Import Status : "></asp:Label>
            <asp:Label ID="txtStatus2" runat="server" Text="" ForeColor="Green"></asp:Label><br />
        </div>

        <div class="export">
            <asp:ImageButton ID="ExportFile" runat="server" ImageUrl="~/Image/exportexcel.png" OnClick="Export_Click" Height="98px" Width="316px" /><br />
            <asp:Label ID="txtExport" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
