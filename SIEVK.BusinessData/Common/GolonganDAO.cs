using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIEVK.Domain.Common;

namespace SIEVK.BusinessData.Common
{
    public class GolonganDAO
    {
        public DataTable GetList(String nama = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("PANGKAT", SqlDbType.VarChar, nama);
                dt = db.ExecuteDataTable("[sp_KDGOLList]");
            }
            return dt;
        }
    }
}
