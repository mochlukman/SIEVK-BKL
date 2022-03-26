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
    public class RKPDCatatanLaporanRealisasiPemdaService
    {
        Mapper map = new Mapper();
        RKPDCatatanLaporanRealisasiPemdaDAO dao = new RKPDCatatanLaporanRealisasiPemdaDAO();

        public List<RKPDCatatanLaporanRealisasi> GetCatatanLaporanRealisasiList(string kdTahun, string kdTriwulan)
        {
            DataTable dt = dao.GetCatatanLaporanRealisasi(kdTahun, kdTriwulan);
            List<RKPDCatatanLaporanRealisasi> lst = map.BindDataList<RKPDCatatanLaporanRealisasi>(dt);
            return lst;
        }

        public RKPDCatatanLaporanRealisasi GetCatatanLaporanRealisasi(string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            DataTable dt = dao.GetCatatanLaporanRealisasi(kdTahun, kdTriwulan, kdFAKTOR);
            RKPDCatatanLaporanRealisasi obj = map.BindData<RKPDCatatanLaporanRealisasi>(dt);
            return obj;
        }

        public void DeleteData(string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            dao.Delete(kdTahun, kdTriwulan, kdFAKTOR);
        }

        public void InsertData(RKPDCatatanLaporanRealisasi obj)
        {
            dao.Create(obj);
        }

        public void UpdateData(RKPDCatatanLaporanRealisasi obj)
        {
            dao.Update(obj);
        }

        public Dictionary<string, string> ValidateDataCatatan(RKPDCatatanLaporanRealisasi clr, bool isCreate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            DataTable dt = dao.GetCatatanLaporanRealisasi(clr.KDTAHUN, clr.KDTRWN, clr.KDFAKTOR);
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
                    if (dt.Rows[0]["KDTAHUN"].ToString().Trim() != clr.KDTAHUN.Trim() || dt.Rows[0]["KDTRWN"].ToString().Trim() != clr.KDTRWN.Trim() || dt.Rows[0]["KDFAKTOR"].ToString().Trim() != clr.KDFAKTOR.Trim())
                    {
                        dic.Add("ErrNomorCttExist", "Faktor sudah ada.");
                    }
                }
            }

            return dic;
        }


    }
}
