<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Benefit_Audit_Extract.aspx.cs" Inherits="Benefit_Audit_Extract.Benefit_Audit_Extract" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Benefit Audit Extract UI</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }

        h2 {
            background-color: #4CAF50;
            color: white;
            padding: 10px;
            text-align: center;
            font-size: 20px;
        }

        .form-container {
            border: 1px solid #ccc;
            padding: 20px;
            margin-top: 10px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: inline-block;
            width: 220px;
        }

        input[type="text"], input[type="date"], select {
            padding: 5px;
            width: 250px;
        }

        .run-audit-btn, .download-btn, .reset-btn {
            padding: 5px 15px;
            color: white;
            border: none;
            cursor: pointer;
            margin-left: 10px;
        }

        .run-audit-btn {
            background-color: #28a745;
        }

        .download-btn {
            background-color: #007bff;
        }

        .reset-btn {
            background-color: #dc3545;
        }

        .form-actions {
            margin-top: 20px;
            text-align: right;
        }

        .history-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .history-table th, .history-table td {
            border: 1px solid #ccc;
            padding: 8px;
            text-align: left;
        }

        .history-table th {
            background-color: #f4f4f4;
        }
    </style>

    <script type="text/javascript">
        window.onload = function () {
            var dateInputs = document.querySelectorAll('input[type="date"]');
            dateInputs.forEach(function (input) {
                input.setAttribute('value', input.value);  // For date input reset
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Benefit Audit Extracts</h2>

        <div class="form-container">
            <div class="form-group">
                <label for="ddlAuditType">Select Audit Type:</label>
                <asp:DropDownList ID="ddlAuditType" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="FOCUS">Focus Audits</asp:ListItem>
                    <asp:ListItem Value="RANDOM">Random Audits</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtStartDate">Select Start Date:</label>
                <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtEndDate">Select End Date:</label>
                <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtRecordCount">Number of Records:</label>
                <asp:TextBox ID="txtRecordCount" runat="server" MaxLength="5" />
            </div>

            <div class="form-group">
                <label for="txtPercentage">Percentage of Records:</label>
                <asp:TextBox ID="txtPercentage" runat="server" MaxLength="3" />
            </div>

            <div class="form-actions">
                <asp:Button ID="btnRunAudit" CssClass="run-audit-btn" Text="Run Audit" OnClick="btnRunAudit_Click" runat="server" />
                <asp:Button ID="btnDownloadXML" CssClass="download-btn" Text="Download XML" OnClick="btnDownloadXML_Click" runat="server" Enabled="false" EnableViewState="True" />
                <asp:Button ID="btnReset" CssClass="reset-btn" Text="Reset" OnClick="btnReset_Click" runat="server" />
            </div>
           

            <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Green"></asp:Label>
        </div>

        <div class="form-container">
    <h3>Audit History</h3>
    <asp:GridView ID="gvAuditHistory" runat="server" AutoGenerateColumns="False" CssClass="history-table" GridLines="None">
        <Columns>
            <asp:BoundField DataField="AuditType" HeaderText="Audit Type" />
            <asp:BoundField DataField="DateGenerated" HeaderText="Date Generated" 
                            DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Download XML">
                <ItemTemplate>
                    <a href='<%# Eval("FilePath", ResolveUrl("~/DownloadXML.ashx?file={0}")) %>' 
                       target="_blank">
                        <%# Eval("FileName") %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>

    </form>
</body>
</html>
