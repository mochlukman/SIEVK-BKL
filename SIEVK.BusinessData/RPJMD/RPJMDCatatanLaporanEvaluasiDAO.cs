using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDCatatanLaporanEvaluasiDAO
    {
        public DataTable GetCatatanLaporanEvaluasi(String kdtahun, String noctt)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdtahun);
                db.AddParameter("NoCtt", SqlDbType.VarChar, noctt);
                dt = db.ExecuteDataTable("[sp_RPJMD_CatatanLaporanEvaluasiList]");
            }
            return dt;
        }

        public void Delete(string kdtahun, string NoCtt)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdtahun);
                db.AddParameter("NoCtt", SqlDbType.VarChar, NoCtt);
                db.ExecuteNonQuery("[sp_RPJMD_CatatanLaporanEvaluasiDelete]");
            }
        }

        public void Update(RPJMDCatatanLaporanEvaluasi ctt)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, ctt.KDTAHUN);
                db.AddParameter("NoCtt", SqlDbType.VarChar, ctt.NOCTT);
                db.AddParameter("NmCtt", SqlDbType.VarChar, ctt.NMCTT);
                db.AddParameter("IsiCtt", SqlDbType.VarChar, ctt.ISICTT);
                db.ExecuteNonQuery("[sp_RPJMD_CatatanLaporanEvaluasiUpdate]");
            }
        }

        public void Insert(RPJMDCatatanLaporanEvaluasi ctt)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, ctt.KDTAHUN);
                db.AddParameter("NoCtt", SqlDbType.VarChar, ctt.NOCTT);
                db.AddParameter("NmCtt", SqlDbType.VarChar, ctt.NMCTT);
                db.AddParameter("IsiCtt", SqlDbType.VarChar, ctt.ISICTT);
                db.ExecuteNonQuery("[sp_RPJMD_CatatanLaporanEvaluasiInsert]");
            }
        }
    }
}
