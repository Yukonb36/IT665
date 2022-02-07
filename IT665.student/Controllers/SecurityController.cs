using IT665.Models.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IT665.Controllers
{
    public class SecurityController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View(new Login());
        }

        public ActionResult Logout()
        {
            Session["LoginToken"] = null;
            Session["Roles"] = null;
            Response.Redirect("/Security/Index");
            return null;
        }

        [HttpPost]
        public ActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {

                // authenticate user credentials and get roles
                var connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["AdventureWorks2012"].ConnectionString;
                var sqlQuery = "select password, salt from [Security].[User] where username = @UserName;select role from [Security].[UserRole] where userid = (select userid from [Security].[User] where username = @UserName)";
                var sqlConnection = new SqlConnection(connectionString);
                DataSet ds = new DataSet();
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@UserName", string.Format("{0}", model.UserName));
                    using (SqlDataReader dr = sqlCommand.ExecuteReader())
                    {
                        ds.Load(dr, LoadOption.OverwriteChanges, "User", "Roles");
                    }
                }

                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var salt = ds.Tables[0].Rows[0]["salt"].ToString();
                        var password = ds.Tables[0].Rows[0]["password"].ToString();
                        if (GenerateHash(model.Password, salt) == password)
                        {
                            Session["LoginToken"] = Guid.NewGuid();

                            // load roles
                            Session["Roles"] = ds.Tables[1].AsEnumerable()
                                .Select(r => r.Field<string>("Role")).ToArray();

                            // redirect to app landing page 
                            Response.Redirect("/Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();
                }
                ModelState.AddModelError("Login", "Login Failed");
            }
            return View(model);
        }


        private static string GenerateHash(string password, string salt)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(salt + password);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }


        static Random random = new Random();
        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public JsonResult GetHash(string password)
        {
            // create a random 8 char hash
            var salt = RandomString (8);
            var hash = GenerateHash(password, salt);
            return Json(new { salt = salt, hash = hash }, JsonRequestBehavior.AllowGet);
        }
        //private static byte[] GetSalt(int maximumSaltLength)
        //{
        //    var salt = new byte[maximumSaltLength];
        //    using (var random = new RNGCryptoServiceProvider())
        //    {
        //        random.GetNonZeroBytes(salt);
        //    }

        //    return salt;
        //}

        //static byte[] Hash(string value, string salt)
        //{
        //    return Hash(Encoding.ASCII.GetBytes(value), Encoding.ASCII.GetBytes(salt));
        //}

        //static byte[] Hash(byte[] value, byte[] salt)
        //{
        //    byte[] saltedValue = value.Concat(salt).ToArray();
        //    return new SHA256Managed().ComputeHash(saltedValue);
        //}

        //static bool CheckAuthentication(string userPass, string storedPass, string salt)
        //{
        //    var storedHash = Hash(storedPass, salt);
        //    var userHash = Hash(userPass, salt);
        //    return storedHash.SequenceEqual(userHash);
        //}
    }
}