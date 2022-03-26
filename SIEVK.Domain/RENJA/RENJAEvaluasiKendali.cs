using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.Domain.RENJA
{
    public class RENJAEvaluasiKendali
    {
        public string NOMOR
        { get; set; }

        public string UNITKEY
        { get; set; }

        public string KDTAHUN
        { get; set; }

        public string KDTAHAP
        { get; set; }

        public string URAIAN
        { get; set; }

        public string TARGET_RENJA
        { get; set; }

        public string TARGET_RKA
        { get; set; }

        public decimal PAGU_RKA
        { get; set; }

        public decimal PAGU_RENJA
        { get; set; }

        public string TYPE
        { get; set; }

        public int KESESUAIAN
        { get; set; }

        public string MASALAH
        { get; set; }

        public string SOLUSI
        { get; set; }

        public string HASIL
        { get; set; }
    }
}
