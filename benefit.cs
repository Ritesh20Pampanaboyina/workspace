using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Benefit_Audit_Extract
{
    public partial class Benefit_Audit_Extract : System.Web.UI.Page
    {
        private DataAccessLayer dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            dataAccess = new DataAccessLayer();

            if (!IsPostBack)
            {
                BindAuditHistory();
                LoadAuditHistory();
            }
        }

        private void LoadAuditHistory()
        {
            try
            {
                var dal = new DataAccessLayer();
                DataTable auditHistory = dal.GetAuditHistory();
                gvAuditHistory.DataSource = auditHistory;
                gvAuditHistory.DataBind();
            }
            catch (Exception ex)
            {
                DisplayMessage("Error loading audit history: " + ex.Message, System.Drawing.Color.Red);
            }
        }

        private void BindAuditHistory()
        {
            try
            {
                DataTable auditHistory = dataAccess.GetAuditHistory();
                if (auditHistory != null && auditHistory.Rows.Count > 0)
                {
                    foreach (DataRow row in auditHistory.Rows)
                    {
                        string filePath = row["FilePath"].ToString();
                        // Check if file exists and update the FileName column
                        if (File.Exists(filePath))
                        {
                            row["FileName"] = $"<a href='{filePath}' target='_blank'>{Path.GetFileName(filePath)}</a>";
                        }
                        else
                        {
                            row["FileName"] = "File not available. Waiting for generation.";
                        }
                    }

                    gvAuditHistory.DataSource = auditHistory;
                    gvAuditHistory.DataBind();
                }
                else
                {
                    gvAuditHistory.DataSource = null;
                    gvAuditHistory.DataBind();
                    DisplayMessage("No audit history found.", System.Drawing.Color.Red);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Error retrieving audit history: " + ex.Message, System.Drawing.Color.Red);
            }
        }

        protected void btnRunAudit_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate and process the date inputs
                DateTime startDate;
                DateTime endDate;
                string startDateStr = txtStartDate.Text.Trim();
                string endDateStr = txtEndDate.Text.Trim();

                bool isStartDateValid = DateTime.TryParseExact(startDateStr, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDate);
                bool isEndDateValid = DateTime.TryParseExact(endDateStr, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDate);

                if (!isStartDateValid || !isEndDateValid)
                {
                    DisplayMessage("Invalid date format. Please enter the date in yyyy-MM-dd format.", System.Drawing.Color.Red);
                    return;
                }

                if (startDate > endDate)
                {
                    DisplayMessage("Start date cannot be later than end date.", System.Drawing.Color.Red);
                    return;
                }

                // Get the selected audit type
                string auditType = ddlAuditType.SelectedValue;
                if (auditType != "FOCUS" && auditType != "RANDOM")
                {
                    DisplayMessage("Please select a valid audit type.", System.Drawing.Color.Red);
                    return;
                }

                string recordCount = txtRecordCount.Text.Trim();
                string percentage = txtPercentage.Text.Trim();

                // Update the audit data
                dataAccess.UpdateAuditData(auditType, startDate, endDate, string.IsNullOrEmpty(recordCount) ? null : recordCount, string.IsNullOrEmpty(percentage) ? null : percentage);

                // Display message indicating waiting for file generation
                DisplayMessage("Waiting for file generation. Please refresh the page after a few minutes.", System.Drawing.Color.Blue);

                // Call BindAuditHistory to refresh the audit history
                LoadAuditHistory();
                BindAuditHistory();

                // Force a refresh of the GridView to show updated data
                gvAuditHistory.DataSource = dataAccess.GetAuditHistory(auditType);
                gvAuditHistory.DataBind();

                // Enable the download XML button after the audit is complete
                btnDownloadXML.Enabled = true;

                DisplayMessage("Audit successfully completed. Download available in the history below.", System.Drawing.Color.Green);
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error running audit: {ex.Message}", System.Drawing.Color.Red);
            }
        }

        private void DisplayMessage(string message, System.Drawing.Color color)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = color;
            lblMessage.Visible = true;
        }

        protected void btnDownloadXML_Click(object sender, EventArgs e)
        {
            string auditType = ddlAuditType.SelectedValue;

            try
            {
                string filePath = dataAccess.GetLatestXMLFilePath(auditType);

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    string fileName = Path.GetFileName(filePath);

                    // Serve the file for download
                    Response.ContentType = "application/xml";
                    Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
                else
                {
                    DisplayMessage("XML file not found for the selected audit type. Please wait for it to be generated.", System.Drawing.Color.Red);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Error downloading XML: " + ex.Message, System.Drawing.Color.Red);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}
