using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RENSTRA;
using System.Data;

namespace SIEVK.BusinessData.RENSTRA
{
    public class RENSTRAEvaluasiBijakDAO
    {        
        public DataTable GetEvaluasiBijak(string kdUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                dt = db.ExecuteDataTable("sp_RENSTRA_EVABIJAKRENSTRAList");
            }
            return dt;
        }

        public DataTable GetEvaluasiBijak(string kdUnit, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RENSTRA_EVABIJAKRENSTRAByID");
            }
            return dt;
        }

        public void Delete(string kdUnit, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RENSTRA_EVABIJAKRENSTRADelete");
            }
        }

        public void Create(RENSTRAEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENSTRA_EVABIJAKRENSTRAInsert");
            }
        }

        public void Update(RENSTRAEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENSTRA_EVABIJAKRENSTRAUpdate");
            }
        }      
    }
}
