using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PrsLibrary {
    public class Product : PrsTables {
        public int Id { get; set; }
        public string VendorId { get; set; }
        public string Name { get; set; }
        public string VendorPartNumber { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string PhotoPath { get; set; }

        private static SqlCommand CreateConnection(string ConnStr, string Sql, string message) {
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != ConnectionState.Open) {
                throw new ApplicationException(message);

            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            return Cmd;
        }

        private static void AddSqlInsertUpdateParameters(SqlCommand Cmd, Product product) {
            Cmd.Parameters.Add(new SqlParameter("@id", product.Id));
            Cmd.Parameters.Add(new SqlParameter("@vendorid", product.VendorId));
            Cmd.Parameters.Add(new SqlParameter("@name", product.Name));
            Cmd.Parameters.Add(new SqlParameter("@vendorpartnumber", product.VendorPartNumber));
            Cmd.Parameters.Add(new SqlParameter("@price", product.Price));
            Cmd.Parameters.Add(new SqlParameter("@unit", product.Unit));
            Cmd.Parameters.Add(new SqlParameter("@photopath", product.PhotoPath));
        }
        //Select
        public static ProductCollection Select(string WhereClause, string OrderByClause) {
            string Sql = string.Format("SELECT * from [Product] WHERE {0} ORDER BY {1}", WhereClause, OrderByClause);
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
            ProductCollection products = new ProductCollection();
            while (Reader.Read()) {
                int id = Reader.GetInt32(Reader.GetOrdinal("Id"));
                string vendorid = Reader.GetString(Reader.GetOrdinal("VendorId"));
                string name = Reader.GetString(Reader.GetOrdinal("Name"));
                string vendorpartnumber = Reader.GetString(Reader.GetOrdinal("VendorPartNumber"));
                decimal price = Reader.GetDecimal(Reader.GetOrdinal("Price"));
                string unit = Reader.GetString(Reader.GetOrdinal("Unit"));
                string photopath = Reader.GetString(Reader.GetOrdinal("PhotoPath"));

                Product product = new Product();
                product.Id = id;
                product.VendorId = vendorid;
                product.Name = name;
                product.VendorPartNumber = vendorpartnumber;
                product.Price = price;
                product.Unit = unit;
                product.PhotoPath = photopath;

                products.Add(product);
            }
            return products;
        }
        //UPDATE
        public static bool Update(Product products) {
            string Sql = string.Format("UPDATE [product] Set " +
                        " Id = @id," +
                        " VendorId = @vendorid," +
                        " Name = @name," +
                        " VendorPartNumber = @vendorpartnumber, " +
                        " Price = @price, " +
                        " Unit = @unit, " +
                        " PhotoPath = @photopath, " +
                        " WHERE ID = @Id; ");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            AddSqlInsertUpdateParameters(Cmd, products);
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
        }
        //INSERT
        public static bool Insert(Product product) {
            string Sql = string.Format("insert into [product] " +
           "Id, VendorId, Name, VendorPartNumber, Price, Unit, PhotoPath) " +
           " values" +
           "(@id, @vendorid, @name, @vendorpartnumber, @price, @unit, @photopath)");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            AddSqlInsertUpdateParameters(Cmd, product);
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
            product.Id = GetLastIdGenerated(ConnStr, "Product");
            Cmd.Connection.Close();
            return (recsAffected == 1);
            }
            private static int GetLastIdGenerated(string ConnStr, string TableName) {
            string sql = string.Format("SELECT IDENT_CURRENT({0})", TableName);
            SqlCommand Cmd = CreateConnection(ConnStr, "SELECT IDENT_CURRENT('Product')", "failed to insert");
            object NewId = Cmd.ExecuteScalar();
            return int.Parse(NewId.ToString());
            }
        //DELETE
        public static bool Delete(Product product) {
            string Sql = string.Format("DELETE from [Product] WHERE ID = @id");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("id", product.Id));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                return true;
            }
            return recsAffected == 1;
        }
    }
}
