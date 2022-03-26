using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.RKPD;
using System.Data;

namespace SIEVK.BusinessData.RKPD
{
    public class RKPDRealisasiProgramDAO
    {        
        public DataTable GetProgram(string unitKey, string kdTahap, string kdTahun)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                dt = db.ExecuteDataTable("sp_RKPD_ProgramPaguList");
            }
            return dt;
        }

        public DataTable GetProgramRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                dt = db.ExecuteDataTable("sp_RKPD_ProgramRealisasiList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.ExecuteNonQuery("sp_RKPD_ProgramRealisasiDelete");
            }
        }

        public void Update(RKPDProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("Real1", SqlDbType.Decimal, realisasi.REAL1);
                db.AddParameter("Real2", SqlDbType.Decimal, realisasi.REAL2);
                db.AddParameter("Real3", SqlDbType.Decimal, realisasi.REAL3);
                db.AddParameter("Real4", SqlDbType.Decimal, realisasi.REAL4);
                db.AddParameter("RealAkhir", SqlDbType.Decimal, realisasi.REALAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RKPD_ProgramRealisasiUpdate");
            }
        }

        public void Create(RKPDProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("Real1", SqlDbType.Decimal, realisasi.REAL1);
                db.AddParameter("Real2", SqlDbType.Decimal, realisasi.REAL2);
                db.AddParameter("Real3", SqlDbType.Decimal, realisasi.REAL3);
                db.AddParameter("Real4", SqlDbType.Decimal, realisasi.REAL4);
                db.AddParameter("RealAkhir", SqlDbType.Decimal, realisasi.REALAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RKPD_ProgramRealisasiInsert");
            }
        }

      
    }
}
