using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.RPJMD;

namespace SIEVK.BusinessData.RPJMD
{
    public class RPJMDProgramDAO
    {
        public DataTable GetProgramPaguById(string unitKey, string kdTahap, string idPrgrm)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahap", SqlDbType.VarChar, kdTahap);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, idPrgrm);

                dt = db.ExecuteDataTable("sp_RPJMD_ProgramById");
            }
            return dt;
        }

        public void Update(RPJMDProgram program)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, program.UNITKEY);
                db.AddParameter("KDTahap", SqlDbType.VarChar, program.KDTAHAP);
                db.AddParameter("IDPrgrm", SqlDbType.VarChar, program.IDPRGRM);
                db.AddParameter("PAGU1", SqlDbType.Decimal, program.PAGU1);
                db.AddParameter("PAGU2", SqlDbType.Decimal, program.PAGU2);
                db.AddParameter("PAGU3", SqlDbType.Decimal, program.PAGU3);
                db.AddParameter("PAGU4", SqlDbType.Decimal, program.PAGU4);
                db.AddParameter("PAGU5", SqlDbType.Decimal, program.PAGU5);
                db.AddParameter("PAGU6", SqlDbType.Decimal, program.PAGU6);
                db.AddParameter("PAGUAKHIR", SqlDbType.Decimal, program.PAGUAKHIR);
                db.ExecuteNonQuery("sp_RPJMD_ProgramUpdate");
            }
        }
    }
}
