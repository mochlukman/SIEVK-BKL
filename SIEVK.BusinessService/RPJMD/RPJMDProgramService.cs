using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RPJMD;
using SIEVK.BusinessData.RPJMD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RPJMD
{
    public class RPJMDProgramService
    {
        Mapper map = new Mapper();
        RPJMDProgramDAO dao = new RPJMDProgramDAO();

        public RPJMDProgram GetProgramPaguById(string unitKey, string kdTahap, string idPrgrm)
        {
            DataTable dt = dao.GetProgramPaguById(unitKey, kdTahap, idPrgrm);
            List<RPJMDProgram> lst = map.BindDataList<RPJMDProgram>(dt);
            return lst[0];
        }

        public void Update(RPJMDProgram Program)
        {
            dao.Update(Program);
        }
    }
}
