﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.Domain.APBD;
using System.Data;

namespace SIEVK.BusinessData.APBD
{
    public class APBDCatatanLaporanRealisasiSKPDDAO
    {
        public DataTable GetCatatanLaporanRealisasi(string unitKey, string kdTahun, string kdTriwulan, string kdFAKTOR = null)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, kdTriwulan);
                db.AddParameter("KDFAKTOR", SqlDbType.VarChar, kdFAKTOR);
                dt = db.ExecuteDataTable("sp_APBD_CatatanLaporanRealisasiSKPDList");
            }
            return dt;
        }

        public void Delete(string unitKey, string kdTahun, string kdTriwulan, string kdFAKTOR)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UnitKey", SqlDbType.VarChar, unitKey);
                db.AddParameter("KDTahun", SqlDbType.VarChar, kdTahun);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, kdTriwulan);
                db.AddParameter("KDFAKTOR", SqlDbType.VarChar, kdFAKTOR);
                db.ExecuteNonQuery("sp_APBD_CatatanLaporanRealisasiSKPDDelete");
            }
        }

        public void Create(APBDCatatanLaporanRealisasi realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, realisasi.KDTRWN);
                db.AddParameter("KDFAKTOR", SqlDbType.VarChar, realisasi.KDFAKTOR);
                db.AddParameter("NMCtt", SqlDbType.VarChar, realisasi.NMCTT);
                db.AddParameter("IsiCtt", SqlDbType.VarChar, realisasi.ISICTT);
                db.ExecuteNonQuery("sp_APBD_CatatanLaporanRealisasiSKPDCreate");
            }
        }

        public void Update(APBDCatatanLaporanRealisasi realisasi)
        {
            DataTable dt = new DataTable();
            using (DataAccess db = new DataAccess())
            {
                db.AddParameter("UNITKEY", SqlDbType.VarChar, realisasi.UNITKEY);
                db.AddParameter("KDTahun", SqlDbType.VarChar, realisasi.KDTAHUN);
                db.AddParameter("KDTriwulan", SqlDbType.VarChar, realisasi.KDTRWN);
                db.AddParameter("KDFAKTOR", SqlDbType.VarChar, realisasi.KDFAKTOR);
                db.AddParameter("NMCtt", SqlDbType.VarChar, realisasi.NMCTT);
                db.AddParameter("IsiCtt", SqlDbType.VarChar, realisasi.ISICTT);
                db.ExecuteNonQuery("sp_APBD_CatatanLaporanRealisasiSKPDUpdate");
            }
        }


    }
}
