using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ShopCartItem
/// </summary>
public class ShopCartItem
{
    public Book book { get; set; }
    public int number { get; set; }
    public int bookId { get { return book.id; } }
    public ShopCartItem() { }
    public ShopCartItem(Book theBook, int num)
    {
        book = theBook;
        number = num;
    }
}