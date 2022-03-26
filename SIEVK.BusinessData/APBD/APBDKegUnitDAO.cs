using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.APBD
{
    public class APBDKegUnitDAO
    {
        public DataTable GetbyId(string unitKey, string kdTahap, string kdTahun, string idKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDKeg", SqlDbType.VarChar, idKeg);

                dt = db.ExecuteDataTable("sp_APBD_KegUnitId");
            }
            return dt;
        }

        public void Update(APBDKinerja kegUnit)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKeg", SqlDbType.VarChar, kegUnit.IDKEG);

                db.AddParameter("UnitKey", SqlDbType.VarChar, kegUnit.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kegUnit.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kegUnit.KDTAHUN);

                db.AddParameter("URKIN", SqlDbType.VarChar, kegUnit.URKIN);
                db.AddParameter("TARGET", SqlDbType.VarChar, kegUnit.TARGET);
                db.AddParameter("PAGU", SqlDbType.Decimal, kegUnit.PAGU);
                db.AddParameter("ANGKAS1", SqlDbType.Decimal, kegUnit.ANGKAS1);
                db.AddParameter("ANGKAS2", SqlDbType.Decimal, kegUnit.ANGKAS2);
                db.AddParameter("ANGKAS3", SqlDbType.Decimal, kegUnit.ANGKAS3);
                db.AddParameter("ANGKAS4", SqlDbType.Decimal, kegUnit.ANGKAS4);
                db.AddParameter("LOKASI", SqlDbType.VarChar, kegUnit.LOKASI);
                db.AddParameter("KET", SqlDbType.VarChar, kegUnit.KET);
                db.AddParameter("CTT", SqlDbType.VarChar, kegUnit.CTT);
                
                db.ExecuteNonQuery("sp_APBD_KegUnitUpdate");
            }
        }
              
    }
}
