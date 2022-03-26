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
    public class RPJMDKinerjaKegiatanService
    {
        Mapper map = new Mapper();
        RPJMDKinerjaKegiatanDAO dao = new RPJMDKinerjaKegiatanDAO();

        public RPJMDKinerjaKegiatan GetKinerjaKegiatanById(string unitKey, string kdTahap, string idKeg, string idKinKeg)
        {
            DataTable dt = dao.GetKinerjaKegiatanById(unitKey, kdTahap, idKeg, idKinKeg);
            List<RPJMDKinerjaKegiatan> lst = map.BindDataList<RPJMDKinerjaKegiatan>(dt);
            return lst[0];
        }

        public void Update(RPJMDKinerjaKegiatan kinKegiatan)
        {
            dao.Update(kinKegiatan);
        }
    }
}
