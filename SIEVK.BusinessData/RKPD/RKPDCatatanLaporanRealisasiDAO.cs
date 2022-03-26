using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RKPD;
using System.Data;

namespace SIEVK.BusinessData.RKPD
{
    public class RKPDCatatanLaporanRealisasiDAO 
    {        
        public DataTable GetCatatanLaporanRealisasi(string kdTahun, string kdTriwulan, string noCtt = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, kdTriwulan);
                db.AddParameter("NOCtt", SqlDbType.VarChar, noCtt);
                dt = db.ExecuteDataTable("sp_RKPD_CatatanLaporanRealisasiList");
            }
            return dt;
        }

        public void Delete(string kdTahun, string kdTriwulan)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, kdTriwulan);
                db.ExecuteNonQuery("sp_RKPD_CatatanLaporanRealisasiDelete");
            }
        }

        public void Create(RKPDCatatanLaporanRealisasi realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, realisasi.KDTRWN);
                db.AddParameter("NOCtt", SqlDbType.VarChar, realisasi.NOCTT);
                db.AddParameter("NMCtt", SqlDbType.VarChar, realisasi.NMCTT);
                db.AddParameter("IsiCtt", SqlDbType.VarChar, realisasi.ISICTT);
                db.ExecuteNonQuery("sp_RKPD_CatatanLaporanRealisasiCreate");
            }
        }

        public void Update(RKPDCatatanLaporanRealisasi realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, realisasi.KDTRWN);
                db.AddParameter("NOCtt", SqlDbType.VarChar, realisasi.NOCTT);
                db.AddParameter("NMCtt", SqlDbType.VarChar, realisasi.NMCTT);
                db.AddParameter("IsiCtt", SqlDbType.VarChar, realisasi.ISICTT);                
                db.ExecuteNonQuery("sp_RKPD_CatatanLaporanRealisasiInsert");
            }
        }

      
    }
}
