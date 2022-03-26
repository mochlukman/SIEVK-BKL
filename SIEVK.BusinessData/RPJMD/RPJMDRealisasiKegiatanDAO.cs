using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDRealisasiKegiatanDAO
    {
        public DataTable GetKegiatanPagu(string unitKey, string kdTahap, string idPrgrm, string idkeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                db.AddParameter("IDKeg", SqlDbType.VarChar, idkeg);
                dt = db.ExecuteDataTable("sp_RPJMD_KegiatanPaguList");
            }
            return dt;
        }

        public DataTable GetKegiatanRealisasi(string unitKey, string kdTahap, string IDKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDKeg", SqlDbType.VarChar, IDKeg);
                dt = db.ExecuteDataTable("sp_RPJMD_KegiatanRealisasiList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string IDKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDKeg", SqlDbType.VarChar, IDKeg);
                db.ExecuteNonQuery("sp_RPJMD_KegiatanRealisasiDelete");
            }
        }

        public void Update(RPJMDKegiatan realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDKeg", SqlDbType.VarChar, realisasi.IDKEG);
                db.AddParameter("Real1", SqlDbType.Decimal, realisasi.REAL1);
                db.AddParameter("Real2", SqlDbType.Decimal, realisasi.REAL2);
                db.AddParameter("Real3", SqlDbType.Decimal, realisasi.REAL3);
                db.AddParameter("Real4", SqlDbType.Decimal, realisasi.REAL4);
                db.AddParameter("Real5", SqlDbType.Decimal, realisasi.REAL5);
                db.AddParameter("Real6", SqlDbType.Decimal, realisasi.REAL6);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_KegiatanRealisasiUpdate");
            }
        }

        public void Create(RPJMDKegiatan realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDKeg", SqlDbType.VarChar, realisasi.IDKEG);
                db.AddParameter("Real1", SqlDbType.Decimal, realisasi.REAL1);
                db.AddParameter("Real2", SqlDbType.Decimal, realisasi.REAL2);
                db.AddParameter("Real3", SqlDbType.Decimal, realisasi.REAL3);
                db.AddParameter("Real4", SqlDbType.Decimal, realisasi.REAL4);
                db.AddParameter("Real5", SqlDbType.Decimal, realisasi.REAL5);
                db.AddParameter("Real6", SqlDbType.Decimal, realisasi.REAL6);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_KegiatanRealisasiInsert");
            }
        }
    }
}
