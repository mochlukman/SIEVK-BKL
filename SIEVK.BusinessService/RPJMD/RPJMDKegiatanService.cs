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
    public class RPJMDKegiatanService
    {
        Mapper map = new Mapper();
        RPJMDKegiatanDAO dao = new RPJMDKegiatanDAO();

        public RPJMDKegiatan GetKegiatanPaguById(string unitKey, string kdTahap, string idPrgrm, String idkeg = null)
        {
            DataTable dt = dao.GetKegiatanPaguById(unitKey, kdTahap, idPrgrm, idkeg);
            List<RPJMDKegiatan> lst = map.BindDataList<RPJMDKegiatan>(dt);
            return lst[0];
        }

        public void Update(RPJMDKegiatan Kegiatan)
        {
            dao.Update(Kegiatan);
        }
    }
}
