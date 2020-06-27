using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PMIS.HelperClass;
using PMIS.ServiceLayer;


namespace PMIS.Reports.ReportViewer
{
    public partial class Prescription : System.Web.UI.Page
    {
        //private readonly IPrescriptionServices _prescriptionServices;

        //public Prescription(IPrescriptionServices prescriptionServices)
        //{
        //    _prescriptionServices = prescriptionServices;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    //var medrecordNo = Request["medRecNo"].Trim();

                    //var docprescriptionList 

                    var cryRep=new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "Prescription";

                    string report = Utilities.ReportPath(reportName);

                    cryRep.Load(report);

                    SqlConnectionStringBuilder cnstrbuilding = new SqlConnectionStringBuilder(Utilities.DBGateway());

                    crConinfo.ServerName = cnstrbuilding.DataSource;
                    crConinfo.DatabaseName = cnstrbuilding.InitialCatalog;
                    crConinfo.UserID = cnstrbuilding.UserID;
                    crConinfo.Password = cnstrbuilding.Password;


                    var cryTables = cryRep.Database.Tables;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table cryTable in cryTables)
                    {
                        var tbloginfo = cryTable.LogOnInfo;
                        tbloginfo.ConnectionInfo = crConinfo;
                        tbloginfo.ConnectionInfo.IntegratedSecurity = true;
                        cryTable.ApplyLogOnInfo(tbloginfo);
                    }



                    CrystalReportPrescription.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    cryRep.Database.Tables[0].SetDataSource(ContainerClass.MedicalRecord);
                    cryRep.Database.Tables[1].SetDataSource(ContainerClass.DocPrescription);
                   
                 
                    //cryRep.Database.Tables[1].SetDataSource(ContainerClass.PatientDetails);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Prescription");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }

        }
    }
}