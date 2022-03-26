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
    public class RPJMDKinerjaProgramService
    {
        Mapper map = new Mapper();
        RPJMDKinerjaProgramDAO dao = new RPJMDKinerjaProgramDAO();

        public List<RPJMDKinerjaProgram> GetKinerjaProgram(string unitKey, string kdTahap, string idprgrm)
        {
            DataTable dt = dao.GetKinerjaProgram(unitKey, kdTahap, idprgrm);
            List<RPJMDKinerjaProgram> lst = map.BindDataList<RPJMDKinerjaProgram>(dt);
            return lst;
        }


        public List<RPJMDKinerjaProgram> GetKinerjaProgramList(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = dao.GetKinerjaProgram(unitKey, kdTahap, IDPrgrm);
            List<RPJMDKinerjaProgram> lst = map.BindDataList<RPJMDKinerjaProgram>(dt);
            return lst;
        }

        public RPJMDKinerjaProgram GetListKinerjaProgram(string unitKey, string kdTahap, string IDPrgrm)
        {
            DataTable dt = dao.GetKinerjaProgram(unitKey, kdTahap, IDPrgrm);
            RPJMDKinerjaProgram lst = map.BindData<RPJMDKinerjaProgram>(dt);
            return lst;
        }

        public void DeleteDataKinpgrm(string unitKey, string kdTahap, string IDPrgrm, string kinpgrpjmkey)
        {
            dao.Delete(unitKey, kdTahap, IDPrgrm, kinpgrpjmkey);
        }

        public void UpdateDataKinpgrm(RPJMDKinerjaProgram program)
        {
            dao.Update(program);
        }

        public void CreateDataKinpgrm(RPJMDKinerjaProgram program)
        {
            dao.Create(program);
        }
    }
}
