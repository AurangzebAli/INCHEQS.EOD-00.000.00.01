using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
//using INCHEQS.Helpers;
using System.Web.Mvc;
using System.Globalization;
using INCHEQS.EOD.DayEndProcess;
using System.Data.Entity;
using INCHEQS.DataAccessLayer;
using INCHEQS.Common;
using INCHEQS.EOD.Resource;
using Npgsql;

public class DayEndProcessDao : IDayEndProcessDao
{
    private readonly ApplicationDbContext dbContext;

    public DayEndProcessDao(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public DataTable GetChequeInfo(string bankCode)
    {
        DataTable ds = new DataTable();

        string stmt = "select convert(varchar(20),fldcleardate,105) as fldClearDate From tblInwardClearDate where fldStatus = 'Y' group by fldClearDate order by fldClearDate desc ";

        ds = dbContext.GetRecordsAsDataTable(stmt);

        return ds;
    }

    public List<string> Validate(FormCollection col, string bankCode)
    {
        List<string> err = new List<string>();
        DataTable ds = new DataTable();
        /* Author:Ali for ICS End Process to show count popup
        //if (col["txtChequeInfo"] == null || col["txtArchivalHistory"] == null || col["txtArchivalTable"] == null || col["txtArchivalChequeImg"] == null || col["txtHousekeepData"] == null || col["txtHousekeepHistory"] == null || col["txtHousekeepImg"] == null)
        //{
        //    err.Add(Locale.Pleasecheckallthemandatoryfield);
        //}

        */
        ds = dbContext.GetRecordsAsDataTableSP("spcgItemsEOD", new[] {
            new SqlParameter("@clearDate", DateUtils.formatDateToSql(col["CIAchDate"]))
        });

        if (ds.Rows.Count > 0)
        {
            int total = Convert.ToInt32(ds.Rows[0]["total"]);
            if (total != 0)
            {
                err.Add(total + " item(s) not yet verify");
            }
        }
        return err;
    }

}