using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDRealisasiKinerjaKegiatanDAO
    {
        public DataTable GetKinerjaKegiatan(string unitKey, string kdTahap, string IDKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDKeg", SqlDbType.VarChar, IDKeg);
                dt = db.ExecuteDataTable("sp_RPJMD_KinerjaKegiatanList");
            }
            return dt;
        }

        public DataTable GetKinerjaKegiatanRealisasi(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDKeg", SqlDbType.VarChar, IDKeg);
                db.AddParameter("KINKEGRPJMKEY", SqlDbType.VarChar, kinkegrpjmkey);
                dt = db.ExecuteDataTable("[sp_RPJMD_KinerjaKegiatanRealisasiList]");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDKeg", SqlDbType.VarChar, IDKeg);
                db.AddParameter("KINKEGRPJMKEY", SqlDbType.VarChar, kinkegrpjmkey);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaKegiatanRealisasiDelete");
            }
        }

        public void Update(RPJMDKinerjaKegiatan realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDKeg", SqlDbType.VarChar, realisasi.IDKEG);
                db.AddParameter("KINKEGRPJMKEY", SqlDbType.VarChar, realisasi.KINKEGRPJMKEY);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTar5", SqlDbType.VarChar, realisasi.REALTAR5);
                db.AddParameter("RealTar6", SqlDbType.VarChar, realisasi.REALTAR6);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaKegiatanRealisasiUpdate");
            }
        }

        public void Create(RPJMDKinerjaKegiatan realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDKeg", SqlDbType.VarChar, realisasi.IDKEG);
                db.AddParameter("KINKEGRPJMKEY", SqlDbType.VarChar, realisasi.KINKEGRPJMKEY);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTar5", SqlDbType.VarChar, realisasi.REALTAR5);
                db.AddParameter("RealTar6", SqlDbType.VarChar, realisasi.REALTAR6);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaKegiatanRealisasiCreate");
            }
        }   
    }
}
