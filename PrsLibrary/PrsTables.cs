using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

//this is a base/generic/utility class

namespace PrsLibrary {
    public class PrsTables {
        private SqlCommand CreateConnection(string ConnStr, string Sql, string message) {
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != ConnectionState.Open) {
                throw new ApplicationException(message);

            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            return Cmd;
        }
        protected static void ExecuteSqlCommand(SqlCommand Cmd, string message) {
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException(message);
            }
        } 
    }
}
