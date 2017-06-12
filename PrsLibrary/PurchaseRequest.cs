using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PrsLibrary {

    public class PurchaseRequest : PrsTables {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; private set; }
        public string Description { get; set; }
        public string Justification { get; set; }
        public DateTime DateNeeded { get; set; }
        public string DeliveryMode { get; set; }
        public bool DocsAttached { get; set; }
        public string Status { get; set; }
        public decimal Total { get; private set; }
        public DateTime SubmittedDate { get; private set; }

        private static void AddSqlInsertUpdateParameters(SqlCommand Cmd, PurchaseRequest purchaseRequest) {
            Cmd.Parameters.Add(new SqlParameter("@id", purchaseRequest.Id));
            Cmd.Parameters.Add(new SqlParameter("@userid", purchaseRequest.UserId));
            Cmd.Parameters.Add(new SqlParameter("@user", purchaseRequest.User));
            Cmd.Parameters.Add(new SqlParameter("@description", purchaseRequest.Description));
            Cmd.Parameters.Add(new SqlParameter("@justification", purchaseRequest.Justification));
            Cmd.Parameters.Add(new SqlParameter("@dateneeded", purchaseRequest.DateNeeded));
            Cmd.Parameters.Add(new SqlParameter("@deliverymode", purchaseRequest.DeliveryMode));
            Cmd.Parameters.Add(new SqlParameter("@docsattached", purchaseRequest.DocsAttached));
            Cmd.Parameters.Add(new SqlParameter("@status", purchaseRequest.Status));
            Cmd.Parameters.Add(new SqlParameter("@total", purchaseRequest.Total));
            Cmd.Parameters.Add(new SqlParameter("@submitteddate", purchaseRequest.SubmittedDate));
        }
        //Select
        public static PurchaseRequest Select(int Id) {
            PurchaseRequestCollection purchaseRequests = PurchaseRequest.Select($"Id = {Id}", "Id");
            PurchaseRequest purchaseRequest = (purchaseRequests.Count == 1) ? purchaseRequests[0] : null;
            return purchaseRequest;
        }

        public static PurchaseRequestCollection Select(string WhereClause, string OrderByClause) {
            string Sql = string.Format("SELECT * from [purchaseRequest] WHERE {0} ORDER BY {1}", WhereClause, OrderByClause);
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
            PurchaseRequestCollection purchaseRequests = new PurchaseRequestCollection();
            while (Reader.Read()) {
                int id = Reader.GetInt32(Reader.GetOrdinal("Id"));
                int userid = Reader.GetInt32(Reader.GetOrdinal("UserId"));
                string user = Reader.GetString(Reader.GetOrdinal("User"));
                string description = Reader.GetString(Reader.GetOrdinal("Description"));
                string justification = Reader.GetString(Reader.GetOrdinal("Justification"));
                DateTime dateneeded = Reader.GetDateTime(Reader.GetOrdinal("DateNeeded"));
                string deliverymode = Reader.GetString(Reader.GetOrdinal("DeliveryMode"));
                bool docsattached = Reader.GetBoolean(Reader.GetOrdinal("DocsAttached"));
                string status = Reader.GetString(Reader.GetOrdinal("Status"));
                decimal total = Reader.GetDecimal(Reader.GetOrdinal("Total"));
                DateTime submitteddate = Reader.GetDateTime(Reader.GetOrdinal("SubmittedDate"));

                PurchaseRequest purchaseRequest = new PurchaseRequest();
                purchaseRequest.Id = id;
                purchaseRequest.UserId = userid;
                purchaseRequest.Description = description;
                purchaseRequest.Justification = justification;
                purchaseRequest.DateNeeded = dateneeded;
                purchaseRequest.DeliveryMode = deliverymode;
                purchaseRequest.DocsAttached = docsattached;
                purchaseRequest.Status = status;
                purchaseRequest.Total = total;
                purchaseRequest.SubmittedDate = submitteddate;

                purchaseRequest.User = User.Select(purchaseRequest.UserId);
                purchaseRequests.Add(purchaseRequest);
            }
            return purchaseRequests;
        }
        //UPDATE
        public static bool Update(PurchaseRequest purchaseRequests) {
            string Sql = string.Format("UPDATE [PurchaseRequest] Set " +
                        " Id = @id," +
                        " UserId = @userid," +
                        " User = @user," +
                        " Description = @description, " +
                        " Justification = @justification, " +
                        " DateNeeded = @dateneeded, " +
                        " DocsAttached = @docsattached, " +
                        " Status = @status, " +
                        " Total = @total, " +
                        " SubmittedDate = @submitteddate, " +
                        " WHERE ID = @Id; ");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            AddSqlInsertUpdateParameters(Cmd, purchaseRequests);
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
        }
        //DELETE BY ID
        public static bool Delete(int Id) {
            PurchaseRequest purchaseRequest = PurchaseRequest.Select(Id);
            if (purchaseRequest == null) {
                return false;
            }
            bool rc = PurchaseRequest.Delete(purchaseRequest);
            return rc;
        }
        //DELETE
        public static bool Delete(PurchaseRequest purchaseRequest) {
            string Sql = string.Format("DELETE from [PurchaseReauest] WHERE ID = @id");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("id", purchaseRequest.Id));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                return true;
            }
            return recsAffected == 1;
        }

        //INSERT
        public static bool Insert(PurchaseRequest purchaseRequest) {
            string Sql = string.Format("insert into [PurchaseRequest] " +
           "Id, UserId, User, Description, Justification, DateNeeded, DocsAttached, Status, Total, SubmittedDate) " +
           " values" +
           "(@id, @userid, @user, @description, @justification, @dateneeded, @docsattached, @status, @total, @submitteddate)");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            AddSqlInsertUpdateParameters(Cmd, purchaseRequest);
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Insert Failed!");
            }
            return (recsAffected == 1);
        }
        public PurchaseRequest() {
            this.DateNeeded = DateTime.Now.AddDays(7);
            this.DeliveryMode = "USPS";
            this.DocsAttached = false;

        }
        public bool AddLineItem(int ProductID, int Quantity) {
            Product product = Product.Select(ProductID);
            LineItem lineitem = new LineItem {
                PurchaseRequestID = this.Id,
                ProductID = ProductID,
                quantity = Quantity
            };

            bool rc = LineItem.Insert(lineitem);
            if (!rc)
                throw new ApplicationException("Insert failed!");
            this.Total += Quantity * product.Price;
            rc = PurchaseRequest.Update(this);
            return rc;
        }
        public bool DeleteLineItem(int LineItemId) {
            LineItem lineitem = LineItem.Select(LineItemId) {
                if (lineitem == null) {
                    throw new ApplicationException("Line item to delete not found");
                }
                decimal amount = lineitem.Product.Price * lineitem.quantity;
                bool rc = LineItem.Delete(lineitem);
                if (!rc) {
                    throw new ApplicationException("Line item delete failed");
                }
                this.Total -= amount;
                rc = PurchaseRequest.Update(this);
                if (!rc) {
                    throw new ApplicationException("Purchase request update fail");
                }
                return rc;
            }
        public bool UpdateLineItem(int LineItemId, int NewQuantity) {
                LineItem lineItem = LineItem.Select(LineItemId);
                if (lineitem == null) {
                    throw new ApplicationException("Line item to delete not found");
                }
                if(NewQuantity < 0) {
                    throw new ApplicationException("cannot be less than zero!");
                }
                decimal oldAmount = lineItem.Product.Price * lineItem.quantity;
                lineItem.quantity = NewQuantity;
                decimal newAmount = lineItem.Product.Price * lineItem.quantity;
                decimal changeTotal = newAmount - oldAmount;
                

                bool rc = LineItem.Update(lineitem);
                if (!rc)
                    throw new ApplicationException("Insert failed!");
                this.Total += changeTotal;
                rc = PurchaseRequest.Update(this);
                return rc;
            }
        }
    }
}
