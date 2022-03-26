using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Mapping;

namespace SIEVK.BusinessData.Mapping
{
    public class MappingKegiatan13_90DAO
    {
        public DataTable GetKegiatanMappingList(string idKeg)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG", SqlDbType.VarChar, idKeg);
                dt = db.ExecuteDataTable("sp_MAPPING_Kegiatan_13_90List");
            }
            return dt;
        }

        public DataTable GetKegiatanMappingByID(string idKeg, string idKEG90)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG", SqlDbType.VarChar, idKeg);
                db.AddParameter("IDKEG90", SqlDbType.VarChar, idKEG90);
                dt = db.ExecuteDataTable("sp_MAPPING_Kegiatan_13_90ByID");
            }
            return dt;
        }

        public void Delete(string idKeg, string idKEG90)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG", SqlDbType.VarChar, idKeg);
                db.AddParameter("IDKEG90", SqlDbType.VarChar, idKEG90);
                db.ExecuteNonQuery("sp_MAPPING_Kegiatan_13_90Delete");
            }
        }

        public void Update(MappingKegiatan13_90 keg13_90)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG", SqlDbType.VarChar, keg13_90.IDKEG);
                db.AddParameter("IDKEG90", SqlDbType.VarChar, keg13_90.IDKEG90);
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, keg13_90.KDTAHUN);
                db.AddParameter("IDKEG90_Prev", SqlDbType.VarChar, keg13_90.IDKEG90_Prev);
                db.ExecuteNonQuery("sp_MAPPING_Kegiatan_13_90Update");
            }
        }

        public void Create(MappingKegiatan13_90 keg13_90)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG", SqlDbType.VarChar, keg13_90.IDKEG);
                db.AddParameter("IDKEG90", SqlDbType.VarChar, keg13_90.IDKEG90);
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, keg13_90.KDTAHUN);
                db.ExecuteNonQuery("sp_MAPPING_Kegiatan_13_90Create");
            }
        }
    }
}
