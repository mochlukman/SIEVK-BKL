using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessData.Common
{
    public class PegawaiDAO
    {
        public DataTable GetList(String nip = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NIP", SqlDbType.VarChar, nip);
                dt = db.ExecuteDataTable("[sp_PegawaiList]");
            }
            return dt;
        }

        public void Insert(Pegawai pegawai)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NIP", SqlDbType.VarChar, pegawai.NIP);
                db.AddParameter("UnitKey", SqlDbType.VarChar, pegawai.UnitKey);
                db.AddParameter("KDGOL", SqlDbType.VarChar, pegawai.KDGOL);
                db.AddParameter("NAMA", SqlDbType.VarChar, pegawai.NAMA);
                db.AddParameter("ALAMAT", SqlDbType.VarChar, pegawai.ALAMAT);
                db.AddParameter("JABATAN", SqlDbType.VarChar, pegawai.JABATAN);
                db.AddParameter("PDDK", SqlDbType.VarChar, pegawai.PDDK);
                db.ExecuteNonQuery("[sp_PegawaiInsert]");
            }
        }

        public void Update(Pegawai pegawai)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NIP", SqlDbType.VarChar, pegawai.NIP);
                db.AddParameter("UnitKey", SqlDbType.VarChar, pegawai.UnitKey);
                db.AddParameter("KDGOL", SqlDbType.VarChar, pegawai.KDGOL);
                db.AddParameter("NAMA", SqlDbType.VarChar, pegawai.NAMA);
                db.AddParameter("ALAMAT", SqlDbType.VarChar, pegawai.ALAMAT);
                db.AddParameter("JABATAN", SqlDbType.VarChar, pegawai.JABATAN);
                db.AddParameter("PDDK", SqlDbType.VarChar, pegawai.PDDK);
                db.ExecuteNonQuery("sp_PegawaiUpdate");
            }
        }

        public void Delete(Pegawai pegawai)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("NIP", SqlDbType.VarChar, pegawai.NIP);
                db.ExecuteNonQuery("sp_PegawaiDelete");
            }
        }

    }
}
