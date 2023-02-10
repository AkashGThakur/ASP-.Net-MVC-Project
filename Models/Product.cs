using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProductApp.Models
{
    public class Product
    {
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string ProductName { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Please enter correct rate")]
        public decimal Rate { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter description")]
        [StringLength(200, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Description { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Please enter category")]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string CategoryName { get; set; }

        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=cdac;Integrated Security=True";
                cn.Open();

                SqlCommand cmdSelectAll = new SqlCommand();
                cmdSelectAll.Connection = cn;
                cmdSelectAll.CommandType = CommandType.StoredProcedure;
                cmdSelectAll.CommandText = "GetAllProducts";

                SqlDataReader dr = cmdSelectAll.ExecuteReader();
                while(dr.Read())
                {
                    products.Add(new Product {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Rate = Convert.ToDecimal(dr["Rate"]),
                        Description = Convert.ToString(dr["Description"]),
                        CategoryName = Convert.ToString(dr["CategoryName"])
                    }); ;
                }
                dr.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }

            return products;
        }
    
        public static Product GetOneProduct(int productId)
        {
            Product product = new Product();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=cdac;Integrated Security=True";
                cn.Open();

                SqlCommand cmdSelectOne = new SqlCommand();
                cmdSelectOne.Connection = cn;
                cmdSelectOne.CommandType = CommandType.StoredProcedure;
                cmdSelectOne.CommandText = "GetOneProduct";
                cmdSelectOne.Parameters.AddWithValue("@ProductId", productId);

                SqlDataReader dr = cmdSelectOne.ExecuteReader();
                if (dr.Read())
                {
                    product.ProductId = productId;
                    product.ProductName = Convert.ToString(dr["ProductName"]);
                    product.Rate = Convert.ToDecimal(dr["Rate"]);
                    product.Description = Convert.ToString(dr["Description"]);
                    product.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                else
                {
                    product = null; // not found
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }
            return product;
        }
    
        public static void Update(Product product)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=cdac;Integrated Security=True";
                cn.Open();

                SqlCommand cmdInsert = new SqlCommand();
                cmdInsert.Connection = cn;
                cmdInsert.CommandType = CommandType.StoredProcedure;
                cmdInsert.CommandText = "UpdateProduct";

                cmdInsert.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmdInsert.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmdInsert.Parameters.AddWithValue("@Rate", product.Rate);
                cmdInsert.Parameters.AddWithValue("@Description", product.Description);
                cmdInsert.Parameters.AddWithValue("@CategoryName", product.CategoryName);

                cmdInsert.ExecuteNonQuery();
                //Console.WriteLine("Update success");
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                cn.Close();
            }
        }
    }
    }
