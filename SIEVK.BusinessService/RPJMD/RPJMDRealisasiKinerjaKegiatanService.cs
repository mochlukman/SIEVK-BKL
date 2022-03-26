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
    public class RPJMDRealisasiKinerjaKegiatanService
    {
        Mapper map = new Mapper();
        RPJMDRealisasiKinerjaKegiatanDAO dao = new RPJMDRealisasiKinerjaKegiatanDAO();

        public List<RPJMDKinerjaKegiatan> GetKinerjaKegiatan(string unitKey, string kdTahap, string IDKeg)
        {
            DataTable dt = dao.GetKinerjaKegiatan(unitKey, kdTahap, IDKeg);
            List<RPJMDKinerjaKegiatan> lst = map.BindDataList<RPJMDKinerjaKegiatan>(dt);
            return lst;
        }

        public List<RPJMDKinerjaKegiatan> GetKinerjaKegiatanRealisasiList(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            DataTable dt = dao.GetKinerjaKegiatanRealisasi(unitKey, kdTahap, IDKeg, kinkegrpjmkey);
            List<RPJMDKinerjaKegiatan> lst = map.BindDataList<RPJMDKinerjaKegiatan>(dt);
            return lst;
        }

        public RPJMDKinerjaKegiatan GetKinerjaKegiatanRealisasi(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            DataTable dt = dao.GetKinerjaKegiatanRealisasi(unitKey, kdTahap, IDKeg, kinkegrpjmkey);
            RPJMDKinerjaKegiatan lst = map.BindData<RPJMDKinerjaKegiatan>(dt);
            return lst;
        }

        public void DeleteDataRealisasi(string unitKey, string kdTahap, string IDKeg, string kinkegrpjmkey)
        {
            dao.Delete(unitKey, kdTahap, IDKeg, kinkegrpjmkey);
        }

        public void UpdateDataRealisasi(RPJMDKinerjaKegiatan program)
        {
            dao.Update(program);
        }

        public void CreateDataRealisasi(RPJMDKinerjaKegiatan program)
        {
            dao.Create(program);
        }
    }
}
