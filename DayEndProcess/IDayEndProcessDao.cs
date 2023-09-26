using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INCHEQS.EOD.DayEndProcess {
    public interface IDayEndProcessDao {
        
        DataTable GetChequeInfo(string bankCode);
        List<string> Validate(FormCollection col,string bankCode);

        }
    }