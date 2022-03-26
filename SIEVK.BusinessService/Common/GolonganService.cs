using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.Common;
using SIEVK.BusinessData.Common;
using SIEVK.BusinessData;
using System.Data;

namespace SIEVK.BusinessService.Common
{
    public class GolonganService
    {
        Mapper map = new Mapper();
        GolonganDAO dao = new GolonganDAO();

        public Golongan GetGolongan (String nama)
        {
            DataTable dt = dao.GetList(nama);
            Golongan usr = null;
            if (dt.Rows.Count > 0)
            {
                usr = map.BindData<Golongan>(dt);
            }
            return usr;
        }

        public List<Golongan> GetAllGolongan()
        {
            DataTable dt = dao.GetList();
            List<Golongan> lst = map.BindDataList<Golongan>(dt);
            return lst;
        }
    }
}
