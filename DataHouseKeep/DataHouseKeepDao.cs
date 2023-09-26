
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
//using INCHEQS.Helpers;
using System.Web.Mvc;
using System.Linq;
using INCHEQS.EOD.DataHouseKeep;
using INCHEQS.DataAccessLayer;
using INCHEQS.Common;
using INCHEQS.EOD.Resource;
using Npgsql;

public class DataHouseKeepDao : IDataHouseKeepDao {


    private readonly ApplicationDbContext dbContext;
    public DataHouseKeepDao(ApplicationDbContext dbContext) {
        this.dbContext = dbContext;
    }


    public DataHouseKeepModel GetDataHouseKeep() {

        DataHouseKeepModel datahouse = new DataHouseKeepModel();
        string stmt = "SELECT * FROM tblAuditLogRetention";

        DataTable ds = new DataTable();
        ds = dbContext.GetRecordsAsDataTable( stmt);

        if (ds.Rows.Count > 0) {
            DataRow row = ds.Rows[0];
            string auditLog = row["fldAuditLog"].ToString().Trim();
            //1m
            datahouse.audit1 = new string(auditLog.Where(char.IsDigit).ToArray());//takes digit only
            datahouse.audit2 = StringUtils.UCase(StringUtils.Right(auditLog, 1)).ToString();
            //m

            string achauditLog = row["fldAchAuditLog"].ToString().Trim();
            //1m
            datahouse.achaudit1 = new string(achauditLog.Where(char.IsDigit).ToArray()); ;
            //1
            datahouse.achaudit2 = StringUtils.UCase(StringUtils.Right(achauditLog, 1)).ToString();
            //m

        }
        return datahouse;
    }

    public void UpdateData(Dictionary<string, string> col,string userID) {
        string stmt = "UPDATE tblAuditLogRetention SET fldAuditLog = @auditlog, fldAchAuditLog = @archievelog, fldUpdateUserId = @userId, fldUpdateTimeStamp = @todaydate ";

        dbContext.ExecuteNonQuery( stmt, new[] {
                new SqlParameter("@auditlog" , string.Concat(col["txtPal"], col["PalInterval"])),
                new SqlParameter("@archievelog" , string.Concat(col["txtArchivedALFile"], col["AALFInterval"])),
                new SqlParameter("@userId" , userID),
                new SqlParameter("@todaydate" ,  DateTime.Now)
            });

    }

    public List<string> Validate(FormCollection col) {

        List<string> err = new List<string>();
        int result;
        if ((col["txtPal"].Equals("0"))) {
            err.Add(Locale.ICCSAuditLogcannotbezero);
        }
        if ((col["txtArchivedALFile"].Equals("0"))) {
            err.Add(Locale.ArchivedAuditLogFilescannotbezero);
        }
        if (!(int.TryParse((col["txtPal"]), out result))) {
            err.Add(Locale.ICCSAuditLogmustbenumeric);
        }
        if (!(int.TryParse((col["txtArchivedALFile"]), out result))) {
            err.Add(Locale.ArchivedAuditLogFilesmustbenumeric);
        }

        return err;
    }

    public DataHouseKeepModel getData(FormCollection col) {

        string stmt = "Select * from tblAuditLogRetention";
        DataHouseKeepModel datahousekeep = new DataHouseKeepModel();
        DataTable ds = new DataTable();
        ds = dbContext.GetRecordsAsDataTable( stmt);

        if (ds.Rows.Count > 0) {
            DataRow row = ds.Rows[0];
            datahousekeep.auditLog = row["fldAuditLog"].ToString();
            datahousekeep.archiveLog = row["fldAchAuditLog"].ToString();
        }

        return datahousekeep;
    }
}