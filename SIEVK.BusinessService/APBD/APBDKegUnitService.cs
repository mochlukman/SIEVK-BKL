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
    public class APBDKegUnitService
    {
        Mapper map = new Mapper();
        APBDKegUnitDAO dao = new APBDKegUnitDAO();

        public APBDKinerja GetKinerjaAPBDById(string unitKey, string kdTahap, string kdTahun, string idKeg)
        {
            DataTable dt = dao.GetbyId(unitKey, kdTahap, kdTahun, idKeg);
            List<APBDKinerja> lst = map.BindDataList<APBDKinerja>(dt);
            return lst[0];
        }

        public void UpdateKegUnit(APBDKinerja obj)
        {
            dao.Update(obj);
        }

    }
}
