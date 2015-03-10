using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BookDAL
/// </summary>
public static class BookDAL
{
	///<summary>
    ///get the book information from it's ID
    ///</summary>
    ///<param name="bookid">ID of the book</param>
    ///<returns>Book information</returns>
    public static SqlConnection getConnection()
    {
        string s = System.Configuration.ConfigurationManager.ConnectionStrings["database"].ConnectionString;
        SqlConnection connection = new SqlConnection(s);
        return connection;
    }
    public static Book getBookById(int bookid)
    {
        SqlConnection connection = BookDAL.getConnection();
        string sql = "select * from Book where BookId=@id";
        SqlCommand command = new SqlCommand(sql, connection);
        SqlParameter p1 = new SqlParameter("@id", bookid);
        command.Parameters.Add(p1);
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (!reader.Read())
                return null;
            Book book = new Book();
            book.id = bookid;
            book.title = reader["BookTitle"].ToString();
            book.author1 = Convert.ToString(reader["Author1"]);
            book.author2 = Convert.ToString(reader["Author2"]);
            book.author3 = Convert.ToString(reader["Author3"]);
            book.price = Convert.ToDouble(reader["Price"]);
            book.discount = Convert.ToDouble(reader["Discount"]);
            book.realPrice = Convert.ToDouble(reader["RealPrice"]);
            book.publisherID = Convert.ToInt32(reader["PublisherID"]);
            book.publisherName = Convert.ToString(reader["PublisherName"]);
            book.smallImage = Convert.ToString(reader["SmallImage"]);
            book.bigImage = Convert.ToString(reader["BigImage"]);
            book.saleSum = Convert.ToInt32(reader["SaleSum"]);
            book.saleSum = Convert.ToInt32(reader["SaleSum"]);
            book.clickCount = Convert.ToInt32(reader["ClickCount"]);
            book.content = Convert.ToString(reader["Contents"]);
            book.description = Convert.ToString(reader["Description"]);
            book.publishDate = Convert.ToDateTime(reader["PublishDate"]);
            reader.Close();
            return book;
        }
    }
    ///<summary>
    ///construct where query based on the conditions
    ///</summary>
    ///<param name="criterion">conditions</param>
    ///<returns>where query</returns>
    private static string buildWhereString(SearchBookCriterion criterion)
    {
        string sql = " ";
        string temp = null;
        if (!string.IsNullOrEmpty(criterion.title))
        {
            temp = string.Format(" and (BookTitle like '%{0}%') ", criterion.title);
            sql += temp;
        }
        if (!string.IsNullOrEmpty(criterion.author))
        {
            temp = string.Format(" and (Author1 like '%{0}%' "
                + " or Author2 like '%{0}%' "
                + " or Author3 like '%{0}%' ) ", criterion.author);
            sql += temp;
        }
        if (criterion.date1.HasValue && criterion.date2.HasValue)
        {
            temp = string.Format(" and (PublishDate between '{0}' and '{1}') ", criterion.date1.Value, criterion.date2.Value);
            sql += temp;
        }
        if (criterion.price1.HasValue && criterion.price2.HasValue)
        {
            temp = string.Format(" and (Price between '{0}' and '{1}') ", criterion.price1, criterion.price2);
            sql += temp;
        }
        if (!string.IsNullOrEmpty(criterion.publisher))
        {
            temp = string.Format(" and (PublisherName like '%{0}%') ", criterion.publisher);
            sql += temp;
        }
    }
}