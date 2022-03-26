using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RPJMD;
using System.Data;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDEvaluasiSimpulDAO
    {        
        public DataTable GetEvaluasiSimpul()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("sp_RPJMD_EVASIMPULRPJMDList");
            }
            return dt;
        }

        public DataTable GetEvaluasiSimpul(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RPJMD_EVASIMPULRPJMDByID");
            }
            return dt;
        }

        public void Delete(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RPJMD_EVASIMPULRPJMDDelete");
            }
        }

        public void Create(RPJMDEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RPJMD_EVASIMPULRPJMDInsert");
            }
        }

        public void Update(RPJMDEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RPJMD_EVASIMPULRPJMDUpdate");
            }
        }      
    }
}
