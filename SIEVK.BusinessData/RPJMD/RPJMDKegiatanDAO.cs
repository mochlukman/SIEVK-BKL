using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDKegiatanDAO
    {
        public DataTable GetKegiatanPaguById(string unitKey, string kdTahap, string idPrgrm, string idKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                db.AddParameter("IDKeg", SqlDbType.VarChar, idKeg);

                dt = db.ExecuteDataTable("sp_RPJMD_KegiatanPaguById");
            }
            return dt;
        }

        public void Update(RPJMDKegiatan kegiatan)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, kegiatan.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kegiatan.KDTAHAP);
                db.AddParameter("IDKeg", SqlDbType.VarChar, kegiatan.IDKEG);
                db.AddParameter("PAGULALU", SqlDbType.Decimal, kegiatan.PAGULALU);
                db.AddParameter("PAGU1", SqlDbType.Decimal, kegiatan.PAGU1);
                db.AddParameter("PAGU2", SqlDbType.Decimal, kegiatan.PAGU2);
                db.AddParameter("PAGU3", SqlDbType.Decimal, kegiatan.PAGU3);
                db.AddParameter("PAGU4", SqlDbType.Decimal, kegiatan.PAGU4);
                db.AddParameter("PAGU5", SqlDbType.Decimal, kegiatan.PAGU5);
                db.AddParameter("PAGU6", SqlDbType.Decimal, kegiatan.PAGU6);
                db.AddParameter("PAGUAKHIR", SqlDbType.Decimal, kegiatan.PAGUAKHIR);
                db.ExecuteNonQuery("sp_RPJMD_KegiatanPaguUpdate");
            }
        }
    }
}
