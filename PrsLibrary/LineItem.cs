using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PrsLibrary {
    public class LineItem : PrsTables {
        public int Id { get; set; }
        public int PurchaseRequestID { get; set; }
        public PurchaseRequest PurchaseRequest { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; private set; }
        public int quantity { get; set; }


        private static void AddSqlInsertUpdateParameters(SqlCommand Cmd, LineItem lineItem) {
            Cmd.Parameters.Add(new SqlParameter("@PurchaseRequestID", lineItem.PurchaseRequestID));
            Cmd.Parameters.Add(new SqlParameter("@ProductID", lineItem.ProductID));
            Cmd.Parameters.Add(new SqlParameter("@Quantity", lineItem.quantity));
        }
        //INSERT
        public static bool Insert(LineItem lineItem) {
            string Sql = string.Format("insert into [LineItem] " +
           "LineItemID, LineItemID, Quantity) " +
           " values" +
           "(@lineitemid, @lineitemid, @quantity)");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            AddSqlInsertUpdateParameters(Cmd, lineItem);
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Insert Failed!");
            }
            lineItem.PurchaseRequest = PurchaseRequest.Select(lineItem.PurchaseRequestID);
            lineItem.Product = Product.Select(lineItem.ProductID);

            return (recsAffected == 1);
        }
        //UPDATE
        public static bool Update(LineItem lineItem) {
            string Sql = string.Format("UPDATE [PurchaseRequest] Set " +
                        " PurchaseRequestID = @purchaserequestid," +
                        " LineItemID = @lineitemid," +
                        " Quantity = @quantity " +
                        " WHERE ID = @Id; ");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            AddSqlInsertUpdateParameters(Cmd, lineItem);
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                throw new ApplicationException("Update Failed!");
            }
            return (recsAffected == 1);
        }
        //Delete
        public static bool Delete(LineItem lineitem) {
            string Sql = string.Format("DELETE from [LineItem] WHERE ID = @id");
            string ConnStr = @"Server=STUDENT05;Database=prs;Trusted_Connection=True;";
            SqlConnection Conn = new SqlConnection(ConnStr);
            Conn.Open();
            if (Conn.State != System.Data.ConnectionState.Open) {
                throw new ApplicationException("Connection didn't open");
            }
            SqlCommand Cmd = new SqlCommand(Sql, Conn);
            Cmd.Parameters.Add(new SqlParameter("id", lineitem.Id));
            int recsAffected = Cmd.ExecuteNonQuery();
            if (recsAffected != 1) {
                return true;
            }
            return recsAffected == 1;
        }

        //Select
        public static LineItemCollection Select(string whereClause, string orderByClause) {
            string Sql = string.Format("SELECT * from [LineItem] WHERE {0} ORDER BY {1}", whereClause, orderByClause);
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
            LineItemCollection lineitems = new LineItemCollection();
            while (Reader.Read()) {
                int id = Reader.GetInt32(Reader.GetOrdinal("id"));
                int purchaseRequestId = Reader.GetInt32(Reader.GetOrdinal("PurchaseRequestId"));
                int productId = Reader.GetInt32(Reader.GetOrdinal("ProductID"));
                int quantity = Reader.GetInt32(Reader.GetOrdinal("Quantity"));

                LineItem lineitem = new LineItem();
                lineitem.Id = id;
                lineitem.PurchaseRequestID = purchaseRequestId;
                lineitem.ProductID = productId;
                lineitem.quantity = quantity;


                lineitems.Add(lineitem);
            }
            return lineitems;
        }
        public static LineItem Select(int Id) {
            LineItemCollection lineItems = LineItem.Select($"Id = {Id}", "Id");
            LineItem lineItem = (lineItems.Count == 1) ? lineItems[0] : null;
            return lineItem;
        }
        public static bool Delete(int Id) {
            LineItem lineItem = LineItem.Select(Id);
            if (lineItem == null) {
                return false;
            }
            bool rc = LineItem.Delete(lineItem);
            return rc;
        }
        public LineItem() {
            this.quantity = 1;
        }
    }
}
