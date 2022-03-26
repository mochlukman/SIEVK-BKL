using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Mapping;

namespace SIEVK.BusinessData.Mapping
{
    public class MappingKegiatan90_50DAO
    {
        public DataTable GetKegiatanMappingList(string idKEG90)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG90", SqlDbType.VarChar, idKEG90);
                dt = db.ExecuteDataTable("sp_MAPPING_Kegiatan_90_50List");
            }
            return dt;
        }

        public DataTable GetKegiatanMappingByID(string idKEG90, string idKEG050)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG90", SqlDbType.VarChar, idKEG90);
                db.AddParameter("IDKEG050", SqlDbType.VarChar, idKEG050);
                dt = db.ExecuteDataTable("sp_MAPPING_Kegiatan_90_50ByID");
            }
            return dt;
        }

        public void Delete(string idKEG90, string idKEG050)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG90", SqlDbType.VarChar, idKEG90);
                db.AddParameter("IDKEG050", SqlDbType.VarChar, idKEG050);
                db.ExecuteNonQuery("sp_MAPPING_Kegiatan_90_50Delete");
            }
        }

        public void Update(MappingKegiatan90_50 keg90_50)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG90", SqlDbType.VarChar, keg90_50.IDKEG90);
                db.AddParameter("IDKEG050", SqlDbType.VarChar, keg90_50.IDKEG050);
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, keg90_50.KDTAHUN);
                db.AddParameter("IDKEG050_Prev", SqlDbType.VarChar, keg90_50.IDKEG050_Prev);
                db.ExecuteNonQuery("sp_MAPPING_Kegiatan_90_50Update");
            }
        }

        public void Create(MappingKegiatan90_50 keg90_50)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDKEG90", SqlDbType.VarChar, keg90_50.IDKEG90);
                db.AddParameter("IDKEG050", SqlDbType.VarChar, keg90_50.IDKEG050);
                db.AddParameter("KDTAHUN", SqlDbType.VarChar, keg90_50.KDTAHUN);
                db.ExecuteNonQuery("sp_MAPPING_Kegiatan_90_50Create");
            }
        }
    }
}
