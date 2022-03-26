namespace SIEVK.Domain.APBD
{
    public class APBDKinerja
    {
        public string IDKEG
        { get; set; }

        public string KDTAHUN
        { get; set; }

        public string UNITKEY
        { get; set; }

        public string KDTAHAP
        { get; set; }

        public string KDTRWN
        { get; set; }

        public decimal REALNUM
        { get; set; }

        public decimal REALSTR
        { get; set; }

        public decimal REALSTR1
        { get; set; }

        public decimal REALFISIK
        { get; set; }

        public string SATUAN
        { get; set; }

        public string SATUAN1
        { get; set; }

        public string MASALAH
        { get; set; }

        public string SOLUSI
        { get; set; }

        public string KET
        { get; set; }


        #region used by Index of Realisasi Kinerja
        public string IDPRGRM
        { get; set; }
        public string NMKEG
        { get; set; }
        public string NUKEG
        { get; set; }
        public string URKIN
        { get; set; }
        public string TARGET
        { get; set; }
        public decimal PAGU
        { get; set; }
        public string LOKASI
        { get; set; }
        public string NMTRWN
        { get; set; }

        //Ian, 20200210
        public decimal ANGKAS1
        { get; set; }
        public decimal ANGKAS2
        { get; set; }
        public decimal ANGKAS3
        { get; set; }
        public decimal ANGKAS4
        { get; set; }
        public string CTT
        { get; set; }
        #endregion

    }
}
