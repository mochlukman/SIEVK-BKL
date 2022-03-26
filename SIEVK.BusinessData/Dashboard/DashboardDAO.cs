using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.Dashboard
{
    public class DashboardDAO 
    {
        public DataTable PenyerapanAnggaran(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("@KDTRWN", SqlDbType.VarChar, kdTrwln);
                db.AddParameter("@KDTAHAP", SqlDbType.VarChar, kdTahap);
                db.AddParameter("@KDTAHUN", SqlDbType.VarChar, kdTahun);
                dt = db.ExecuteDataTable("sp_Dashboard_PenyerapanAnggaran");
            }
            return dt;
        }

        public DataTable PenyerapanFisik(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTRWN", SqlDbType.Char, kdTrwln);
                db.AddParameter("KDTAHUN", SqlDbType.Char, kdTahun);
                db.AddParameter("KDTAHAP", SqlDbType.Char, kdTahap);
                dt = db.ExecuteDataTable("[sp_Dashboard_PenyerapanFisik]");
            }
            return dt;
        }        
      
    }
}
