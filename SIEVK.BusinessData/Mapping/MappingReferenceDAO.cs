using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Mapping;

namespace SIEVK.BusinessData.Mapping
{
    public class MappingReferenceDAO
    {
        public DataTable GetMPGRMList(string idPRGRM = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDPRGRM", SqlDbType.VarChar, idPRGRM);
                dt = db.ExecuteDataTable("sp_MPGRMList");
            }
            return dt;
        }

        public DataTable GetMPGRM90List(string idPRGRM90 = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDPRGRM90", SqlDbType.VarChar, idPRGRM90);
                dt = db.ExecuteDataTable("sp_MPGRM90List");
            }
            return dt;
        }

        public DataTable GetMPGRM50List(string idPRGRM050 = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDPRGRM050", SqlDbType.VarChar, idPRGRM050);
                dt = db.ExecuteDataTable("sp_MPGRM050List");
            }
            return dt;
        }

        public DataTable GetMKEGIATANList(string idPRGRM, string idKEG = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDPRGRM", SqlDbType.VarChar, idPRGRM);
                db.AddParameter("IDKEG", SqlDbType.VarChar, idKEG);
                dt = db.ExecuteDataTable("sp_MKEGIATANList");
            }
            return dt;
        }

        public DataTable GetMKEGIATAN90List(string idPRGRM90 = "", string idKEG90 = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDPRGRM90", SqlDbType.VarChar, idPRGRM90);
                db.AddParameter("IDKEG90", SqlDbType.VarChar, idKEG90);
                dt = db.ExecuteDataTable("sp_MKEGIATAN90List");
            }
            return dt;
        }

        public DataTable GetMKEGIATAN50List(string idPRGRM050 = "", string idKEG050 = "")
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("IDPRGRM050", SqlDbType.VarChar, idPRGRM050);
                db.AddParameter("IDKEG050", SqlDbType.VarChar, idKEG050);
                dt = db.ExecuteDataTable("sp_MKEGIATAN050List");
            }
            return dt;
        }
    }
}
