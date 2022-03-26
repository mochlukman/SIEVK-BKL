using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDKinerjaKegiatanDAO
    {
        public DataTable GetKinerjaKegiatanById(string unitKey, string kdTahap, string idKeg, string idKinKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDKeg", SqlDbType.VarChar, idKeg);
                db.AddParameter("KINKEGRPJMKEY", SqlDbType.VarChar, idKinKeg);

                dt = db.ExecuteDataTable("sp_RPJMD_KinerjaKegiatanByID");
            }
            return dt;
        }

        public void Update(RPJMDKinerjaKegiatan kinkegiatan)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, kinkegiatan.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kinkegiatan.KDTAHAP);
                db.AddParameter("IDKeg", SqlDbType.VarChar, kinkegiatan.IDKEG);
                db.AddParameter("KINKEGRPJMKEY", SqlDbType.VarChar, kinkegiatan.KINKEGRPJMKEY);
                db.AddParameter("URKIN", SqlDbType.VarChar, kinkegiatan.URKIN);
                db.AddParameter("KINLALU", SqlDbType.VarChar, kinkegiatan.KINLALU);
                db.AddParameter("TARGET1", SqlDbType.VarChar, kinkegiatan.TARGET1);
                db.AddParameter("TARGET2", SqlDbType.VarChar, kinkegiatan.TARGET2);
                db.AddParameter("TARGET3", SqlDbType.VarChar, kinkegiatan.TARGET3);
                db.AddParameter("TARGET4", SqlDbType.VarChar, kinkegiatan.TARGET4);
                db.AddParameter("TARGET5", SqlDbType.VarChar, kinkegiatan.TARGET5);
                db.AddParameter("TARGET6", SqlDbType.VarChar, kinkegiatan.TARGET6);
                db.AddParameter("TARGETAKHIR", SqlDbType.VarChar, kinkegiatan.TARGETAKHIR);
                db.AddParameter("KET", SqlDbType.VarChar, kinkegiatan.KET);
                db.ExecuteNonQuery("sp_RPJMD_KinerjaKegiatanUpdate");
            }
        }
    }
}
