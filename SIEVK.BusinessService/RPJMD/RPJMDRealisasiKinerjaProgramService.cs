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
    public class RPJMDRealisasiKinerjaProgramService
    {
        Mapper map = new Mapper();
        RPJMDRealisasiKinerjaProgramDAO dao = new RPJMDRealisasiKinerjaProgramDAO();

        public List<RPJMDKinerjaProgram> GetKinerjaProgram(string unitKey, string kdTahap, string idprgrm)
        {
            DataTable dt = dao.GetKinerjaProgram(unitKey, kdTahap, idprgrm);
            List<RPJMDKinerjaProgram> lst = map.BindDataList<RPJMDKinerjaProgram>(dt);
            return lst;
        }

        
        public List<RPJMDKinerjaProgram> GetKinerjaProgramRealisasiList(string unitKey, string kdTahap, string IDPrgrm, string kinpgrpjmkey)
        {
            DataTable dt = dao.GetKinerjaProgramRealisasi(unitKey, kdTahap, IDPrgrm, kinpgrpjmkey);
            List<RPJMDKinerjaProgram> lst = map.BindDataList<RPJMDKinerjaProgram>(dt);
            return lst;
        }

        public RPJMDKinerjaProgram GetKinerjaProgramRealisasi(string unitKey, string kdTahap, string IDPrgrm, string kinpgrpjmkey)
        {
            DataTable dt = dao.GetKinerjaProgramRealisasi(unitKey, kdTahap, IDPrgrm, kinpgrpjmkey);
            RPJMDKinerjaProgram lst = map.BindData<RPJMDKinerjaProgram>(dt);
            return lst;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string IDPrgrm,  string kinpgrpjmkey)
        {
            dao.Delete(unitKey,kdTahap,IDPrgrm,kinpgrpjmkey);
        }

        public void UpdateDataRealisasi(RPJMDKinerjaProgram program)
        {
            dao.Update(program);
        }

        public void CreateDataRealisasi(RPJMDKinerjaProgram program)
        {
            dao.Create(program);
        }
    }
}
