﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using PMIS.Model;

namespace PMIS.HelperClass
{
    public static class Utilities
    {
        public static string GeneratePatientId()
        {
            var pmisentity = new PMISEntities();

            int id = 0;

            try
            {

                var seriesparam = new SqlParameter()
                {
                    ParameterName = "series",
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Output
                };

                var seriesId = pmisentity.Database.SqlQuery<int>("exec Generate_PatCode @series out", seriesparam)
                    .FirstOrDefault();

                id = Convert.ToInt32(seriesId);
            }
            catch (NullReferenceException)
            {

                id += 1;

            }
            catch (FormatException)
            {
                id += 1;
            }

            return (string.Format("{0:000000}", id));
        }

        public static string ReportPath(string repName)
        {

            return HostingEnvironment.MapPath(string.Format("~/Reports/{0}.rpt", repName));

        }

        public static string DBGateway()
        {

            ConnectionStringSettings dbconnString = ConfigurationManager.ConnectionStrings["PMISEntitiesUserIdentity"];

            return dbconnString.ConnectionString;

        }
    }
}