using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RPJPD;
using System.Data;

namespace SIEVK.BusinessData.RPJPD
{
    public class RPJPDEvaluasiBijakDAO
    {        
        public DataTable GetEvaluasiBijak()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("sp_RPJPD_EVABIJAKRPJPDList");
            }
            return dt;
        }

        public DataTable GetEvaluasiBijak(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RPJPD_EVABIJAKRPJPDByID");
            }
            return dt;
        }

        public void Delete(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RPJPD_EVABIJAKRPJPDDelete");
            }
        }

        public void Create(RPJPDEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RPJPD_EVABIJAKRPJPDInsert");
            }
        }

        public void Update(RPJPDEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RPJPD_EVABIJAKRPJPDUpdate");
            }
        }      
    }
}
