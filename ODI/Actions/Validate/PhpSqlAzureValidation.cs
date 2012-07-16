﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ODI.Actions.Validate
{
    //Note: this doesn't extend <code>SqlAzureValidation</code> due to the use of a dynamic parameter which causes issues with subclasses calling base methods.
    public class PhpSqlAzureValidation : IValidateAction
    {
        public string Validate(dynamic data)
        {
            string ret = null;
            //First check that the host name is appended to the username.
            if( !data.dbhost.EndsWith(".database.windows.net" ))
                return ODI.Resources.Actions.ValidateResource.TheDatabaseHostFormat;
            


            string username = data.dbusername;
            string host = data.dbhost.Split('.')[0];

            if( !username.EndsWith( "@" + host ))
                return ODI.Resources.Actions.ValidateResource.ForDatabasePHPUsernameMustBe;

            

            // Create a connection string for the sample database
            SqlConnectionStringBuilder connStringBuilder;
            connStringBuilder = new SqlConnectionStringBuilder();
            connStringBuilder.DataSource = data.dbhost;
            connStringBuilder.InitialCatalog = data.dbname;
            connStringBuilder.Encrypt = true;
            connStringBuilder.TrustServerCertificate = false;
            connStringBuilder.UserID = data.dbusername;
            connStringBuilder.Password = data.dbpassword;

            using (SqlConnection conn = new SqlConnection(connStringBuilder.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    try
                    {

                        conn.Open();
                        command.CommandText = "Select getdate()";
                        command.ExecuteScalar();
                        conn.Close();
                    }
                    catch (Exception e)
                    {
                        ret = e.Message;
                    }
                }
            }

            return ret;
        }
    }
}