using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RKPD;
using System.Data;

namespace SIEVK.BusinessData.RKPD
{
    public class RKPDEvaluasiSimpulDAO
    {        
        public DataTable GetEvaluasiSimpul(string kdTahun)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                dt = db.ExecuteDataTable("sp_RKPD_EVASIMPULRKPDList");
            }
            return dt;
        }

        public DataTable GetEvaluasiSimpul(string kdTahun, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RKPD_EVASIMPULRKPDByID");
            }
            return dt;
        }

        public void Delete(string kdTahun, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                db.ExecuteNonQuery("sp_RKPD_EVASIMPULRKPDDelete");
            }
        }

        public void Create(RKPDEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RKPD_EVASIMPULRKPDInsert");
            }
        }

        public void Update(RKPDEvaluasiSimpul evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("ASPEK", SqlDbType.VarChar, evaluasi.ASPEK);
                db.AddParameter("PENJELASAN", SqlDbType.VarChar, evaluasi.PENJELASAN);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RKPD_EVASIMPULRKPDUpdate");
            }
        }      
    }
}
