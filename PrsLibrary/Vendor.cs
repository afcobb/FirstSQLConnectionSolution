using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PrsLibrary {
    public class Vendor : PrsTables {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsRecommended { get; set; }

        private static SqlCommand CreateConnection(string ConnStr, string Sql, string message) { 
           SqlConnection Conn = new SqlConnection(ConnStr);
           Conn.Open();
            if (Conn.State != ConnectionState.Open) {
                throw new ApplicationException(message);

           }
           SqlCommand Cmd = new SqlCommand(Sql, Conn);
           return Cmd;
       }

        private static void AddSqlInsertUpdateParameters(SqlCommand Cmd, Vendor vendor) {
            Cmd.Parameters.Add(new SqlParameter("@code", vendor.Code));
            Cmd.Parameters.Add(new SqlParameter("@name", vendor.Name));
            Cmd.Parameters.Add(new SqlParameter("@address", vendor.Address));
            Cmd.Parameters.Add(new SqlParameter("@city", vendor.City));
            Cmd.Parameters.Add(new SqlParameter("@state", vendor.State));
            Cmd.Parameters.Add(new SqlParameter("@zip", vendor.Zip));
            Cmd.Parameters.Add(new SqlParameter("@phone", vendor.Phone));
            Cmd.Parameters.Add(new SqlParameter("@email", vendor.Email));
            Cmd.Parameters.Add(new SqlParameter("@isrecommended", vendor.IsRecommended));
        }
        //Select
        public static VendorCollection Select(string WhereClause, string OrderByClause) {
            string Sql = string.Format("SELECT * from [Vendor] WHERE {0} ORDER BY {1}", WhereClause, OrderByClause);
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            SqlDataReader Reader = Cmd.ExecuteReader();
            if (!Reader.HasRows) {
                throw new ApplicationException("Result set has no rows!");
            }
            VendorCollection vendors = new VendorCollection();
            while (Reader.Read()) {
                int id = Reader.GetInt32(Reader.GetOrdinal("Id"));
                string code = Reader.GetString(Reader.GetOrdinal("Code"));
                string name = Reader.GetString(Reader.GetOrdinal("Name"));
                string address = Reader.GetString(Reader.GetOrdinal("Address"));
                string city = Reader.GetString(Reader.GetOrdinal("City"));
                string state = Reader.GetString(Reader.GetOrdinal("State"));
                string zip = Reader.GetString(Reader.GetOrdinal("Zip"));
                string phone = Reader.GetString(Reader.GetOrdinal("Phone"));
                string email = Reader.GetString(Reader.GetOrdinal("Email"));
                bool isRecommended = Reader.GetBoolean(Reader.GetOrdinal("IsRecommended"));

                Vendor vendor = new Vendor();
                vendor.Id = id;
                vendor.Code = code;
                vendor.Name = name;
                vendor.Address = address;
                vendor.City = city;
                vendor.State = state;
                vendor.Zip = zip;
                vendor.IsRecommended = isRecommended;


                vendors.Add(vendor);
            }
            return vendors;
        }
        //UPDATE
        public static bool Update(Vendor vendor) {
            string Sql = string.Format("UPDATE [vendor] Set " +
                        " Code = @code," +
                        " Name = @name," +
                        " Address = @address, " +
                        " City = @city, " +
                        " State = @state, " +
                        " Zip = @zip, " +
                        " Phone = @phone, " +
                        " Email = @email, " +
                        " IsRecommended = @isrecommended, " +                       
                        " WHERE ID = @Id; ");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("@code", vendor.Code));
            Cmd.Parameters.Add(new SqlParameter("@name", vendor.Name));
            Cmd.Parameters.Add(new SqlParameter("@address", vendor.Address));
            Cmd.Parameters.Add(new SqlParameter("@city", vendor.City));
            Cmd.Parameters.Add(new SqlParameter("@state", vendor.State));
            Cmd.Parameters.Add(new SqlParameter("@zip", vendor.Zip));
            Cmd.Parameters.Add(new SqlParameter("@phone", vendor.Phone));
            Cmd.Parameters.Add(new SqlParameter("@email", vendor.Email));
            Cmd.Parameters.Add(new SqlParameter("@isrecommended", vendor.IsRecommended));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
        }
        //INSERT
        public static bool Insert(Vendor vendor) {
            string Sql = string.Format("insert into [vendor] " +
           "Code, Name, Address, City, State, Zip, Phone, Email, IsRecommmended) " +
           " values" +
           "(@code, @name, @address, @city, @state, @zip,, @phone, @email, @isrecommended)");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("@code", vendor.Code));
            Cmd.Parameters.Add(new SqlParameter("@name", vendor.Name));
            Cmd.Parameters.Add(new SqlParameter("@address", vendor.Address));
            Cmd.Parameters.Add(new SqlParameter("@city", vendor.City));
            Cmd.Parameters.Add(new SqlParameter("@state", vendor.State));
            Cmd.Parameters.Add(new SqlParameter("@zip", vendor.Zip));
            Cmd.Parameters.Add(new SqlParameter("@phone", vendor.Phone));
            Cmd.Parameters.Add(new SqlParameter("@email", vendor.Email));
            Cmd.Parameters.Add(new SqlParameter("@isrecommended", vendor.IsRecommended));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
            //get the last id inserted
            vendor.Id = GetLastIdGenerated(ConnStr, "Vendor");
            Cmd.Connection.Close();
            return (recsAffected == 1);
        }
            private static int GetLastIdGenerated(string ConnStr, string TableName) {
            string sql = string.Format("SELECT IDENT_CURRENT({0})", TableName);
                SqlCommand Cmd = CreateConnection(ConnStr, "SELECT IDENT_CURRENT('VENDOR')", "failed to insert");
                object NewId = Cmd.ExecuteScalar();
                return int.Parse(NewId.ToString());
        }

       
        //DELETE
        public static bool Delete(Vendor vendor) {
            string Sql = string.Format("DELETE from [Vendor] WHERE ID = @id");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("id", vendor.Id));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                return true;
            }
            return recsAffected == 1;
        }
    }
}
