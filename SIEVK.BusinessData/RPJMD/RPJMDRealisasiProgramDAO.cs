using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDRealisasiProgramDAO
    {
        public DataTable GetProgramPagu(string unitKey, string kdTahap)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                dt = db.ExecuteDataTable("sp_RPJMD_ProgramPaguList");
            }
            return dt;
        }
        public DataTable GetProgramPaguById(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);

                dt = db.ExecuteDataTable("sp_RPJMD_ProgramById");
            }
            return dt;
        }
        public DataTable GetProgramRealisasi(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                dt = db.ExecuteDataTable("sp_RPJMD_ProgramRealisasiList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, IDPrgrm);
                db.ExecuteNonQuery("sp_RPJMD_ProgramRealisasiDelete");
            }
        }
        public void Updatepagu(RPJMDProgram pagu)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, pagu.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, pagu.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, pagu.IDPRGRM);
                db.AddParameter("PAGU1", SqlDbType.Decimal, pagu.PAGU1);
                db.AddParameter("PAGU2", SqlDbType.Decimal, pagu.PAGU2);
                db.AddParameter("PAGU3", SqlDbType.Decimal, pagu.PAGU3);
                db.AddParameter("PAGU4", SqlDbType.Decimal, pagu.PAGU4);
                db.AddParameter("PAGU5", SqlDbType.Decimal, pagu.PAGU5);
                db.AddParameter("PAGU6", SqlDbType.Decimal, pagu.PAGU6);
                db.AddParameter("PAGUAKHIR", SqlDbType.Decimal, pagu.PAGUAKHIR);
                db.AddParameter("KET", SqlDbType.VarChar, pagu.KET);
                db.AddParameter("CTT", SqlDbType.VarChar, pagu.CTT);
                db.ExecuteNonQuery("sp_RPJMD_ProgramUpdate");
            }
        }
        public void Update(RPJMDProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("Real1", SqlDbType.Decimal, realisasi.REAL1);
                db.AddParameter("Real2", SqlDbType.Decimal, realisasi.REAL2);
                db.AddParameter("Real3", SqlDbType.Decimal, realisasi.REAL3);
                db.AddParameter("Real4", SqlDbType.Decimal, realisasi.REAL4);
                db.AddParameter("Real5", SqlDbType.Decimal, realisasi.REAL5);
                db.AddParameter("Real6", SqlDbType.Decimal, realisasi.REAL6);
                db.AddParameter("RealAkhir", SqlDbType.Decimal, realisasi.REALAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_ProgramRealisasiUpdate");
            }
        }

        public void Create(RPJMDProgram realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, realisasi.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, realisasi.IDPRGRM);
                db.AddParameter("Real1", SqlDbType.Decimal, realisasi.REAL1);
                db.AddParameter("Real2", SqlDbType.Decimal, realisasi.REAL2);
                db.AddParameter("Real3", SqlDbType.Decimal, realisasi.REAL3);
                db.AddParameter("Real4", SqlDbType.Decimal, realisasi.REAL4);
                db.AddParameter("Real5", SqlDbType.Decimal, realisasi.REAL5);
                db.AddParameter("Real6", SqlDbType.Decimal, realisasi.REAL6);
                db.AddParameter("RealAkhir", SqlDbType.Decimal, realisasi.REALAKHIR);
                db.AddParameter("Ket", SqlDbType.VarChar, realisasi.KET);
                db.AddParameter("Ctt", SqlDbType.VarChar, realisasi.CTT);
                db.ExecuteNonQuery("sp_RPJMD_ProgramRealisasiInsert");
            }
        }

    }
}
