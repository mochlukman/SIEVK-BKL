using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RKPD;
using System.Data;

namespace SIEVK.BusinessData.RKPD
{
    public class RKPDEvaluasiBijakDAO
    {        
        public DataTable GetEvaluasiBijak(string kdTahun)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                dt = db.ExecuteDataTable("sp_RKPD_EVABIJAKRKPDList");
            }
            return dt;
        }

        public DataTable GetEvaluasiBijak(string kdTahun, string nomor)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, kdTahun);
                db.AddParameter("NOMOR", SqlDbType.VarChar, nomor);
                dt = db.ExecuteDataTable("sp_RKPD_EVABIJAKRKPDByID");
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
                db.ExecuteNonQuery("sp_RKPD_EVABIJAKRKPDDelete");
            }
        }

        public void Create(RKPDEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RKPD_EVABIJAKRKPDInsert");
            }
        }

        public void Update(RKPDEvaluasiBijak evaluasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NOMOR", SqlDbType.Char, evaluasi.NOMOR);
                db.AddParameter("KDTAHUN", SqlDbType.Char, evaluasi.KDTAHUN);
                db.AddParameter("URAIAN", SqlDbType.VarChar, evaluasi.URAIAN);
                db.AddParameter("KESESUAIAN", SqlDbType.Int, evaluasi.KESESUAIAN);
                db.AddParameter("MASALAH", SqlDbType.VarChar, evaluasi.MASALAH);
                db.AddParameter("SOLUSI", SqlDbType.VarChar, evaluasi.SOLUSI);
                db.AddParameter("KET", SqlDbType.VarChar, evaluasi.KET);
                db.ExecuteNonQuery("sp_RKPD_EVABIJAKRKPDUpdate");
            }
        }      
    }
}
