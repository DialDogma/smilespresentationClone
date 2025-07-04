<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SmileSConvertFileV2.index" EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--@*Bootstrap CSS*@--%>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Style/MediaStyle.css" rel="stylesheet" />

    <style>
        #gdvData > tbody > tr > td > table > tbody > tr > td > a {
            padding-left: 3px;
        }

        #gdvData2 > tbody > tr > td > table > tbody > tr > td > a {
            padding-left: 3px;
        }

        .error {
            color: red;
        }

        #loader {
            bottom: 0;
            height: 175px;
            left: 0;
            margin: auto;
            position: absolute;
            right: 0;
            /*top: 0;*/
            width: 175px;
        }

            #loader .dot {
                bottom: 0;
                height: 100%;
                left: 0;
                margin: auto;
                position: absolute;
                right: 0;
                top: 0;
                width: 87.5px;
            }

                #loader .dot::before {
                    border-radius: 100%;
                    content: "";
                    height: 87.5px;
                    left: 0;
                    position: absolute;
                    right: 0;
                    top: 0;
                    transform: scale(0);
                    width: 87.5px;
                }

                #loader .dot:nth-child(7n+1) {
                    transform: rotate(45deg);
                }

                    #loader .dot:nth-child(7n+1)::before {
                        animation: 0.8s linear 0.1s normal none infinite running load;
                        background: #00ff80 none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+2) {
                    transform: rotate(90deg);
                }

                    #loader .dot:nth-child(7n+2)::before {
                        animation: 0.8s linear 0.2s normal none infinite running load;
                        background: #00ffea none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+3) {
                    transform: rotate(135deg);
                }

                    #loader .dot:nth-child(7n+3)::before {
                        animation: 0.8s linear 0.3s normal none infinite running load;
                        background: #00aaff none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+4) {
                    transform: rotate(180deg);
                }

                    #loader .dot:nth-child(7n+4)::before {
                        animation: 0.8s linear 0.4s normal none infinite running load;
                        background: #0040ff none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+5) {
                    transform: rotate(225deg);
                }

                    #loader .dot:nth-child(7n+5)::before {
                        animation: 0.8s linear 0.5s normal none infinite running load;
                        background: #2a00ff none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+6) {
                    transform: rotate(270deg);
                }

                    #loader .dot:nth-child(7n+6)::before {
                        animation: 0.8s linear 0.6s normal none infinite running load;
                        background: #9500ff none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+7) {
                    transform: rotate(315deg);
                }

                    #loader .dot:nth-child(7n+7)::before {
                        animation: 0.8s linear 0.7s normal none infinite running load;
                        background: magenta none repeat scroll 0 0;
                    }

                #loader .dot:nth-child(7n+8) {
                    transform: rotate(360deg);
                }

                    #loader .dot:nth-child(7n+8)::before {
                        animation: 0.8s linear 0.8s normal none infinite running load;
                        background: #ff0095 none repeat scroll 0 0;
                    }

        /*#loader .loading {
                background-image: url("../images/loading.gif");
                background-position: 50% 50%;
                background-repeat: no-repeat;
                bottom: -40px;
                height: 20px;
                left: 0;
                position: absolute;
                right: 0;
                width: 180px;
            }*/

        @keyframes load {
            100% {
                opacity: 0;
                transform: scale(1);
            }
        }

        @keyframes load {
            100% {
                opacity: 0;
                transform: scale(1);
            }
        }
    </style>
