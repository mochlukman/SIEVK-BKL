using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RPJPD;
using System.Data;

namespace SIEVK.BusinessData.RPJPD
{
    public class RPJPDEvaluasiSimpulDAO
    {        
        public DataTable GetEvaluasiSimpul()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("sp_RPJPD_EVASIMPULRPJPDList");
            }
            return dt;
        }

        public DataTable GetEvaluasiSimpul(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RPJPD_EVASIMPULRPJPDByID");
            }
            return dt;
        }

        public void Delete(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RPJPD_EVASIMPULRPJPDDelete");
            }
        }

        public void Create(RPJPDEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RPJPD_EVASIMPULRPJPDInsert");
            }
        }

        public void Update(RPJPDEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RPJPD_EVASIMPULRPJPDUpdate");
            }
        }      
    }
}
