using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.Domain.Common
{
    public class ErrorLog
    {
        public int ID
        { get; set; }

        public String ErrorMessage
        { get; set; }

        public String InnerException
        { get; set; }

        public String ExceptionStackTrace
        { get; set; }

        public String CreatedBy
        { get; set; }

        public DateTime CreatedTime
        { get; set; }

    }
}
