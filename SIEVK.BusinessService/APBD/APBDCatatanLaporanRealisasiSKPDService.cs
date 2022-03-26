using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.APBD;
using SIEVK.BusinessData.APBD;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.APBD
{
    public class APBDCatatanLaporanRealisasiSKPDService
    {
        Mapper map = new Mapper();
        APBDCatatanLaporanRealisasiSKPDDAO dao = new APBDCatatanLaporanRealisasiSKPDDAO();

        public List<APBDCatatanLaporanRealisasi> GetCatatanLaporanRealisasiList(string unitKey, string kdTahun, string kdTriwulan)
        {
            DataTable dt = dao.GetCatatanLaporanRealisasi(unitKey, kdTahun, kdTriwulan);
            List<APBDCatatanLaporanRealisasi> lst = map.BindDataList<APBDCatatanLaporanRealisasi>(dt);
            return lst;
        }

        public APBDCatatanLaporanRealisasi GetCatatanLaporanRealisasi(string unitKey, string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            DataTable dt = dao.GetCatatanLaporanRealisasi(unitKey, kdTahun, kdTriwulan, kdFAKTOR);
            APBDCatatanLaporanRealisasi obj = map.BindData<APBDCatatanLaporanRealisasi>(dt);
            return obj;
        }

        public void DeleteData(string unitKey, string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            dao.Delete(unitKey, kdTahun, kdTriwulan, kdFAKTOR);
        }

        public void InsertData(APBDCatatanLaporanRealisasi obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(APBDCatatanLaporanRealisasi obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateDataCatatan(APBDCatatanLaporanRealisasi clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            DataTable dt = dao.GetCatatanLaporanRealisasi(clr.UNITKEY, clr.KDTAHUN, clr.KDTRWN, clr.KDFAKTOR);
            if (isCreate)
            {
                if (dt.Rows.Count > 0)
                {
                    dic.Add("ErrNomorCttExist", "Faktor sudah ada.");
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["UNITKEY"].ToString().Trim() != clr.UNITKEY.Trim() || dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["KDTRWN"].ToString().Trim() != clr.KDTRWN.Trim() || dt.Rows[0]["KDFAKTOR"].ToString().Trim() != clr.KDFAKTOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Faktor sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
