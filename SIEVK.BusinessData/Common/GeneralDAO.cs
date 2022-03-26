using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;
using System.Data;

namespace SIEVK.BusinessData.Common
{
    public class GeneralDAO
    {
        public void InsertError(ErrorLog error)
        {
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("ErrorMessage", SqlDbType.NVarChar, error.ErrorMessage);
                db.AddParameter("InnerException", SqlDbType.NVarChar, error.InnerException);
                db.AddParameter("ExceptionStackTrace", SqlDbType.NVarChar, error.ExceptionStackTrace);
                db.AddParameter("CreatedBy", SqlDbType.VarChar, error.CreatedBy);
                db.ExecuteNonQuery("sp_ErrorLogInsert");
            }
        }

        public DataTable GetUnitOrganization(string unitKey = null, string kdLevel = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KdLevel", SqlDbType.VarChar, kdLevel);
                dt = db.ExecuteDataTable("[sp_UnitOrganizationList]");
            }
            return dt;
        }

        public DataTable GetTahapan(string key)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("Key", SqlDbType.VarChar, key);
                dt = db.ExecuteDataTable("[sp_TahapanList]");
            }
            return dt;
        }

        public DataTable GetTahunAnggaran()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("[sp_TahunAnggaranList]");
            }
            return dt;
        }

        public DataTable GetProgramRPJMD(string unitkey, string kdtahapan, string idprgrm = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitkey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdtahapan);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idprgrm);
                dt = db.ExecuteDataTable("sp_RPJMD_ProgramList");
            }
            return dt;
        }

        public DataTable GetProgramRKPD(string unitKey, string IDPrgrm = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                dt = db.ExecuteDataTable("[sp_RKPD_ProgramList]");
            }
            return dt;
        }

        public DataTable GetProgramAPBD(string unitKey, string kdTahun, string idPrgrm = null, string kdTahap= null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                dt = db.ExecuteDataTable("[sp_APBD_ProgramList]");
            }
            return dt;
        }

        public DataTable GetTriwulan()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("[sp_General_TriwulanList]");
            }
            return dt;
        }

        public DataTable GetJFaktor()
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                dt = db.ExecuteDataTable("[sp_RKPD_JFaktor]");
            }
            return dt;
        }

        

    }
}
