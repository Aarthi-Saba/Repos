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
    public class CompanyJobEducationRepository : IDataRepository<CompanyJobEducationPoco>
    {
        private readonly string _connectionStr;
        
        public CompanyJobEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (CompanyJobEducationPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Company_Job_Educations]
                                           ([Id],[Job],[Major],[Importance])
                                           VALUES
                                           (@Id,@Job,@Major,@Importance)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Job", poco.Job);
                    scommand.Parameters.AddWithValue("@Major", poco.Major);
                    scommand.Parameters.AddWithValue("@Importance", poco.Importance);
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

        public IList<CompanyJobEducationPoco> GetAll(params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Job],[Major],[Importance],[Time_Stamp]
                                  FROM [dbo].[Company_Job_Educations]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                CompanyJobEducationPoco[] pocoitems = new CompanyJobEducationPoco[1100];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    CompanyJobEducationPoco poco = new CompanyJobEducationPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Job = readpointer.GetGuid(1);
                    poco.Major = readpointer.GetString(2);
                    poco.Importance = readpointer.GetInt16(3);
                    poco.TimeStamp = (byte[])readpointer[4];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyJobEducationPoco> GetList(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobEducationPoco GetSingle(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobEducationPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobEducationPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Company_Job_Educations] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobEducationPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Company_Job_Educations]
                                           SET [Id] = @Id,[Job] = @Job,[Major] = @Major,[Importance] = @Importance
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Job", poco.Job);
                    scommand.Parameters.AddWithValue("@Major", poco.Major);
                    scommand.Parameters.AddWithValue("@Importance", poco.Importance);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
