using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RENJA;
using System.Data;

namespace SIEVK.BusinessData.RENJA
{
    public class RENJAEvaluasiKendaliDAO
    {        
        public DataTable GetEvaluasiKendali(string kdTahun, string kdUnit, string kdTahap)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("KDTAHAP", SqlDbType.VarChar, kdTahap);
                dt = db.ExecuteDataTable("sp_RENJA_EVAKENDALIRENJAList");
            }
            return dt;
        }

        public DataTable GetEvaluasiKendali(string kdTahun, string kdUnit, string nomor, string kdTahap)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("KDTAHAP", SqlDbType.VarChar, kdTahap);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RENJA_EVAKENDALIRENJAByID");
            }
            return dt;
        }

        public void Delete(string kdTahun, string kdUnit, string nomor, string kdTahap)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("KDTAHAP", SqlDbType.VarChar, kdTahap);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RENJA_EVAKENDALIRENJADelete");
            }
        }

        public void Create(RENJAEvaluasiKendali evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("KDTAHAP", SqlDbType.Char, evaluasi.KDTAHAP);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("TARGET_RENJA", SqlDbType.VarChar, evaluasi.TARGET_RENJA);
                db.AddParameter("TARGET_RKA", SqlDbType.VarChar, evaluasi.TARGET_RKA);
                db.AddParameter("PAGU_RKA", SqlDbType.Decimal, evaluasi.PAGU_RKA);
                db.AddParameter("PAGU_RENJA", SqlDbType.Decimal, evaluasi.PAGU_RENJA);
                db.AddParameter("TYPE", SqlDbType.Char, evaluasi.TYPE);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("HASIL", SqlDbType.VarChar, evaluasi.HASIL);
                db.ExecuteNonQuery("sp_RENJA_EVAKENDALIRENJAInsert");
            }
        }

        public void Update(RENJAEvaluasiKendali evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("KDTAHAP", SqlDbType.Char, evaluasi.KDTAHAP);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("TARGET_RENJA", SqlDbType.VarChar, evaluasi.TARGET_RENJA);
                db.AddParameter("TARGET_RKA", SqlDbType.VarChar, evaluasi.TARGET_RKA);
                db.AddParameter("PAGU_RKA", SqlDbType.Decimal, evaluasi.PAGU_RKA);
                db.AddParameter("PAGU_RENJA", SqlDbType.Decimal, evaluasi.PAGU_RENJA);
                db.AddParameter("TYPE", SqlDbType.Char, evaluasi.TYPE);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("HASIL", SqlDbType.VarChar, evaluasi.HASIL);
                db.ExecuteNonQuery("sp_RENJA_EVAKENDALIRENJAUpdate");
            }
        }      
    }
}
