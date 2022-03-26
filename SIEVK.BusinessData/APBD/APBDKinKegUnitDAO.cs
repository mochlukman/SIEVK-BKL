using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.APBD
{
    public class APBDKinKegUnitDAO
    {
        public DataTable GetKinKegUnitList(string unitKey, string kdTahap, string kdTahun, string idKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDKeg", SqlDbType.VarChar, idKeg);

                dt = db.ExecuteDataTable("sp_APBD_KinKegUnitList");
            }
            return dt;
        }

        public DataTable GetKinKegUnitbyId(string unitKey, string kdTahap, string kdTahun, string idKeg, string noUrkin)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDKeg", SqlDbType.VarChar, idKeg);
                db.AddParameter("NOURKIN", SqlDbType.VarChar, noUrkin);

                dt = db.ExecuteDataTable("sp_APBD_KinKegUnitId");
            }
            return dt;
        }

        public void Create(APBDKinKegUnit kinKegUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKeg", SqlDbType.VarChar, kinKegUnit.IDKEG);

                db.AddParameter("UnitKey", SqlDbType.VarChar, kinKegUnit.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kinKegUnit.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kinKegUnit.KDTAHUN);

                db.AddParameter("NOURKIN", SqlDbType.VarChar, kinKegUnit.NOURKIN);
                db.AddParameter("URKIN", SqlDbType.VarChar, kinKegUnit.URKIN);
                db.AddParameter("TARGET", SqlDbType.VarChar, kinKegUnit.TARGET);
                db.AddParameter("REALISASI", SqlDbType.VarChar, kinKegUnit.REALISASI);
                db.AddParameter("KET", SqlDbType.VarChar, kinKegUnit.KET);

                db.ExecuteNonQuery("sp_APBD_KinKegUnitInsert");
            }
        }

        public void Update(APBDKinKegUnit kinKegUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKeg", SqlDbType.VarChar, kinKegUnit.IDKEG);

                db.AddParameter("UnitKey", SqlDbType.VarChar, kinKegUnit.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kinKegUnit.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kinKegUnit.KDTAHUN);

                db.AddParameter("NOURKIN", SqlDbType.VarChar, kinKegUnit.NOURKIN);
                db.AddParameter("URKIN", SqlDbType.VarChar, kinKegUnit.URKIN);
                db.AddParameter("TARGET", SqlDbType.VarChar, kinKegUnit.TARGET);
                db.AddParameter("REALISASI", SqlDbType.VarChar, kinKegUnit.REALISASI);
                db.AddParameter("KET", SqlDbType.VarChar, kinKegUnit.KET);
                
                db.ExecuteNonQuery("sp_APBD_KinKegUnitUpdate");
            }
        }

        public void Delete(string idKeg, string kdTahun, string kdTahap, string unitKey, string noUrkin)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKeg", SqlDbType.VarChar, idKeg);

                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);

                db.AddParameter("NOURKIN", SqlDbType.VarChar, noUrkin);
                db.ExecuteNonQuery("sp_APBD_KinKegUnitDelete");
            }
        }

    }
}
