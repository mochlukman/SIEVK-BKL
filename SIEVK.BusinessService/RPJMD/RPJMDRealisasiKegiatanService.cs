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
    public class RPJMDRealisasiKegiatanService
    {
        Mapper map = new Mapper();
        RPJMDRealisasiKegiatanDAO dao = new RPJMDRealisasiKegiatanDAO();

        public List<RPJMDKegiatan> GetKegiatanPagu(string unitKey, string kdTahap, string idPrgrm, String idkeg = null)
        {
            DataTable dt = dao.GetKegiatanPagu(unitKey, kdTahap, idPrgrm, idkeg);
            List<RPJMDKegiatan> lst = map.BindDataList<RPJMDKegiatan>(dt);
            return lst;
        }

        public List<RPJMDKegiatan> GetKegiatanRealisasiList(string unitKey, string kdTahap, string IDKeg)
        {
            DataTable dt = dao.GetKegiatanRealisasi(unitKey, kdTahap, IDKeg);
            List<RPJMDKegiatan> lst = map.BindDataList<RPJMDKegiatan>(dt);
            return lst;
        }

        public RPJMDKegiatan GetKegiatanRealisasi(string unitKey, string kdTahap, string IDKeg)
        {
            DataTable dt = dao.GetKegiatanRealisasi(unitKey, kdTahap, IDKeg);
            RPJMDKegiatan lst = map.BindData<RPJMDKegiatan>(dt);
            return lst;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string IDKeg)
        {
            dao.Delete(unitKey, kdTahap, IDKeg);
        }

        public void UpdateDataRealisasi(RPJMDKegiatan Kegiatan)
        {
            dao.Update(Kegiatan);
        }

        public void CreateDataRealisasi(RPJMDKegiatan Kegiatan)
        {
            dao.Create(Kegiatan);
        }
    }
}
