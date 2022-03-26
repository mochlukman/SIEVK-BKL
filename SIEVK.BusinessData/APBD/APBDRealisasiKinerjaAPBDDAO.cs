using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.APBD
{
    public class APBDRealisasiKinerjaAPBDDAO 
    {        
        public DataTable GetKinerjaAPBD(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                dt = db.ExecuteDataTable("sp_APBD_KinerjaAPBDList");
            }
            return dt;
        }

        public DataTable GetKinerjaAPBDRealisasi(string unitKey, string kdTahap, string kdTahun, string iDKeg, string kdTrwn = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDKeg", SqlDbType.VarChar, iDKeg);
                db.AddParameter("KDTrwn", SqlDbType.VarChar, kdTrwn);
                dt = db.ExecuteDataTable("sp_APBD_KinerjaAPBDRealisasiList");
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
                db.ExecuteNonQuery("sp_APBD_KinerjaAPBDRealisasiDelete");
            }
        }
        
        public void Update(APBDKinerja realisasi)
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
                db.AddParameter("RealStr1", SqlDbType.Decimal, realisasi.REALSTR1);
                db.AddParameter("RealFisik", SqlDbType.Decimal, realisasi.REALFISIK);
                db.AddParameter("Satuan", SqlDbType.VarChar, realisasi.SATUAN);
                db.AddParameter("Satuan1", SqlDbType.VarChar, realisasi.SATUAN1);
                db.AddParameter("Masalah", SqlDbType.VarChar, realisasi.MASALAH);
                db.AddParameter("Solusi", SqlDbType.VarChar, realisasi.SOLUSI);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                
                db.ExecuteNonQuery("sp_APBD_KinerjaAPBDRealisasiUpdate");
            }
        }

        public void Create(APBDKinerja realisasi)
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
                db.AddParameter("RealStr1", SqlDbType.Decimal, realisasi.REALSTR1);
                db.AddParameter("RealFisik", SqlDbType.Decimal, realisasi.REALFISIK);
                db.AddParameter("Satuan", SqlDbType.VarChar, realisasi.SATUAN);
                db.AddParameter("Satuan1", SqlDbType.VarChar, realisasi.SATUAN1);
                db.AddParameter("Masalah", SqlDbType.VarChar, realisasi.MASALAH);
                db.AddParameter("Solusi", SqlDbType.VarChar, realisasi.SOLUSI);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);

                db.ExecuteNonQuery("sp_APBD_KinerjaAPBDRealisasiInsert");
            }
        }
              
    }
}
