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
    public class CompanyProfileRepository : IDataRepository<CompanyProfilePoco>
    {
        private readonly string _connectionStr;
        public CompanyProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (CompanyProfilePoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Company_Profiles]
                                           ([Id],[Registration_Date],[Company_Website],[Contact_Phone],[Contact_Name],[Company_Logo])
                                           VALUES
                                           (@Id,@Registration_Date,@Company_Website,@Contact_Phone,@Contact_Name,@Company_Logo)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Registration_Date", poco.RegistrationDate);
                    scommand.Parameters.AddWithValue("@Company_Website", poco.CompanyWebsite);
                    scommand.Parameters.AddWithValue("@Contact_Phone", poco.ContactPhone);
                    scommand.Parameters.AddWithValue("@Contact_Name", poco.ContactName);
                    scommand.Parameters.AddWithValue("@Company_Logo", poco.CompanyLogo);
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

        public IList<CompanyProfilePoco> GetAll(params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Registration_Date],[Company_Website],[Contact_Phone],[Contact_Name],
                                  [Company_Logo],[Time_Stamp]
                                  FROM [dbo].[Company_Profiles]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                CompanyProfilePoco[] pocoitems = new CompanyProfilePoco[300];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    CompanyProfilePoco poco = new CompanyProfilePoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.RegistrationDate = readpointer.GetDateTime(1);
                    poco.CompanyWebsite = readpointer.IsDBNull(2) ? null : readpointer.GetString(2);
                    poco.ContactPhone = readpointer.GetString(3);
                    poco.ContactName = readpointer.IsDBNull(4) ? null : readpointer.GetString(4);
                    poco.CompanyLogo = readpointer.IsDBNull(5) ? null : (byte[])readpointer[5]; ;
                    poco.TimeStamp = (byte[])readpointer[6];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyProfilePoco> GetList(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyProfilePoco GetSingle(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyProfilePoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Company_Profiles] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Company_Profiles]
                                           SET [Id] = @Id, [Registration_Date] = @Registration_Date,
                                           [Company_Website] = @Company_Website,[Contact_Phone] = @Contact_Phone,
                                           [Contact_Name] = @Contact_Name,[Company_Logo] = @Company_Logo
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Registration_Date", poco.RegistrationDate);
                    scommand.Parameters.AddWithValue("@Company_Website", poco.CompanyWebsite);
                    scommand.Parameters.AddWithValue("@Contact_Phone", poco.ContactPhone);
                    scommand.Parameters.AddWithValue("@Contact_Name", poco.ContactName);
                    scommand.Parameters.AddWithValue("@Company_Logo", poco.CompanyLogo);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
