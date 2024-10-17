using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Percentage_Enrollment_UI
{
    public partial class Percentage_Enrollment_UI : Page
    {
        private DataAccessLayer dataAccess = new DataAccessLayer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind the program checkboxes when the page is loaded for the first time
                BindProgramCheckboxes();
            }
        }

        private void BindProgramCheckboxes()
        {
            // Fetch program data from the database
            DataTable programs = dataAccess.GetPrograms();

            // Clear existing controls before adding new ones
            programCheckboxes.Controls.Clear();

            if (programs.Rows.Count > 0)
            {
                // Read from the database and create checkboxes for each program
                foreach (DataRow row in programs.Rows)
                {
                    string programId = row["programid"].ToString();
                    string description = row["description"].ToString();

                    CheckBox programCheckbox = new CheckBox
                    {
                        ID = "chkProgram_" + programId,
                        Text = description,
                        CssClass = "program-checkbox"
                    };
                    programCheckboxes.Controls.Add(programCheckbox);
                    programCheckboxes.Controls.Add(new LiteralControl("<br />"));
                }
            }
            else
            {
                // If no records are returned, use hardcoded program IDs
                var hardcodedProgramIds = new[]
                {
                    "PGMP0000000004", "PGM0000000348", "PGM0000000349", 
                    "PGM0000000352", "PGM0000000353", "PGMP0000000005",
                    "PGMP0000000007", "PGMP0000000008", "PGMP0000000009",
                    "PGMP0000000010", "PGMP0000000019", "PGMP0000000020"
                };

                foreach (var programId in hardcodedProgramIds)
                {
                    CheckBox programCheckbox = new CheckBox
                    {
                        ID = "chkProgram_" + programId,
                        Text = programId, // Display programId itself for manual entries
                        CssClass = "program-checkbox"
                    };
                    programCheckboxes.Controls.Add(programCheckbox);
                    programCheckboxes.Controls.Add(new LiteralControl("<br />"));
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Collect selected program IDs
            string selectedPrograms = string.Empty;
            foreach (Control control in programCheckboxes.Controls)
            {
                if (control is CheckBox checkbox && checkbox.Checked)
                {
                    selectedPrograms += checkbox.Text + ",";
                }
            }

            // Remove trailing comma
            if (selectedPrograms.EndsWith(","))
            {
                selectedPrograms = selectedPrograms.TrimEnd(',');
            }

            // Call the DataAccessLayer to submit the audit request
            dataAccess.SubmitAuditRequest(
                ddlAuditType.SelectedValue,
                ddlEnrollmentSpecialist.SelectedValue,
                ddlDSU.SelectedValue,
                ddlGroups.SelectedValue,
                int.TryParse(txtAddsTerms.Text, out int numAddsTerms) ? numAddsTerms : 0,
                decimal.TryParse(txtPercentChanges.Text, out decimal percentChanges) ? percentChanges : 0,
                DateTime.TryParse(txtBeginDate.Text, out DateTime beginDate) ? beginDate : DateTime.MinValue,
                DateTime.TryParse(txtEndDate.Text, out DateTime endDate) ? endDate : DateTime.MinValue,
                selectedPrograms
            );

            // Show success message
            lblMessage.Text = "Audit request submitted successfully!";
            lblMessage.Visible = true; 
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // Logic to reset the form fields
            ddlAuditType.SelectedIndex = 0;
            ddlEnrollmentSpecialist.SelectedIndex = 0;
            ddlDSU.SelectedIndex = 0;
            ddlGroups.SelectedIndex = 0;
            txtAddsTerms.Text = string.Empty;
            txtPercentChanges.Text = string.Empty;
            txtBeginDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;

            // Uncheck all checkboxes
            foreach (Control control in programCheckboxes.Controls)
            {
                if (control is CheckBox checkbox)
                {
                    checkbox.Checked = false;
                }
            }
        }
    }
}
