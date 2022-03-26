using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDRealisasiKinerjaProgramDAO
    {
        public DataTable GetKinerjaProgram(string unitKey, string kdTahap, string idprgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idprgrm);
                dt = db.ExecuteDataTable("sp_RPJMD_KinerjaProgramList");
            }
            return dt;
        }

        public DataTable GetKinerjaProgramRealisasi(string unitKey, string kdTahap, string IDPrgrm, string kinpgrpjmkey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("KINPGRPJMKEY", SqlDbType.VarChar, kinpgrpjmkey);
                dt = db.ExecuteDataTable("[sp_RPJMD_KinerjaProgramRealisasiList]");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string IDPrgrm, string kinpgrpjmkey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("KINPGRPJMKEY", SqlDbType.VarChar, kinpgrpjmkey);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaProgramRealisasiDelete");
            }
        }

        public void Update(RPJMDKinerjaProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("KINPGRPJMKEY", SqlDbType.VarChar, realisasi.KINPGRPJMKEY);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTar5", SqlDbType.VarChar, realisasi.REALTAR5);
                db.AddParameter("RealTar6", SqlDbType.VarChar, realisasi.REALTAR6);
                db.AddParameter("RealTarAkhir", SqlDbType.VarChar, realisasi.REALTARAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaProgramRealisasiUpdate");
            }
        }

        public void Create(RPJMDKinerjaProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("KINPGRPJMKEY", SqlDbType.VarChar, realisasi.KINPGRPJMKEY);
                db.AddParameter("RealTar1", SqlDbType.VarChar, realisasi.REALTAR1);
                db.AddParameter("RealTar2", SqlDbType.VarChar, realisasi.REALTAR2);
                db.AddParameter("RealTar3", SqlDbType.VarChar, realisasi.REALTAR3);
                db.AddParameter("RealTar4", SqlDbType.VarChar, realisasi.REALTAR4);
                db.AddParameter("RealTar5", SqlDbType.VarChar, realisasi.REALTAR5);
                db.AddParameter("RealTar6", SqlDbType.VarChar, realisasi.REALTAR6);
                db.AddParameter("RealTarAkhir", SqlDbType.VarChar, realisasi.REALTARAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("[sp_RPJMD_KinerjaProgramRealisasiInsert]");
            }
        }   
    }
}
