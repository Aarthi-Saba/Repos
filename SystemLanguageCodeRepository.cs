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
    public class SystemLanguageCodeRepository : IDataRepository<SystemLanguageCodePoco>
    {
        private readonly string _connectionStr;
        public SystemLanguageCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[System_Language_Codes]
                                           ([LanguageID],[Name],[Native_Name])
                                           VALUES
                                           (@LanguageID,@Name,@Native_Name)";

                    scommand.Parameters.AddWithValue("@LanguageID", poco.LanguageID);
                    scommand.Parameters.AddWithValue("@Name", poco.Name);
                    scommand.Parameters.AddWithValue("@Native_Name", poco.NativeName);
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

        public IList<SystemLanguageCodePoco> GetAll(params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [LanguageID],[Name],[Native_Name]
                                  FROM [dbo].[System_Language_Codes]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                SystemLanguageCodePoco[] pocoitems = new SystemLanguageCodePoco[10];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    SystemLanguageCodePoco poco = new SystemLanguageCodePoco();
                    poco.LanguageID = readpointer.GetString(0);
                    poco.Name = readpointer.GetString(1);
                    poco.NativeName = readpointer.GetString(2);
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<SystemLanguageCodePoco> GetList(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemLanguageCodePoco GetSingle(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemLanguageCodePoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[System_Language_Codes] WHERE [LanguageID]=@LanguageID";
                    scommand.Parameters.AddWithValue("@LanguageID", poco.LanguageID);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[System_Language_Codes]
                                           SET [LanguageID] = @LanguageID,[Name] = @Name,[Native_Name] = @Native_Name 
                                           WHERE [LanguageID]=@LanguageID";

                    scommand.Parameters.AddWithValue("@LanguageID", poco.LanguageID);
                    scommand.Parameters.AddWithValue("@Name", poco.Name);
                    scommand.Parameters.AddWithValue("@Native_Name", poco.NativeName);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
