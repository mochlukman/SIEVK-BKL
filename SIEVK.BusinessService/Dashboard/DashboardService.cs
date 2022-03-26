using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.Dashboard;
using SIEVK.BusinessData.Dashboard;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.Dashboard
{
    public class DashboardService
    {
        Mapper map = new Mapper();
        DashboardDAO dao = new DashboardDAO();

        public List<GrafikPenyerapan> GetPenyerapanAnggaran(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            DataTable dt = dao.PenyerapanAnggaran(kdTahap, kdTahun, kdTrwln);
            List<GrafikPenyerapan> lst = map.BindDataList<GrafikPenyerapan>(dt);
            return lst;
        }

        public List<GrafikPenyerapan> PenyerapanFisik(String kdTahap = "", String kdTahun = "", String kdTrwln = "")
        {
            DataTable dt = dao.PenyerapanFisik(kdTahap, kdTahun, kdTrwln);
            List<GrafikPenyerapan> lst = map.BindDataList<GrafikPenyerapan>(dt);
            return lst;
        }       
        
    }
}
