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
    public class CompanyDescriptionRepository : IDataRepository<CompanyDescriptionPoco>
    {
        private readonly string _connectionStr;
        public CompanyDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyDescriptionPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyDescriptionPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Company_Descriptions]
                                           ([Id],[Company],[LanguageID],[Company_Name],[Company_Description])
                                           VALUES
                                           (@Id,@Company,@LanguageID,@Company_Name,@Company_Description)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Company", poco.Company);
                    scommand.Parameters.AddWithValue("@LanguageID", poco.LanguageId);
                    scommand.Parameters.AddWithValue("@Company_Name", poco.CompanyName);
                    scommand.Parameters.AddWithValue("@Company_Description", poco.CompanyDescription);
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

        public IList<CompanyDescriptionPoco> GetAll(params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Company],[LanguageID],[Company_Name],[Company_Description],[Time_Stamp]
                                       FROM [JobPortal_Local].[dbo].[Company_Descriptions]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                CompanyDescriptionPoco[] pocoitems = new CompanyDescriptionPoco[700];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    CompanyDescriptionPoco poco = new CompanyDescriptionPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Company = readpointer.GetGuid(1);
                    poco.LanguageId = readpointer.GetString(2);
                    poco.CompanyName = readpointer.GetString(3);
                    poco.CompanyDescription = readpointer.GetString(4);
                    poco.TimeStamp = (byte[])readpointer[5];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyDescriptionPoco> GetList(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyDescriptionPoco GetSingle(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyDescriptionPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyDescriptionPoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Company_Descriptions] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyDescriptionPoco poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Company_Descriptions]
                                           SET [Id] = @Id,[Company] = @Company,[LanguageID] = @LanguageID,
                                           [Company_Name] = @Company_Name,[Company_Description] = @Company_Description
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Company", poco.Company);
                    scommand.Parameters.AddWithValue("@LanguageID", poco.LanguageId);
                    scommand.Parameters.AddWithValue("@Company_Name", poco.CompanyName);
                    scommand.Parameters.AddWithValue("@Company_Description", poco.CompanyDescription);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