</head>
<body>
    <input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />
    <form id="form1" runat="server" class="form-horizontal">
        <div class="container" style="margin-top: 10px;">
            <div>
                <fieldset>
                    <legend>ค้นหา</legend>
                    <div class="form-group">

                        <label class="col-md-1 control-label" for="selectbasic">ชนิดไฟล์*</label>
                        <div class="col-md-3">
                            <asp:DropDownList ID="lstFile" runat="server" CssClass="form-control insert">
                                <asp:ListItem>RA</asp:ListItem>
                                <asp:ListItem>BC</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <label class="col-md-1 control-label" for="selectbasic">บริษัท*</label>
                        <div class="col-md-3">
                            <asp:DropDownList ID="lstCompany" runat="server" CssClass="form-control insert">
                                <asp:ListItem Value="AI.">AI</asp:ListItem>
                                <asp:ListItem Value="BU.">BU</asp:ListItem>
                                <asp:ListItem Value="SB.">SB</asp:ListItem>
                                <asp:ListItem Value="SE.">SE</asp:ListItem>
                                <asp:ListItem Value="UL.">UL</asp:ListItem>
                                <asp:ListItem Value="VR.">VR</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <label id="lblDate" class="col-md-1 control-label" for="selectbasic">วันที่**</label>
                        <div class="col-md-3">
                            <asp:TextBox ID="tbDate" runat="server" placeholder="Ex: 1/1/60 , 12/12/60" class="form-control input-md insert" MaxLength="8" required="required" checkDate="checkDate"></asp:TextBox>
                        </div>

                        <div style="text-align: center;">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="Button1_Click" CssClass="btn btn-default" Style="margin-top: 18px;" align="center" Enabled="true" />
                            <button type="button" id="btnAddErrorCode" class="btn btn-default" style="margin-top: 18px" runat="server">เพิ่มที่ไม่มีรหัส</button>
                            <button type="button" id="btnAddCode" class="btn btn-default" style="margin-top: 18px" runat="server">เพิ่มรหัส</button>
                        </div>
                    </div>
                </fieldset>
                <fieldset id="addcodefield" style="display: none">
                    <legend>เพิ่มรหัส</legend>
                    <div>
                        <table id="addTable" class="table table-bordered">
                            <tr>
                                <th>ชื่อรายการ</th>
                                <th>กรอกรหัส</th>

                            </tr>
                            <tr>
                                <td>
                                    <input type="text" id="provincetxt" name="provincetxt" class="form-control" required="required" runat="server" />
                                </td>
                                <td>
                                    <input type="text" id="salemancodetxt" name="salemancodetxt" class="form-control" required="required" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <%--<button id="btnSavecode" runat="server" class="btn btn-default" onserverclick="btnSavecode_ServerClick">บันทึก</button>--%>
                        <button id="btnSaveAddCodes" runat="server" class="btn btn-default">บันทึก</button>
                        <asp:Button ID="btnCancelSavecodes" runat="server" Text="ยกเลิก" CssClass="btn btn-default" />
                    </div>
                </fieldset>
                <fieldset id="errorcodefield" style="display: none">
                    <legend>แสดงรายการที่ไม่มีรหัส</legend>
                    <div>
                        <table id="errorTable" class="table table-bordered">
                            <tr>
                                <th>ชื่อรายการที่ไม่มีรหัส</th>
                                <th>กรอกรหัส</th>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <%--<button id="btnSavenocode" runat="server" class="btn btn-default" onserverclick="btnSave_ServerClick">บันทึก</button>--%>
                        <button id="btnSaveErrorCodes" runat="server" class="btn btn-default">บันทึก</button>
                        <%--<asp:Button ID="btnSaveErrorcodes" runat="server" Text="บันทึก" CssClass="btn btn-default" OnClientClick="SetCode();return false;" OnClick="btnSaveErrorcodes_Click"/>--%>
                        <asp:Button ID="btnCancelSaveErrorcodes" runat="server" Text="ยกเลิก" CssClass="btn btn-default" />
                    </div>
                </fieldset>
                <fieldset>
                    <legend>แสดงรายการ</legend>
                    <div>
                        <asp:Button ID="btnDownload" runat="server" Text="ดาวน์โหลด สาขา" OnClick="btnDownload_Click" CssClass="btn btn-default" Enabled="false" />
                        <asp:GridView ID="gdvData" runat="server" AllowPaging="True" OnPageIndexChanging="gdvData_PageIndexChanging" CssClass="table"></asp:GridView>
                        <div id="loader" runat="server">
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <div class="dot"></div>
                            <%--<div class="loading"></div>--%>
                        </div>
                    </div>
                    <div>
                        <asp:Button ID="btnDownload2" runat="server" Text="ดาวน์โหลด โรงพยาบาล" OnClick="btnDownload2_Click" CssClass="btn btn-default" Enabled="false" />
                        <asp:GridView ID="gdvData2" runat="server" AllowPaging="True" OnPageIndexChanging="gdvData2_PageIndexChanging" CssClass="table"></asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">ยืนยันการบันทึก</h4>
                    </div>
                    <div class="modal-body">
                        <h3 class="modal-title">ต้องการบันทึกใช่หรือไม่</h3>
                    </div>
                    <div class="modal-footer">
                        <%--<span class="label label-danger">*หลังจาก บันทึก แล้วกรุณาทำรายการใหม่อีกครั้ง</span>--%>
                        <button type="button" id="btnSaveErrorcodesModal" class="btn btn-default" onserverclick="btnSaveErrorcodes_Click" runat="server">บันทึก</button>
                        <button type="button" id="btnCancelModalErrorModal" class="btn btn-default" data-dismiss="modal">ยกเลิก</button>
                        <%--<asp:Button ID="btnSave2" runat="server" Text="บันทึก"  CssClass="btn btn-default" OnClientClick="CallMethod(event)"/>--%>
                    </div>
                </div>

            </div>
        </div>

         <!-- Modal -->
        <div class="modal fade" id="myModal2" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">ยืนยันการบันทึก</h4>
                    </div>
                    <div class="modal-body">
                        <h3 class="modal-title">ต้องการบันทึกใช่หรือไม่</h3>
                    </div>
                    <div class="modal-footer">
                        <%--<span class="label label-danger">*หลังจาก บันทึก แล้วกรุณาทำรายการใหม่อีกครั้ง</span>--%>
                        <button type="button" id="btnSaveAddcodesModal" class="btn btn-default" onserverclick="btnSaveAddcodes_ServerClick" runat="server">บันทึก</button>
                        <button type="button" id="btnCancelModalAddModal" class="btn btn-default" data-dismiss="modal">ยกเลิก</button>
                        <%--<asp:Button ID="btnSave2" runat="server" Text="บันทึก"  CssClass="btn btn-default" OnClientClick="CallMethod(event)"/>--%>
                    </div>
                </div>

            </div>
        </div>



        <input type="hidden" id="errorCode" name="errorCode" value="" runat="server" />
        <input type="hidden" id="saveCode" name="saveCode" value="" runat="server" />

    </form>



    <!-- jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Bootstrap JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <!-- jquery-validate -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js"></script>

    <script>

        var errorCodeVal = $('#errorCode').val();

        $(function () {

            $('#loader').hide();

            $("form").validate();

            /////////////
            var isPostBackObject = document.getElementById('isPostBack');

            if (isPostBackObject != null) {
                var value = document.getElementById("lstFile").options[document.getElementById("lstFile").selectedIndex].value;
                if (value == "RA") {
                    $('#lstCompany').attr('disabled', false);
                } else {
                    $('#lstCompany').attr('disabled', true);
                }

                $('#btnSearch').attr('disabled', false);

                $('#btnAddErrorCode').css('background-color', '#f44336');
                $('#btnAddErrorCode').attr('disabled', false);

                $('#loader').hide();
            }
            ///////////////////

            $('#btnSearch').click(function (e) {

                $('#addcodefield').css('display', 'none');
                $('#errorcodefield').css('display', 'none');


                if ($('#tbDate').val() != "") {
                    $('#loader').show();
                }

                $('#gdvData').hide();
                $('#gdvData2').hide();
                $('#btnDownload').attr('disabled', true);
                $('#btnDownload2').attr('disabled', true);
            });

            $('#btnAddCode').click(function (e) {

                $('#addcodefield').css('display', '');
                $('#errorcodefield').css('display', 'none');
                $('#lstFile').attr('disabled', true);
                $('#lstCompany').attr('disabled', true);
                $('#tbDate').attr('disabled', true);
            });

            $('#btnAddErrorCode').click(function (e) {
                
                LoadFieldErrorCode();
            });

            $('#btnSaveAddCodes').click(function (e) {
                e.preventDefault();

                if ($("form").valid()) {
                    $('#myModal2').modal();
                }            

            });

            $('#btnCancelSavecodes').click(function (e) {
                e.preventDefault();
                $('#addcodefield').css('display', 'none');
                $('#lstFile').attr('disabled', false);
                $('#lstCompany').attr('disabled', false);
                $('#tbDate').attr('disabled', false);
            });


            $('#btnSaveErrorCodes').click(function (e) {

                e.preventDefault();
                //var countBlank = 0;

                //var items = document.getElementsByClassName('code');

                //for (var i = 0; i < items.length; i++) {
                //    if ($.trim(items[i].value) == '') {
                //        countBlank++;
                //    }
                //}

                //if (countBlank == 0) {
                //    SetCode();
                //    $('#myModal').modal();
                //}

                if ($("form").valid()) {
                    $('#myModal').modal();
                } 
            });

            $('#btnCancelSaveErrorcodes').click(function (e) {
                e.preventDefault();
                $('#errorcodefield').css('display', 'none');
                $('#lstFile').attr('disabled', false);
                $('#lstCompany').attr('disabled', false);
                $('#tbDate').attr('disabled', false);
            });



            $('#lstFile').change(function (e) {

                var value = document.getElementById("lstFile").options[document.getElementById("lstFile").selectedIndex].value;

                if (value == "RA") {
                    $('#lstCompany').attr('disabled', false);
                } else {
                    $('#lstCompany').attr('disabled', true);
                }
            });

            jQuery.extend(jQuery.validator.messages, {
                required: "กรุณากรอกข้อมูลช่องนี้",
                remote: "Please fix this field.",
                email: "กรุณากรอก Email ให้ถูกต้อง",
                url: "Please enter a valid URL.",
                date: "Ple55555ase enter a valid date.",
                dateISO: "Please enter a valid date (ISO).",
                number: "Please enter a valid number.",
                digits: "Please enter only digits.",
                creditcard: "Please enter a valid credit card number.",
                equalTo: "Please enter the same value again.",
                accept: "Please enter a value with a valid extension.",
                maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
                minlength: jQuery.validator.format("Please enter at least {0} characters."),
                rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
                range: jQuery.validator.format("Please enter a value between {0} and {1}."),
                max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
                min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
            });

            jQuery.validator.addMethod("checkDate", function (value) {

                var chkValue = value.split('/');

                if (chkValue.length == 3) {

                    var chkMonth = chkValue[1];
                    var chkDate = chkValue[0];

                    if (chkMonth[0] == "0" || chkDate[0] == "0" || chkMonth[0] == " " || chkDate[0] == " ") {
                        $('#btnSearch').attr('disabled', true);
                        return false;
                    } else {
                        if ((chkValue[0].length == 2 && chkValue[1].length == 2 && chkValue[2].length == 2) || (chkValue[0].length == 1 && chkValue[1].length == 1 && chkValue[2].length == 2) ||
                            (chkValue[0].length == 1 && chkValue[1].length == 2 && chkValue[2].length == 2) || (chkValue[0].length == 2 && chkValue[1].length == 1 && chkValue[2].length == 2)) {

                            var chkMonth = parseInt(chkValue[1]);
                            var chkDate = parseInt(chkValue[0]);

                            switch (chkMonth) {
                                case 1:
                                case 3:
                                case 5:
                                case 7:
                                case 8:
                                case 10:
                                case 12:
                                    if (chkDate > 0 && chkDate < 32) {
                                        $('#btnSearch').attr('disabled', false);
                                        return true;
                                    } else {
                                        $('#btnSearch').attr('disabled', true);
                                        return false;
                                    }
                                    break;

                                case 2:
                                    if (chkDate > 0 && chkDate < 30) {
                                        $('#btnSearch').attr('disabled', false);
                                        return true;
                                    } else {
                                        $('#btnSearch').attr('disabled', true);
                                        return false;
                                    }
                                    break;
                                case 4:
                                case 6:
                                case 9:
                                case 11:
                                    if (chkDate > 0 && chkDate < 31) {
                                        $('#btnSearch').attr('disabled', false);
                                        return true;
                                    } else {
                                        $('#btnSearch').attr('disabled', true);
                                        return false;
                                    }
                                    break;
                            }
                        } else {
                            $('#btnSearch').attr('disabled', true);
                            return false;
                        }
                    }

                } else {
                    $('#btnSearch').attr('disabled', true);
                    return false;
                };
            }, "วันที่ไม่ถูกต้อง");
        });

        function LoadFieldErrorCode() {

            if (errorCodeVal != '') {

                var errorCodeStrArr = $.trim(errorCodeVal).split(' ');

                $('#errorTable > tbody').empty();

                for (var i = 0; i < errorCodeStrArr.length; i++) {
                    $('#errorTable > tbody').append(
                        '<tr><td>' +
                        (i + 1) + '.' + errorCodeStrArr[i] +
                        '</td>' +
                        '<td>' +
                        '<input type="text" id="salemancode' + i + '" class="form-control code" name="salemancode' + i + '" required="required"/>' +
                        '</td>' +
                        '</tr>'
                    );
                }
            } else {
                $('#errorTable').hide();
            }

            $('#errorcodefield').css('display', '');
            $('#addcodefield').css('display', 'none');
        }

        function SetCode() {

            if (errorCodeVal != '') {
                var errorCodeStrArr = $.trim(errorCodeVal).split(' ');

                var saveCodeTxt = '';

                var count = 0;

                $('.code').each(function () {
                    if (count = 0) {
                        saveCodeTxt = saveCodeTxt + $(this).val();
                    } else {
                        saveCodeTxt = saveCodeTxt + ' ' + $(this).val();
                    }
                    count++;
                });

                $('#saveCode').val(saveCodeTxt);

            }

        }
    </script>
</body>
</html>
