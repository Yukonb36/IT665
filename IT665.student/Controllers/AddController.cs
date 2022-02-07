using IT665.Attributes;
using IT665.Models.Add;
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
    public class AddController : BaseController
    {
        [RequiresRole("CustomerManager")]
        public ActionResult CustomerAdd()
        {
            return View(new Models.Add.CustomerAdd());
        }


        IEnumerable<DtoType> queryAndLoadDtoCustomer<DtoType>(string sql, NameValueCollection parameters, Func<SqlDataReader, DtoType> createDtoFunc)
        {
            var connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["AdventureWorks2012"].ConnectionString;
            var sqlQuery = sql;
            var sqlConnection = new SqlConnection(connectionString);
            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            foreach (var p in parameters.AllKeys)
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

        public ActionResult CustomerAdd(Models.Add.CustomerAdd model)
        {
            if (ModelState.IsValid)
            {

                var sqlQuery = "INSERT INTO AdventureWorks2012.Person.BusinessEntity ([rowguid],[ModifiedDate]) VALUES (NEWID(), getdate()); INSERT INTO [Person].[Person] ([BusinessEntityID] ,[PersonType],[NameStyle],[Title],[FirstName],[MiddleName],[LastName],[Suffix],[EmailPromotion],[AdditionalContactInfo],[Demographics],[rowguid],[ModifiedDate]) VALUES ((select top 1 BusinessEntityID FROM [AdventureWorks2012].person.BusinessEntity order by BusinessEntityID desc),'IN',@nameStyle,@title,@firstName,@middleName,@lastName,@suffix,@emailPromotion,NULL,NULL,newid(),getdate()); select top 1 firstName, lastName from adventureworks2012.person.person order by businessentityid desc;";
   
                var parameters = new NameValueCollection();
                parameters.Add("@nameStyle", string.Format("{0}", model.nameStyle));
                parameters.Add("@title", string.Format("{0}", model.title));
                parameters.Add("@firstName", string.Format("{0}", model.firstName));
                parameters.Add("@middleName", string.Format("{0}", model.middleName));
                parameters.Add("@lastName", string.Format("{0}", model.lastName));
                parameters.Add("@suffix", string.Format("{0}", model.suffix));
                parameters.Add("@emailPromotion", string.Format("{0}", model.emailPromotion));
                
                model.Results = queryAndLoadDtoCustomer(sqlQuery, parameters,
                    (reader) => new CustomerAddResults
                    {
                        firstName = (string)reader["firstName"],
                        lastName = (string)reader["lastName"]
                    }).ToList();
            }
            return View(model);
        }
    }
}