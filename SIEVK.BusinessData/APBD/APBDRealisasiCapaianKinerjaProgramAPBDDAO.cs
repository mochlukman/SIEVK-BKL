using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.APBD
{
    public class APBDRealisasiCapaianKinerjaProgramAPBDDAO 
    {        
        public DataTable GetKinerjaProgramProgramList(string unitKey, string kdTahap, string kdTahun)
        {

            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                dt = db.ExecuteDataTable("[sp_APBD_KinerjaProgramProgramList]");
            }
            return dt;
        }

        public DataTable GetKinerjaProgramRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string Kinpgrapbdkey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("Kinpgrapbdkey", SqlDbType.VarChar, Kinpgrapbdkey);
                dt = db.ExecuteDataTable("[sp_APBD_KinerjaProgramRealisasiList]");
            }
            return dt;
        }

        public DataTable GetKinerjaProgramCapaianKinerjaList(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                dt = db.ExecuteDataTable("[sp_APBD_KinerjaProgramCapaianKinerjaList]");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string kdTahun, string IDPrgrm, string kinPgrapbdKey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("KinPgRapBdKey", SqlDbType.VarChar, kinPgrapbdKey);
                db.ExecuteNonQuery("[sp_APBD_KinerjaProgramRealisasiDelete]");
            }
        }

        public void Update(APBDCapaianKinerjaProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("KINPGRAPBDKEY", SqlDbType.VarChar, realisasi.KINPGRAPBDKEY);
                db.AddParameter("IDPRGRM", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTarAkhir", SqlDbType.VarChar, realisasi.REALTARAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("[sp_APBD_KinerjaProgramRealisasiUpdate]");
            }
        }

        public void Create(APBDCapaianKinerjaProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("KINPGRAPBDKEY", SqlDbType.VarChar, realisasi.KINPGRAPBDKEY);
                db.AddParameter("IDPRGRM", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTarAkhir", SqlDbType.VarChar, realisasi.REALTARAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("[sp_APBD_KinerjaProgramRealisasiInsert]");
            }
        }

    }
}
