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
    public class SystemCountryCodeRepository : IDataRepository<SystemCountryCodePoco>
    {
        private readonly string _connectionStr;
        public SystemCountryCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (SystemCountryCodePoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[System_Country_Codes]
                                           ([Code],[Name])
                                           VALUES
                                           (@Code,@Name)";

                    scommand.Parameters.AddWithValue("@Code", poco.Code);
                    scommand.Parameters.AddWithValue("@Name", poco.Name);
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

        public IList<SystemCountryCodePoco> GetAll(params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Code],[Name]
                                  FROM [dbo].[System_Country_Codes]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                SystemCountryCodePoco[] pocoitems = new SystemCountryCodePoco[20];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    SystemCountryCodePoco poco = new SystemCountryCodePoco();
                    poco.Code = readpointer.GetString(0);
                    poco.Name = readpointer.GetString(1);
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<SystemCountryCodePoco> GetList(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemCountryCodePoco GetSingle(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemCountryCodePoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (SystemCountryCodePoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[System_Country_Codes] WHERE [Code]=@Code";
                    scommand.Parameters.AddWithValue("@Code", poco.Code);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (SystemCountryCodePoco poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[System_Country_Codes]
                                           SET [Code] = @Code,[Name] = @Name 
                                           WHERE [Code]=@Code";

                    scommand.Parameters.AddWithValue("@Code", poco.Code);
                    scommand.Parameters.AddWithValue("@Name", poco.Name);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
