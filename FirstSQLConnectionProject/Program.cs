using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PrsLibrary;



namespace FirstSQLConnectionProject {
    class Program {
        static void Main(string[] args) {

            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
                Conn.Open();
                if (Conn.State != System.Data.ConnectionState.Open) {
                    throw new ApplicationException("Connection didn't open");
                }


            //            //insert
            //            string SqlInsert = @"insert [user] (UserName, Password, FirstName, LastName, Phone, Email, IsAdmin, IsReviewer)
            //values(@username, @password, @firstname, @lastname, @phone, @email, @isadmin, @isreviewer)";
            //            SqlCommand CmdInsert = new SqlCommand(SqlInsert, Conn); // creating new command'
            //            CmdInsert.Parameters.Add(new SqlParameter("@username", "acobb"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@isadmin", "1"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@password", "password"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@firstname", "alex"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@lastname", "cobb"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@phone", "513-592-0744"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@email", "afcobb@gmail.com"));
            //            CmdInsert.Parameters.Add(new SqlParameter("@isreviewer", "1"));
            //            int recsAffected = CmdInsert.ExecuteNonQuery();
            //            if(recsAffected != 1) { // checking if the operation works
            //                throw new ApplicationException("Insert failed!");
            //            }

            //Select - using UserCollection class
            string whereClause = "Id = 3";
            string orderByClause = "Username desc";
            UserCollection users = User.Select(whereClause, orderByClause);
            User user = users[0];
            user.FirstName = "Alex";
            bool success = User.Update(user);
            success = User.Delete(user);
            //Insert function
            User user2Insert = new User {
                UserName = "afcobb",
                Password = "password",
                FirstName = "alex",
                LastName = "cobb",
                Phone = "5135920744",
                Email = "afcobb@gmail.com",
                IsReviewer = false,
                IsAdmin = false
            };
            success = User.Insert(user2Insert);


            VendorCollection vendors = Vendor.Select("1 - 1", "Id");
            Vendor vendor = new Vendor {
                Code = "Krog0002",
                Name = "Kroger",
                Address = "123 Any St.",
                City = "Cincinnati", State = "OH", Zip = "45201",
                Email = "info@kroger.com", Phone = "513-555-1212",
                IsRecommended = true
            };
            bool rc = Vendor.Insert(vendor);
            vendors = Vendor.Select("Code = 'Krog0001'", "Code");
            vendor = vendors[0];
            vendor.Code = "KROG1111";
            rc = Vendor.Update(vendor);
            rc = Vendor.Delete(vendor);
            int i = 0;
        }
        ProductCollection products = Product.Select("1 - 1", "Id");
        Product product = new Product {
            Name = "Hat",
            VendorPartNumber = "Hat001",
            Price = 19.99M, //M denotes that its a decimal not a double
            Unit = "Each",
            PhotoPath = null
        };



    }
}
