//using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace SIEVK.Pemda.Bengkalis.Views
{
    public partial class ViewAPBDEvaluasiPerKegiatan2 : System.Web.UI.Page
    {
        static string conString = ConfigurationManager.ConnectionStrings["SIEVKConnection"].ConnectionString;
        static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
        string user = builder.UserID;
        string pass = builder.Password;
        string server = builder.DataSource;
        string databasename = builder.InitialCatalog;
        ReportDocument rd = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ReportDocument rd = new ReportDocument();
                //string reportPath = Request.QueryString["ReportPath"];
                //var kdtahap = Request.QueryString["TahapanCombo"].ToString();
                //var kdtahun = Request.QueryString["TahunCombo"].ToString();
                //var kdtrwn = Request.QueryString["TriwulanCombo"].ToString();
                //DateTime tgl = DateTime.ParseExact(Request.QueryString["FilterDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //sementara
                string reportPath = "~/Report/Evarenstraskpd.rpt";
                var kdtahap = "21";
                var unitkey = "2";
                DateTime tgl = DateTime.ParseExact(DateTime.Now.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //rptViewer.LocalReport.ReportPath = Server.MapPath(reportPath);
                rd.Load(Server.MapPath(reportPath));

                //ReportParameter[] parameters = new ReportParameter[3];
                //parameters[0] = new ReportParameter("@KDTAHAP", kdtahap);
                //parameters[1] = new ReportParameter("@UNITKEY", unitkey);
                //parameters[2] = new ReportParameter("@Tanggal", tgl.ToString());
                //rptViewer.LocalReport.SetParameters(parameters);

                SqlConnection sqlCon = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("WSPR_EVAKINKEU", sqlCon);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                rd.SetParameterValue("@KDTAHAP", kdtahap);
                rd.SetParameterValue("@UNITKEY", unitkey);
                rd.SetParameterValue("@Tanggal", tgl.ToString());

                rd.SetDataSource(dt);

                //ReportDataSource RDS = new ReportDataSource( "DataSet1", customers);

                //rptViewer.LocalReport.DataSources.Add(RDS);

                rptViewer.ReportSource = rd;
                //rptViewer.DataBind();
                rptViewer.RefreshReport();
            }
        }
    }
}