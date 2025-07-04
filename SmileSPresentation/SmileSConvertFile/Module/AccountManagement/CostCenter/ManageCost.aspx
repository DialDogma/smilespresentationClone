<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageCost.aspx.cs" Inherits="MasterTest.ManageCost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="~/Bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="<%=Page.ResolveClientUrl("~/css/Covert1.css")%>" rel="stylesheet" />
    <script src="Bootstrap/jQuery/jquery-3.2.1.slim.min.js"></script>
    <script src="Bootstrap/Scripts/umd/popper.min.js"></script>
    <script src="Bootstrap/js/bootstrap.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="head">
            <div class="form-group">
                <asp:ScriptManager ID="scp" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="upd" runat="server">
                    <ContentTemplate>

                        <label id="lbSearch" runat="server">Detail :</label>
                        <input type="text" id="txtSearch" runat="server" />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

                        <!-- Button trigger modal -->
                        <%--<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">
                        Insert
                    </button>--%>

                        <asp:Button ID="insertbtn" CssClass="btn btn-primary" runat="server" Text="Insert" OnClick="insertbtn_Click" />

                        <!-- Modal -->
                        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Insert</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-group">
                                            <asp:Label ID="lbCode1" runat="server" Text="*Code : "></asp:Label>
                                            <asp:TextBox ID="txtCode1" runat="server"></asp:TextBox><br />
                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="lbDetail1" runat="server" Text="*Detail : "></asp:Label>
                                            <asp:TextBox ID="txtDetail" TextMode="MultiLine" Rows="2" Columns="40" runat="server"></asp:TextBox><br />
                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="lbDeptType1" runat="server" Text="*DeptType : "></asp:Label>
                                            <asp:TextBox ID="txtDept1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnClose" CssClass="btn btn-secondary" data-dismiss="modal" runat="server" Text="Close" />
                                        <asp:Button ID="btnInsert" CssClass="btn btn-success" runat="server" Text="New Add" OnClick="Button_Insert" />

                                    </div>
                                </div>
                            </div>
                        </div>


                        <div>
                            <table class="table" id="costtable">
                                <tr>
                                    <td>
                                        <asp:GridView ID="dataGridView" CssClass="gridview" runat="server" HorizontalAlign="Center" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="CostCenterAccountCode,CostCenterAccountDetail,DeptType" OnRowDataBound="dataGridView_RowDataBound" OnSelectedIndexChanged="dataGridView_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dataGridView_PageIndexChanging" PageSize="8" AutoGenerateColumns="False">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" />
                                                <asp:BoundField HeaderText="Cost Center" DataField="CostCenterAccountCode"/>
                                                <asp:BoundField HeaderText="Cost Center Name" DataField="CostCenterAccountDetail"/>
                                                <asp:BoundField HeaderText="Cost Type" DataField="DeptType" />
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                        </asp:GridView>

                                        <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                            <div class="modal-dialog" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h5 class="modal-title" id="exampleModalLabel2">Edit</h5>
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                            <span aria-hidden="true">&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="form-group">
                                                            <asp:Label ID="lbCodeEdit1" runat="server" Text="Cost Center : "></asp:Label>
                                                            <asp:TextBox ID="txtCodeEdit1" runat="server"></asp:TextBox><br />
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Label ID="lbDetailEdit1" runat="server" Text="Cost Center Name : "></asp:Label>
                                                            <asp:TextBox ID="txtDetailEdit1" TextMode="MultiLine" Rows="2" Columns="40" runat="server"></asp:TextBox><br />
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Label ID="lbDeptTypeEdit1" runat="server" Text="Cost Type : "></asp:Label>
                                                            <asp:TextBox ID="txtDeptTypeEdit1" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button ID="btnCloseEdit" CssClass="btn btn-secondary" data-dismiss="modal" runat="server" Text="Close" />
                                                        <asp:Button ID="btnEdit" CssClass="btn btn-success" runat="server" Text="Edit" OnClick="btnEdit_Click" />
                                                        <asp:Button ID="btnDelete" CssClass="btn btn-danger" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                    </td>

                                </tr>
                            </table>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>
