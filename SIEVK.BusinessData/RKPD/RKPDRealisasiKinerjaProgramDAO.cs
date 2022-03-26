using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RKPD;
using System.Data;

namespace SIEVK.BusinessData.RKPD
{
    public class RKPDRealisasiKinerjaProgramDAO
    {        
        public DataTable GetKinerjaProgram(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                dt = db.ExecuteDataTable("sp_RKPD_KinerjaProgramList");
            }
            return dt;
        }

        public DataTable GetKinerjaProgramRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgRKPDKey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("KinPgRKPDKey", SqlDbType.VarChar, kinPgRKPDKey);
                dt = db.ExecuteDataTable("sp_RKPD_KinerjaProgramRealisasiList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgRKPDKey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("KinPgRKPDKey", SqlDbType.VarChar, kinPgRKPDKey);
                db.ExecuteNonQuery("sp_RKPD_KinerjaProgramRealisasiDelete");
            }
        }

        public void Update(RKPDKinerjaProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("KinPgRKPDKey", SqlDbType.VarChar, realisasi.KINPGRKPDKEY);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTarAkhir", SqlDbType.VarChar, realisasi.REALTARAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RKPD_KinerjaProgramRealisasiUpdate");
            }
        }


        public void Create(RKPDKinerjaProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("KinPgRKPDKey", SqlDbType.VarChar, realisasi.KINPGRKPDKEY);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTarAkhir", SqlDbType.VarChar, realisasi.REALTARAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RKPD_KinerjaProgramRealisasiInsert");
            }
        }

    }
}
