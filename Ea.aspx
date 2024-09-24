<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnrollmentAudit.aspx.cs" Inherits="AuditApp.EnrollmentAudit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enrollment Audit - Focus & Random</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Enrollment Audit</h2>
            
            <!-- DropDown for Audit Type (Focus/Random) -->
            <asp:Label ID="lblAuditType" runat="server" Text="Audit Type: " />
            <asp:DropDownList ID="ddlAuditType" runat="server">
                <asp:ListItem Text="Focus" Value="Focus"></asp:ListItem>
                <asp:ListItem Text="Random" Value="Random"></asp:ListItem>
            </asp:DropDownList>
            
            <!-- Date Range for pulling Audit Data -->
            <asp:Label ID="lblStartDate" runat="server" Text="Start Date: " />
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>

            <asp:Label ID="lblEndDate" runat="server" Text="End Date: " />
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>

            <!-- Submit button to trigger the audit extraction -->
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Audit" OnClick="btnSubmit_Click" />

            <!-- Display the result or message -->
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
