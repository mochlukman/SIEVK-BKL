using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RENJA;
using System.Data;

namespace SIEVK.BusinessData.RENJA
{
    public class RENJAEvaluasiBijakDAO
    {        
        public DataTable GetEvaluasiBijak(string kdTahun, string kdUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                dt = db.ExecuteDataTable("sp_RENJA_EVABIJAKRENJAList");
            }
            return dt;
        }

        public DataTable GetEvaluasiBijak(string kdTahun, string kdUnit, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RENJA_EVABIJAKRENJAByID");
            }
            return dt;
        }

        public void Delete(string kdTahun, string kdUnit, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RENJA_EVABIJAKRENJADelete");
            }
        }

        public void Create(RENJAEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENJA_EVABIJAKRENJAInsert");
            }
        }

        public void Update(RENJAEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENJA_EVABIJAKRENJAUpdate");
            }
        }      
    }
}
