using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace PrsLibrary {
    public class UserCollection : List<User> {

        public UserCollection Select(string Sql) {
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
            UserCollection users = new UserCollection();
            while (Reader.Read()) {
                int id = Reader.GetInt32(Reader.GetOrdinal("Id"));
                string userName = Reader.GetString(Reader.GetOrdinal("Username"));
                string password = Reader.GetString(Reader.GetOrdinal("Password"));
                string firstName = Reader.GetString(Reader.GetOrdinal("FirstName"));
                string lastName = Reader.GetString(Reader.GetOrdinal("LastName"));
                string phone = Reader.GetString(Reader.GetOrdinal("Phone"));
                string email = Reader.GetString(Reader.GetOrdinal("Email"));
                bool isReviewer = Reader.GetBoolean(Reader.GetOrdinal("IsReviewer"));
                bool isAdmin = Reader.GetBoolean(Reader.GetOrdinal("IsAdmin"));

                User user = new User();
                user.id = id;
                user.UserName = userName;
                user.Password = password;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.Phone = phone;
                user.IsReviewer = isReviewer;
                user.IsAdmin = isAdmin;

                users.Add(user);
            }
            Conn.Close();
            return users;
        }
    }
}
