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
    public class APBDRealisasiKinerjaAPBDService
    {
        Mapper map = new Mapper();
        APBDRealisasiKinerjaAPBDDAO dao = new APBDRealisasiKinerjaAPBDDAO();

        public List<APBDKinerja> GetKinerjaAPBD(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = dao.GetKinerjaAPBD(unitKey, kdTahap, kdTahun, idPrgrm);
            List<APBDKinerja> lst = map.BindDataList<APBDKinerja>(dt);
            return lst;
        }

        public List<APBDKinerja> GetKinerjaAPBDRealisasiList(string unitKey, string kdTahap, string kdTahun, string idKeg)
        {
            DataTable dt = dao.GetKinerjaAPBDRealisasi(unitKey, kdTahap, kdTahun, idKeg);
            List<APBDKinerja> lst = map.BindDataList<APBDKinerja>(dt);
            return lst;
        }

        public APBDKinerja GetKinerjaAPBDRealisasi(string unitKey, string kdTahap, string kdTahun, string idKeg, string kdTrwn)
        {
            DataTable dt = dao.GetKinerjaAPBDRealisasi(unitKey, kdTahap, kdTahun, idKeg, kdTrwn);
            APBDKinerja obj = map.BindData<APBDKinerja>(dt);
            return obj;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string kdTahun, string idKeg, string kdTrwn)
        {
            dao.Delete(unitKey, kdTahap, kdTahun, idKeg, kdTrwn);
        }

        public void UpdateDataRealisasi(APBDKinerja obj)
        {
            dao.Update(obj);
        }

        public void CreateDataRealisasi(APBDKinerja obj)
        {
            dao.Create(obj);
        }

    }
}
