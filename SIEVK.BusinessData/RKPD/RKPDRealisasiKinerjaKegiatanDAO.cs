using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RKPD;
using System.Data;

namespace SIEVK.BusinessData.RKPD
{
    public class RKPDRealisasiKinerjaKegiatanDAO 
    {        
        public DataTable GetKinerjaKegiatan(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                dt = db.ExecuteDataTable("sp_RKPD_KinerjaKegiatanList");
            }
            return dt;
        }

        public DataTable GetKinerjaKegiatanRealisasi(string unitKey, string kdTahap, string kdTahun, string iDKeg, string kdTrwn = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDKeg", SqlDbType.VarChar, iDKeg);
                db.AddParameter("KDTrwn", SqlDbType.VarChar, kdTrwn);
                dt = db.ExecuteDataTable("sp_RKPD_KinerjaKegiatanRealisasiList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string kdTahun, string iDKeg, string kdTrwn)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDKeg", SqlDbType.VarChar, iDKeg);
                db.AddParameter("KDTrwn", SqlDbType.VarChar, kdTrwn);
                db.ExecuteNonQuery("sp_RKPD_KinerjaKegiatanRealisasiDelete");
            }
        }

        public void Update(RKPDKinerjaKegiatan realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("IDKeg", SqlDbType.VarChar, realisasi.IDKEG);
                db.AddParameter("KDTrwn", SqlDbType.VarChar, realisasi.KDTRWN);
                db.AddParameter("RealNum", SqlDbType.Decimal, realisasi.REALNUM);
                db.AddParameter("RealStr", SqlDbType.Decimal, realisasi.REALSTR);
                db.AddParameter("RealFisik", SqlDbType.Decimal, realisasi.REALFISIK);
                db.AddParameter("Satuan", SqlDbType.VarChar, realisasi.SATUAN);
                db.AddParameter("Masalah", SqlDbType.VarChar, realisasi.MASALAH);
                db.AddParameter("Solusi", SqlDbType.VarChar, realisasi.SOLUSI);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RKPD_KinerjaKegiatanRealisasiUpdate");
            }
        }

        public void Create(RKPDKinerjaKegiatan realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKeg", SqlDbType.VarChar, realisasi.IDKEG);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTrwn", SqlDbType.VarChar, realisasi.KDTRWN);
                db.AddParameter("RealNum", SqlDbType.Decimal, realisasi.REALNUM);
                db.AddParameter("RealStr", SqlDbType.Decimal, realisasi.REALSTR);
                db.AddParameter("RealFisik", SqlDbType.Decimal, realisasi.REALFISIK);
                db.AddParameter("Satuan", SqlDbType.VarChar, realisasi.SATUAN);
                db.AddParameter("Masalah", SqlDbType.VarChar, realisasi.MASALAH);
                db.AddParameter("Solusi", SqlDbType.VarChar, realisasi.SOLUSI);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RKPD_KinerjaKegiatanRealisasiInsert");
            }
        }
              
    }
}
