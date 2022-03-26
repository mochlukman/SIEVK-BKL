using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDKinerjaProgramDAO
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

        public DataTable GetKinerjaProgramLoad(string unitKey, string kdTahap, string IDPrgrm, string kinpgrpjmkey)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.AddParameter("KINPGRPJMKEY", SqlDbType.VarChar, kinpgrpjmkey);
                dt = db.ExecuteDataTable("[sp_RPJMD_KinerjaProgramLoad]");
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
                db.ExecuteNonQuery("sp_RPJMD_KinerjaProgramDelete");
            }
        }

        public void Update(RPJMDKinerjaProgram kinpgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, kinpgrm.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kinpgrm.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, kinpgrm.IDPRGRM);
                db.AddParameter("KINPGRPJMKEY", SqlDbType.VarChar, kinpgrm.KINPGRPJMKEY);
                db.AddParameter("Nokin", SqlDbType.VarChar, kinpgrm.NOKIN);
                db.AddParameter("Urkin", SqlDbType.VarChar, kinpgrm.URKIN);
                db.AddParameter("Kinlalu", SqlDbType.VarChar, kinpgrm.KINLALU);
                db.AddParameter("Target1", SqlDbType.VarChar, kinpgrm.TARGET1);
                db.AddParameter("Target2", SqlDbType.VarChar, kinpgrm.TARGET2);
                db.AddParameter("Target3", SqlDbType.VarChar, kinpgrm.TARGET3);
                db.AddParameter("Target4", SqlDbType.VarChar, kinpgrm.TARGET4);
                db.AddParameter("Target5", SqlDbType.VarChar, kinpgrm.TARGET5);
                db.AddParameter("Target6", SqlDbType.VarChar, kinpgrm.TARGET6);
                db.AddParameter("Targetakhir", SqlDbType.VarChar, kinpgrm.TARGETAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, kinpgrm.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, kinpgrm.CTT);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaProgramUpdate");
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
                db.ExecuteNonQuery("[sp_RPJMD_KinerjaProgramInsert]");
            }
        }
    }
}
