using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ODPI.Actions.Validate
{
    public class SqlAzureValidation : IValidateAction
    {

        public string Validate(dynamic data)
        {
            string ret = null;
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