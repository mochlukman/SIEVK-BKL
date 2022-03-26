using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.RENJA;
using SIEVK.BusinessData.RENJA;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.RENJA
{
    public class RENJAEvaluasiBijakService
    {
        Mapper map = new Mapper();
        RENJAEvaluasiBijakDAO dao = new RENJAEvaluasiBijakDAO();

        public List<RENJAEvaluasiBijak> GetEvaluasiBijakList(string kdTahun, string kdUnit)
        {
            DataTable dt = dao.GetEvaluasiBijak(kdTahun, kdUnit);
            List<RENJAEvaluasiBijak> lst = map.BindDataList<RENJAEvaluasiBijak>(dt);
            return lst;
        }

        public RENJAEvaluasiBijak GetEvaluasiBijak(string kdTahun, string kdUnit, string nomor)
        {
            DataTable dt = dao.GetEvaluasiBijak(kdTahun, kdUnit, nomor);
            RENJAEvaluasiBijak obj = map.BindData<RENJAEvaluasiBijak>(dt);
            return obj;
        }

        public void DeleteData(string kdTahun, string kdUnit, string nomor)
        {
            dao.Delete(kdTahun, kdUnit, nomor);
        }

        public void InsertData(RENJAEvaluasiBijak obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RENJAEvaluasiBijak obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RENJAEvaluasiBijak clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiBijak(clr.KDTAHUN, clr.UNITKEY, clr.NOMOR);
            if (isCreate)
            {
                if (dt.Rows.Count > 0)
                {
                    dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["UNITKEY"].ToString().Trim() != clr.UNITKEY.Trim() || dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
