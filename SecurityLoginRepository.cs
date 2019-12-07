using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace CareerCloud.ADODataAccessLayer
{
    public class SecurityLoginRepository : IDataRepository<SecurityLoginPoco>
    {
        private readonly string _connectionStr;
        public SecurityLoginRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (SecurityLoginPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Security_Logins]
                                           ([Id],[Login],[Password],[Created_Date],[Password_Update_Date],
                                           [Agreement_Accepted_Date],[Is_Locked],[Is_Inactive],[Email_Address],
                                           [Phone_Number],[Full_Name],[Force_Change_Password],[Prefferred_Language])
                                           VALUES
                                           (@Id,@Login,@Password,@Created_Date,@Password_Update_Date,@Agreement_Accepted_Date,
                                           @Is_Locked,@Is_Inactive,@Email_Address,@Phone_Number,@Full_Name,@Force_Change_Password,
                                           @Prefferred_Language)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Password", poco.Password);
                    scommand.Parameters.AddWithValue("@Created_Date", poco.Created);
                    scommand.Parameters.AddWithValue("@Password_Update_Date", poco.PasswordUpdate);
                    scommand.Parameters.AddWithValue("@Agreement_Accepted_Date", poco.AgreementAccepted);
                    scommand.Parameters.AddWithValue("@Is_Locked", poco.IsLocked);
                    scommand.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);
                    scommand.Parameters.AddWithValue("@Email_Address", poco.EmailAddress);
                    scommand.Parameters.AddWithValue("@Phone_Number", poco.PhoneNumber);
                    scommand.Parameters.AddWithValue("@Full_Name", poco.FullName);
                    scommand.Parameters.AddWithValue("@Force_Change_Password", poco.ForceChangePassword);
                    scommand.Parameters.AddWithValue("@Prefferred_Language", poco.PrefferredLanguage);
                    connection.Open();
                    int rows_affected = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginPoco> GetAll(params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Login],[Password],[Created_Date],[Password_Update_Date],
                                  [Agreement_Accepted_Date],[Is_Locked],[Is_Inactive],[Email_Address],[Phone_Number],
                                  [Full_Name],[Force_Change_Password],[Prefferred_Language],[Time_Stamp]
                                  FROM [dbo].[Security_Logins]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                SecurityLoginPoco[] pocoitems = new SecurityLoginPoco[150];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    SecurityLoginPoco poco = new SecurityLoginPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Login = readpointer.GetString(1);
                    poco.Password = readpointer.GetString(2);
                    poco.Created = readpointer.GetDateTime(3);
                    poco.PasswordUpdate = readpointer.IsDBNull(4) ? (DateTime?)null : readpointer.GetDateTime(4);
                    poco.AgreementAccepted = readpointer.IsDBNull(5) ? (DateTime?) null : readpointer.GetDateTime(5);
                    poco.IsLocked = readpointer.GetBoolean(6);
                    poco.IsInactive = readpointer.GetBoolean(7);
                    poco.EmailAddress = readpointer.GetString(8);
                    poco.PhoneNumber = readpointer.IsDBNull(9) ? null : readpointer.GetString(9);
                    poco.FullName = readpointer.IsDBNull(10) ? null : readpointer.GetString(10);
                    poco.ForceChangePassword = readpointer.GetBoolean(11);
                    poco.PrefferredLanguage = readpointer.IsDBNull(12) ? null : readpointer.GetString(12);
                    poco.TimeStamp = (byte[])readpointer[13];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<SecurityLoginPoco> GetList(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginPoco GetSingle(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Security_Logins] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Security_Logins]
                                           SET [Id] = @Id,[Login] = @Login,[Password] = @Password,
                                           [Created_Date] = @Created_Date,[Password_Update_Date] = @Password_Update_Date,
                                           [Agreement_Accepted_Date] = @Agreement_Accepted_Date,[Is_Locked] = @Is_Locked,
                                           [Is_Inactive] = @Is_Inactive,[Email_Address] = @Email_Address,
                                           [Phone_Number] = @Phone_Number,[Full_Name] = @Full_Name,
                                           [Force_Change_Password] = @Force_Change_Password,[Prefferred_Language] = @Prefferred_Language
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Password", poco.Password);
                    scommand.Parameters.AddWithValue("@Created_Date", poco.Created);
                    scommand.Parameters.AddWithValue("@Password_Update_Date", poco.PasswordUpdate);
                    scommand.Parameters.AddWithValue("@Agreement_Accepted_Date", poco.AgreementAccepted);
                    scommand.Parameters.AddWithValue("@Is_Locked", poco.IsLocked);
                    scommand.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);
                    scommand.Parameters.AddWithValue("@Email_Address", poco.EmailAddress);
                    scommand.Parameters.AddWithValue("@Phone_Number", poco.PhoneNumber);
                    scommand.Parameters.AddWithValue("@Full_Name", poco.FullName);
                    scommand.Parameters.AddWithValue("@Force_Change_Password", poco.ForceChangePassword);
                    scommand.Parameters.AddWithValue("@Prefferred_Language", poco.PrefferredLanguage);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
