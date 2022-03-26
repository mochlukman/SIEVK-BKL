using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RKPD;
using SIEVK.BusinessData.RKPD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RKPD
{
    public class RKPDRealisasiProgramService
    {
        Mapper map = new Mapper();
        RKPDRealisasiProgramDAO dao = new RKPDRealisasiProgramDAO();

        public List<RKPDProgram> GetProgram(string unitKey, string kdTahap, string kdTahun)
        {
            DataTable dt = dao.GetProgram(unitKey, kdTahap, kdTahun);
            List<RKPDProgram> lst = map.BindDataList<RKPDProgram>(dt);
            return lst;
        }

        public List<RKPDProgram> GetProgramRealisasiList(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            DataTable dt = dao.GetProgramRealisasi(unitKey, kdTahap, kdTahun, IDPrgrm);
            List<RKPDProgram> lst = map.BindDataList<RKPDProgram>(dt);
            return lst;
        }

        public RKPDProgram GetProgramRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            DataTable dt = dao.GetProgramRealisasi(unitKey, kdTahap, kdTahun, IDPrgrm);
            RKPDProgram obj = map.BindData<RKPDProgram>(dt);
            return obj;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string kdTahun, string IDPrgrm)
        {
            dao.Delete(unitKey, kdTahap, kdTahun, IDPrgrm);
        }

        public void UpdateDataRealisasi(RKPDProgram program)
        {
            dao.Update(program);
        }

        public void CreateDataRealisasi(RKPDProgram program)
        {
            dao.Create(program);
        }

    }
}
