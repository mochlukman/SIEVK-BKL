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
    public class APBDKinKegUnitService
    {
        Mapper map = new Mapper();
        APBDKinKegUnitDAO dao = new APBDKinKegUnitDAO();

        public List<APBDKinKegUnit> GetKinKegUnitList(string unitKey, string kdTahap, string kdTahun, string idKeg)
        {
            DataTable dt = dao.GetKinKegUnitList(unitKey, kdTahap, kdTahun, idKeg);
            List<APBDKinKegUnit> lst = map.BindDataList<APBDKinKegUnit>(dt);
            return lst;
        }

        public APBDKinKegUnit GetKinKegUnitbyId(string unitKey, string kdTahap, string kdTahun, string idKeg, string noUrkin)
        {
            DataTable dt = dao.GetKinKegUnitbyId(unitKey, kdTahap, kdTahun, idKeg, noUrkin);

            APBDKinKegUnit obj = map.BindData<APBDKinKegUnit>(dt);
            return obj;
        }

        public void Delete(string unitKey, string kdTahap, string kdTahun, string idKeg, string noUrkin)
        {
            dao.Delete(idKeg, kdTahun, kdTahap, unitKey, noUrkin);
        }

        public void Update(APBDKinKegUnit obj)
        {
            dao.Update(obj);
        }

        public void Create(APBDKinKegUnit obj)
        {
            dao.Create(obj);
        }
    }
}
