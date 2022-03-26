using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.APBD;
using SIEVK.BusinessData.APBD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.APBD
{
    public class APBDRealisasiCapaianProgramService
    {
        Mapper map = new Mapper();
        APBDRealisasiCapaianProgramDAO dao = new APBDRealisasiCapaianProgramDAO();

        public List<APBDRealisasiCapaianProgram> GetRealisasiCapaianProgramAPBDList(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = dao.GetRealisasiCapaianProgramAPBD(unitKey, kdTahap, kdTahun, idPrgrm);
            List<APBDRealisasiCapaianProgram> lst = map.BindDataList<APBDRealisasiCapaianProgram>(dt);
            return lst;
        }

        public APBDRealisasiCapaianProgram GetRealisasiCapaianProgramAPBD(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = dao.GetRealisasiCapaianProgramAPBD(unitKey, kdTahap, kdTahun, idPrgrm);
            APBDRealisasiCapaianProgram obj = map.BindData<APBDRealisasiCapaianProgram>(dt);
            return obj;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            dao.Delete(unitKey, kdTahap, kdTahun,idPrgrm);
        }

        public void UpdateDataRealisasi(APBDRealisasiCapaianProgram obj)
        {
            dao.Update(obj);
        }

        public void CreateDataRealisasi(APBDRealisasiCapaianProgram obj)
        {
            dao.Create(obj);
        }

    }
}
