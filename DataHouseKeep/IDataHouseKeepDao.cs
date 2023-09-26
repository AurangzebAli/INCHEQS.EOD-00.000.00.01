using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INCHEQS.EOD.DataHouseKeep {
    public interface IDataHouseKeepDao {
        DataHouseKeepModel GetDataHouseKeep();
        List<string> Validate(FormCollection col);
        DataHouseKeepModel getData(FormCollection col);
        void UpdateData(Dictionary<string, string> col, string userID);

    }
}