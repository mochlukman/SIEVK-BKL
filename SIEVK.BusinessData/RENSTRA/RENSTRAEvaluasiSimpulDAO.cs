using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RENSTRA;
using System.Data;

namespace SIEVK.BusinessData.RENSTRA
{
    public class RENSTRAEvaluasiSimpulDAO
    {        
        public DataTable GetEvaluasiSimpul(string kdUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                dt = db.ExecuteDataTable("sp_RENSTRA_EVASIMPULRENSTRAList");
            }
            return dt;
        }

        public DataTable GetEvaluasiSimpul(string kdUnit, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, kdUnit);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RENSTRA_EVASIMPULRENSTRAByID");
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
                db.ExecuteNonQuery("sp_RENSTRA_EVASIMPULRENSTRADelete");
            }
        }

        public void Create(RENSTRAEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENSTRA_EVASIMPULRENSTRAInsert");
            }
        }

        public void Update(RENSTRAEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("UNITKEY", SqlDbType.Char, evaluasi.UNITKEY);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RENSTRA_EVASIMPULRENSTRAUpdate");
            }
        }      
    }
}
