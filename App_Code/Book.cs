using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Book
/// </summary>
public class Book
{
    public int id { get; set; }
    public string title { get; set; }
    public double price { get; set; }
    public double discount { get; set; }
    public double realPrice { get; set; }
    public string author1 { get; set; }
    public string author2 { get; set; }
    public string author3 { get; set; }
    public string smallImage { get; set; }
    public string bigImage { get; set; }
    public string content { get; set; }
    public string description { get; set; }
    public int clickCount { get; set; }
    public int saleSum { get; set; }
    public int publisherID { get; set; }
    public string publisherName { get; set; }
    public DateTime publishDate { get; set; }
}