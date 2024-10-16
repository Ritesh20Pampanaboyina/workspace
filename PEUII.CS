using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Percentage_Enrollment_UI
{
    public partial class Percentage_Enrollment_UI : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string auditType = ddlAuditType.Value;
            string enrollmentSpecialist = ddlEnrollmentSpecialist.Value;
            string dsu = ddlDSU.Value;
            string groups = ddlGroups.Value;

            int numAddsTerms = int.TryParse(txtAddsTerms.Value, out numAddsTerms) ? numAddsTerms : 0;
            decimal percentMemberChanges = decimal.TryParse(txtPercentChanges.Value, out percentMemberChanges) ? percentMemberChanges : 0;

            DateTime beginDate = DateTime.TryParse(txtBeginDate.Value, out beginDate) ? beginDate : DateTime.MinValue;
            DateTime endDate = DateTime.TryParse(txtEndDate.Value, out endDate) ? endDate : DateTime.MinValue;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AuditType", auditType),
                new SqlParameter("@EnrollmentSpecialist", enrollmentSpecialist),
                new SqlParameter("@DSU", dsu),
                new SqlParameter("@Groups", groups),
                new SqlParameter("@NumAddsTerms", numAddsTerms),
                new SqlParameter("@PercentMemberChanges", percentMemberChanges),
                new SqlParameter("@BeginDate", beginDate),
                new SqlParameter("@EndDate", endDate)
            };

            DataAccessLayer dataAccess = new DataAccessLayer();
            dataAccess.ExecuteStoredProcedure("SubmitEnrollmentAudit", parameters);
            lblMessage.Text = "Audit request submitted successfully!";
            lblMessage.Visible = true;
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Audit request submitted successfully!');", true);
            btnReset_Click(sender, e);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlAuditType.SelectedIndex = 0;
            ddlEnrollmentSpecialist.SelectedIndex = 0;
            ddlDSU.SelectedIndex = 0;
            ddlGroups.SelectedIndex = 0;
            txtAddsTerms.Value = string.Empty;
            txtPercentChanges.Value = string.Empty;
            txtBeginDate.Value = string.Empty;
            txtEndDate.Value = string.Empty;
            lblMessage.Visible = false;
        }
    }
}

