using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrsLibrary;
using System.Data;
using System.Data.SqlClient;

namespace PrsLibrary {
    public class User : PrsTables {

        public int id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        //Create Connection routine
        //private SqlCommand CreateConnection(string ConnStr, string Sql, string message) {
        //    SqlConnection Conn = new SqlConnection(ConnStr);
        //    Conn.Open();
        //    if (Conn.State != ConnectionState.Open) {
        //        throw new ApplicationException(message);

        //    }
        //    SqlCommand Cmd = new SqlCommand(Sql, Conn);
        //    return Cmd;
        //}


        //REFACTORING CODE
        //private static void AddSqlInsertUpdateParameters(SqlCommand Cmd, User user) {
        //    Cmd.Parameters.Add(new SqlParameter("@username", user.UserName));
        //    Cmd.Parameters.Add(new SqlParameter("@password", user.Password));
        //    Cmd.Parameters.Add(new SqlParameter("@firstname", user.FirstName));
        //    Cmd.Parameters.Add(new SqlParameter("@lastname", user.LastName));
        //    Cmd.Parameters.Add(new SqlParameter("@phone", user.Phone));
        //    Cmd.Parameters.Add(new SqlParameter("@email", user.Email));
        //    Cmd.Parameters.Add(new SqlParameter("@isreviewer", user.IsReviewer));
        //    Cmd.Parameters.Add(new SqlParameter("@isadmin", user.IsAdmin));
        //}

        //
        
        //UPDATE
        public static bool Update(User user) {
            string Sql = string.Format("UPDATE [user] Set " +
                        " UserName = @username," +
                        " Password = @password," +
                        " FirstName = @firstname, " +
                        " LastName = @lastname, " +
                        " Phone = @phone, " +
                        " Email = @email, " +
                        " IsReviewer = @isreviewer, " +
                        " IsAdmin = @isadmin " +
                        " WHERE ID = @Id; ");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("@id", user.id));
            Cmd.Parameters.Add(new SqlParameter("@username", user.UserName));
            Cmd.Parameters.Add(new SqlParameter("@password", user.Password));
            Cmd.Parameters.Add(new SqlParameter("@firstname", user.FirstName));
            Cmd.Parameters.Add(new SqlParameter("@lastname", user.LastName));
            Cmd.Parameters.Add(new SqlParameter("@phone", user.Phone));
            Cmd.Parameters.Add(new SqlParameter("@email", user.Email));
            Cmd.Parameters.Add(new SqlParameter("@isreviewer", user.IsReviewer));
            Cmd.Parameters.Add(new SqlParameter("@isadmin", user.IsAdmin));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
        }
        //INSERT
        public static bool Insert(User user) {
            string Sql = string.Format("insert into [user] " +
           " UserName, Password, FirstName, LastName, Phone, Email, IsReviewer, IsAdmin) " +
           " values" +
           "(@username, @password, @firstname, @lastname, @phone, @email, @isreviewer, @isadmin)");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
          
            Cmd.Parameters.Add(new SqlParameter("@username", user.UserName));
            Cmd.Parameters.Add(new SqlParameter("@password", user.Password));
            Cmd.Parameters.Add(new SqlParameter("@firstname", user.FirstName));
            Cmd.Parameters.Add(new SqlParameter("@lastname", user.LastName));
            Cmd.Parameters.Add(new SqlParameter("@phone", user.Phone));
            Cmd.Parameters.Add(new SqlParameter("@email", user.Email));
            Cmd.Parameters.Add(new SqlParameter("@isreviewer", user.IsReviewer));
            Cmd.Parameters.Add(new SqlParameter("@isadmin", user.IsAdmin));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
        }
        //DELETE
        public static bool Delete(User user) { 
            string Sql = string.Format("DELETE from [User] WHERE ID = @id");
        string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
        SqlConnection Conn = new SqlConnection(ConnStr);
        Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("id", user.id));
            int recsAffected = Cmd.ExecuteNonQuery();
            if(recsAffected != 1) {
                return true;
            }
            return recsAffected == 1;
    }

        //Select
        public static User Select(int Id) {
            UserCollection users = User.Select($"Id = {Id}", "Id");
            User user = (users.Count == 1) ? users[0] : null;
            return user;
        }

        public static UserCollection Select(string whereClause, string orderByClause) {
            string Sql = string.Format("SELECT * from [User] WHERE {0} ORDER BY {1}", whereClause, orderByClause);
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
                int id = Reader.GetInt32(Reader.GetOrdinal("id"));
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
            return users;
        }
            
    }
}
