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
    public class RPJMDRealisasiProgramService
    {
        Mapper map = new Mapper();
        RPJMDRealisasiProgramDAO dao = new RPJMDRealisasiProgramDAO();

        public List<RPJMDProgram> GetProgramPagu(string unitKey, string kdTahap)
        {
            DataTable dt = dao.GetProgramPagu(unitKey, kdTahap);
            List<RPJMDProgram> lst = map.BindDataList<RPJMDProgram>(dt);
            return lst;
        }
        public RPJMDProgram GetProgramPaguById(string unitKey, string kdTahap, string idPrgrm)
        {
            DataTable dt = dao.GetProgramPaguById(unitKey, kdTahap, idPrgrm);
            List<RPJMDProgram> lst = map.BindDataList<RPJMDProgram>(dt);
            return lst[0];
        }
        public List<RPJMDProgram> GetProgramRealisasiList(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = dao.GetProgramRealisasi(unitKey, kdTahap, IDPrgrm);
            List<RPJMDProgram> lst = map.BindDataList<RPJMDProgram>(dt);
            return lst;
        }

        public RPJMDProgram GetProgramRealisasi(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = dao.GetProgramRealisasi(unitKey, kdTahap, IDPrgrm);
            RPJMDProgram lst = map.BindData<RPJMDProgram>(dt);
            return lst;
        }

        public void UpdateDataProgram(RPJMDProgram program)
        {
            dao.Updatepagu(program);
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string IDPrgrm)
        {
            dao.Delete(unitKey,kdTahap,IDPrgrm);
        }

        public void UpdateDataRealisasi(RPJMDProgram program)
        {
            dao.Update(program);
        }

        public void CreateDataRealisasi(RPJMDProgram program)
        {
            dao.Create(program);
        }
    }
}
