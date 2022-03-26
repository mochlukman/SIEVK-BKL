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
    public class RKPDRealisasiKinerjaKegiatanService
    {
        Mapper map = new Mapper();
        RKPDRealisasiKinerjaKegiatanDAO dao = new RKPDRealisasiKinerjaKegiatanDAO();

        public List<RKPDKinerjaKegiatan> GetKinerjaKegiatan(string unitKey, string kdTahap, string kdTahun, string idPrgrm)
        {
            DataTable dt = dao.GetKinerjaKegiatan(unitKey, kdTahap, kdTahun, idPrgrm);
            List<RKPDKinerjaKegiatan> lst = map.BindDataList<RKPDKinerjaKegiatan>(dt);
            return lst;
        }

        public List<RKPDKinerjaKegiatan> GetKinerjaKegiatanRealisasiList(string unitKey, string kdTahap, string kdTahun, string idKeg)
        {
            DataTable dt = dao.GetKinerjaKegiatanRealisasi(unitKey, kdTahap, kdTahun, idKeg);
            List<RKPDKinerjaKegiatan> lst = map.BindDataList<RKPDKinerjaKegiatan>(dt);
            return lst;
        }

        public RKPDKinerjaKegiatan GetKinerjaKegiatanRealisasi(string unitKey, string kdTahap, string kdTahun, string idKeg, string kdTrwn)
        {
            DataTable dt = dao.GetKinerjaKegiatanRealisasi(unitKey, kdTahap, kdTahun, idKeg,kdTrwn);
            RKPDKinerjaKegiatan obj = map.BindData<RKPDKinerjaKegiatan>(dt);
            return obj;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string kdTahun, string idKeg, string kdTrwn)
        {
            dao.Delete(unitKey, kdTahap, kdTahun, idKeg, kdTrwn);
        }

        public void UpdateDataRealisasi(RKPDKinerjaKegiatan obj)
        {
            dao.Update(obj);
        }

        public void CreateDataRealisasi(RKPDKinerjaKegiatan obj)
        {
            dao.Create(obj);
        }

    }
}
