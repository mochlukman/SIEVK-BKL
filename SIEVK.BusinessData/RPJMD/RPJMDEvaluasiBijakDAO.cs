using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RPJMD;
using System.Data;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDEvaluasiBijakDAO
    {        
        public DataTable GetEvaluasiBijak()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("sp_RPJMD_EVABIJAKRPJMDList");
            }
            return dt;
        }

        public DataTable GetEvaluasiBijak(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RPJMD_EVABIJAKRPJMDByID");
            }
            return dt;
        }

        public void Delete(string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RPJMD_EVABIJAKRPJMDDelete");
            }
        }

        public void Create(RPJMDEvaluasiBijak evaluasi)
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
                db.ExecuteNonQuery("sp_RPJMD_EVABIJAKRPJMDInsert");
            }
        }

        public void Update(RPJMDEvaluasiBijak evaluasi)
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
                db.ExecuteNonQuery("sp_RPJMD_EVABIJAKRPJMDUpdate");
            }
        }      
    }
}
