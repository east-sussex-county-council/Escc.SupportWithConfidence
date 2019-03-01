using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Configuration;

namespace Escc.SupportWithConfidence.ETL
{




    public static class DataAccess
    {
        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["livedb"].ConnectionString;
        }


        public static void Save(string storedProcedure, SqlParameter[] parameters)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
               
                    SqlHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, storedProcedure, parameters);
                
            }
        }
    }

}
