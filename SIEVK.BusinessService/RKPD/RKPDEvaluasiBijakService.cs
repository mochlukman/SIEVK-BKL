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
    public class RKPDEvaluasiBijakService
    {
        Mapper map = new Mapper();
        RKPDEvaluasiBijakDAO dao = new RKPDEvaluasiBijakDAO();

        public List<RKPDEvaluasiBijak> GetEvaluasiBijakList(string kdTahun)
        {
            DataTable dt = dao.GetEvaluasiBijak(kdTahun);
            List<RKPDEvaluasiBijak> lst = map.BindDataList<RKPDEvaluasiBijak>(dt);
            return lst;
        }

        public RKPDEvaluasiBijak GetEvaluasiBijak(string kdTahun, string nomor)
        {
            DataTable dt = dao.GetEvaluasiBijak(kdTahun, nomor);
            RKPDEvaluasiBijak obj = map.BindData<RKPDEvaluasiBijak>(dt);
            return obj;
        }

        public void DeleteData(string kdTahun, string nomor)
        {
            dao.Delete(kdTahun, nomor);
        }

        public void InsertData(RKPDEvaluasiBijak obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RKPDEvaluasiBijak obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateData(RKPDEvaluasiBijak clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //if (clr.NOMOR == string.Empty)
            //{
            //    dic.Add("ErrNomorEmpty", "Nomor tidak boleh kosong.");
            //    return dic;
            //}

            DataTable dt = dao.GetEvaluasiBijak(clr.KDTAHUN, clr.NOMOR);
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
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["NOMOR"].ToString().Trim() != clr.NOMOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Evaluasi sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
