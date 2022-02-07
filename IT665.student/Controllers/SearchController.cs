using IT665.Attributes;
using IT665.Models.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT665.Controllers
{
    [SessionAuthorize]
    public class SearchController : BaseController
    {
        [RequiresRole("CustomerManager")]
        public ActionResult Customer()
        {
            return View(new CustomerSearch());
        }

        [RequiresRole("ProductManager")]
        public ActionResult Product()
        {
            return View(new ProductSearch());
        }


        IEnumerable<DtoType> queryAndLoadDto<DtoType>(string sql, NameValueCollection parameters, Func<SqlDataReader, DtoType> createDtoFunc)
        {
            var connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["AdventureWorks2012"].ConnectionString;
            var sqlQuery = sql;
            var sqlConnection = new SqlConnection(connectionString);
            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            foreach(var p in parameters.AllKeys)
                sqlCommand.Parameters.AddWithValue(p, parameters[p]);
            try
            {
                sqlConnection.Open();
                var reader = sqlCommand.ExecuteReader();
                var ret = new List<DtoType>();
                while (reader.Read())
                    ret.Add(createDtoFunc(reader));
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        [RequiresRole("CustomerManager")]
        [HttpPost]
        public ActionResult Customer(CustomerSearch model)
        {
            if (ModelState.IsValid)
            {

                var sqlQuery = "select distinct LastName, FirstName, BusinessEntityID from AdventureWorks2012.Person.Person where PersonType in ('IN', 'SC') and LastName like @PartialLastName order by LastName, FirstName";
                var parameters = new NameValueCollection();
                parameters.Add("@PartialLastName", string.Format("{0}%", model.PartialLastName));
                model.Results = queryAndLoadDto(sqlQuery, parameters,
                    (reader) => new CustomerSearchResult
                    {
                        BusinessEntityId = (int)reader["BusinessEntityId"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"]
                    }).ToList();
            }
            return View(model);
        }


        [RequiresRole("ProductManager")]
        [HttpPost]
        public ActionResult Product(ProductSearch model)
        {
            if (ModelState.IsValid)
            {

                var sqlQuery = "select distinct Name, ProductNumber, ProductId from AdventureWorks2012.Production.Product where Name like @PartialProductName order by Name";
                var parameters = new NameValueCollection();
                parameters.Add("@PartialProductName", string.Format("%{0}%", model.PartialProductName));
                model.Results = queryAndLoadDto(sqlQuery, parameters,
                    (reader) => new ProductSearchResult
                    {
                        Name = (string)reader["Name"],
                        ProductNumber = (string)reader["ProductNumber"],
                        ProductId = (int)reader["ProductId"]
                    }).ToList();
            }
            return View(model);
        }

        // PRODUCT IMPLEMENTATION TODO: Add Product() method.  Use Customer() method as a guide.

        // PRODUCT IMPLEMENTATION TODO: Add Product(ProductSearch model) method.  Use Customer(CustomerSearch model) method as a guide.
    }
}