using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RENJA;
using System.Data;

namespace SIEVK.BusinessData.RENJA
{
    public class RENJAEvaluasiSimpulDAO
    {        
        public DataTable GetEvaluasiSimpul(string kdTahun, string kdUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                dt = db.ExecuteDataTable("sp_RENJA_EVASIMPULRENJAList");
            }
            return dt;
        }

        public DataTable GetEvaluasiSimpul(string kdTahun, string kdUnit, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RENJA_EVASIMPULRENJAByID");
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
                db.ExecuteNonQuery("sp_RENJA_EVASIMPULRENJADelete");
            }
        }

        public void Create(RENJAEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENJA_EVASIMPULRENJAInsert");
            }
        }

        public void Update(RENJAEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENJA_EVASIMPULRENJAUpdate");
            }
        }      
    }
}
