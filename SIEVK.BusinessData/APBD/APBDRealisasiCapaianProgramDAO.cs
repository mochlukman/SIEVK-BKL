using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.APBD
{
    public class APBDRealisasiCapaianProgramDAO 
    {        
        public DataTable GetRealisasiCapaianProgramAPBD(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                dt = db.ExecuteDataTable("sp_APBD_RealisasiCapaianProgramList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);
                db.ExecuteNonQuery("sp_APBD_RealisasiCapaianProgramDelete");
            }
        }

        public void Update(APBDRealisasiCapaianProgram realisasi)
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
                db.ExecuteNonQuery("sp_APBD_RealisasiCapaianProgramUpdate");
            }
        }

        public void Create(APBDRealisasiCapaianProgram realisasi)
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
                db.ExecuteNonQuery("sp_APBD_RealisasiCapaianProgramInsert");
            }
        }

      
    }
}
