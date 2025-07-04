<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ConvertExcel5.aspx.cs" Inherits="MasterTest.ConvertExcel5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">

        <div>
            <h1>File : บัญชีแยกประเภท (GL)</h1>
        </div>

        <div class="browse">
                <asp:FileUpload ID="UploadFile" runat="server"/><br />
            <asp:Label ID="txtGuide" runat="server" Text="File Format : GLLEDGER-xxxxxx.txt"></asp:Label><br />
        </div>


        <div class="import">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Image/retrivedata.png" OnClick="Import_Click" Height="90px" Width="316px" />
            <br />

            <%--<asp:Label ID="txtColumn1" CssClass="item" runat="server" Text="Total Columns :"></asp:Label>&nbsp;
                <asp:Label ID="txtColumn2" runat="server" Text=""></asp:Label><br />

                <asp:Label ID="txtRow1" CssClass="item" runat="server" Text="Total Rows :"></asp:Label>&nbsp;
                <asp:Label ID="txtRow2" runat="server" Text=""></asp:Label><br />--%>

            <asp:Label ID="txtStatus1" CssClass="item" runat="server" Text="Import Status : "></asp:Label>
            <asp:Label ID="txtStatus2" runat="server" Text="" ForeColor="Green"></asp:Label><br />
        </div>

        <div class="export">
            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Image/exportexcel.png" OnClick="Export_Click" Height="98px" Width="316px" /><br />
            <asp:Label ID="txtExport" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
