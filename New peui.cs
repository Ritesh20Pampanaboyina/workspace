using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls; // Ensure this is included for ListItem

namespace Percentage_Enrollment_UI
{
    public partial class Percentage_Enrollment_UI : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load the program list on initial page load
                LoadPrograms();
            }
        }

        private void LoadPrograms()
        {
            DataAccessLayer dataAccess = new DataAccessLayer();
            DataTable programs = dataAccess.GetPrograms();

            ddlProgramIds.Items.Clear();

            // Bind the program list to the dropdown
            foreach (DataRow row in programs.Rows)
            {
                string programId = row["programid"].ToString();
                string description = row["description"].ToString();

                ListItem item = new ListItem(description, programId);
                ddlProgramIds.Items.Add(item);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string auditType = ddlAuditType.Value;
            string enrollmentSpecialist = ddlEnrollmentSpecialist.Value;
            string dsu = ddlDSU.Value;
            string groups = ddlGroups.Value;
            string numAddsTerms = txtAddsTerms.Value;
            string percentChanges = txtPercentChanges.Value;
            string beginDate = txtBeginDate.Value;
            string endDate = txtEndDate.Value;

            string selectedPrograms = string.Join(",", ddlProgramIds.Items.Cast<ListItem>()
                .Where(i => i.Selected)
                .Select(i => i.Value));

            // Here you would handle the logic to store this data (not implemented)
            // For example, you could call another stored procedure to insert this data.

            // Show success message
            lblMessage.Text = "Audit request submitted successfully!";
            lblMessage.Visible = true;

            // Clear fields
            ClearFields();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            ddlAuditType.SelectedIndex = 0;
            ddlEnrollmentSpecialist.SelectedIndex = 0;
            ddlDSU.SelectedIndex = 0;
            ddlGroups.SelectedIndex = 0;
            txtAddsTerms.Value = string.Empty;
            txtPercentChanges.Value = string.Empty;
            txtBeginDate.Value = string.Empty;
            txtEndDate.Value = string.Empty;

            // Clear the selected items in the programs list
            foreach (ListItem item in ddlProgramIds.Items)
            {
                item.Selected = false;
            }

            lblMessage.Visible = false; // Hide the message label
        }
    }
}
