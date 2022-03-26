using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIEVK.BusinessData.Common;
using SIEVK.Domain.Mapping;
using SIEVK.BusinessData.Mapping;
using SIEVK.BusinessData;

namespace SIEVK.BusinessService.Mapping
{
    public class MappingReferenceService
    {
        Mapper map = new Mapper();
        MappingReferenceDAO dao = new MappingReferenceDAO();

        public List<MPGRM> GetMPGRMList(string idPRGRM = "")
        {
            DataTable dt = dao.GetMPGRMList(idPRGRM);
            List<MPGRM> lst = map.BindDataList<MPGRM>(dt);
            return lst;
        }

        public List<MPGRM90> GetMPGRM90List(string idPRGRM90 = "")
        {
            DataTable dt = dao.GetMPGRM90List(idPRGRM90);
            List<MPGRM90> lst = map.BindDataList<MPGRM90>(dt);
            return lst;
        }

        public List<MPGRM50> GetMPGRM50List(string idPRGRM050 = "")
        {
            DataTable dt = dao.GetMPGRM50List(idPRGRM050);
            List<MPGRM50> lst = map.BindDataList<MPGRM50>(dt);
            return lst;
        }

        public List<MKEGIATAN> GetMKEGIATANList(string idPRGRM, string idKEG = "")
        {
            DataTable dt = dao.GetMKEGIATANList(idPRGRM, idKEG);
            List<MKEGIATAN> lst = map.BindDataList<MKEGIATAN>(dt);
            return lst;
        }

        public List<MKEGIATAN90> GetMKEGIATAN90List(string idPRGRM90 = "", string idKEG90 = "")
        {
            DataTable dt = dao.GetMKEGIATAN90List(idPRGRM90, idKEG90);
            List<MKEGIATAN90> lst = map.BindDataList<MKEGIATAN90>(dt);
            return lst;
        }

        public List<MKEGIATAN50> GetMKEGIATAN50List(string idPRGRM050 = "", string idKEG050 = "")
        {
            DataTable dt = dao.GetMKEGIATAN50List(idPRGRM050, idKEG050);
            List<MKEGIATAN50> lst = map.BindDataList<MKEGIATAN50>(dt);
            return lst;
        }
    }
}
