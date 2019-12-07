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
    public class CompanyJobDescriptionRepository : IDataRepository<CompanyJobDescriptionPoco>
    {
        private readonly string _connectionStr;
        public CompanyJobDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyJobDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (CompanyJobDescriptionPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Company_Jobs_Descriptions]
                                           ([Id],[Job],[Job_Name],[Job_Descriptions])
                                           VALUES
                                           (@Id,@Job, @Job_Name,@Job_Descriptions)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Job", poco.Job);
                    scommand.Parameters.AddWithValue("@Job_Name", poco.JobName);
                    scommand.Parameters.AddWithValue("@Job_Descriptions", poco.JobDescriptions);
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

        public IList<CompanyJobDescriptionPoco> GetAll(params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Job],[Job_Name],[Job_Descriptions],[Time_Stamp]
                                  FROM [dbo].[Company_Jobs_Descriptions]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                CompanyJobDescriptionPoco[] pocoitems = new CompanyJobDescriptionPoco[1200];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    CompanyJobDescriptionPoco poco = new CompanyJobDescriptionPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Job = readpointer.GetGuid(1);
                    poco.JobName = readpointer.IsDBNull(2) ? null : readpointer.GetString(2);
                    poco.JobDescriptions = readpointer.IsDBNull(3) ? null: readpointer.GetString(3);
                    poco.TimeStamp = (byte[])readpointer[4];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyJobDescriptionPoco> GetList(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobDescriptionPoco GetSingle(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobDescriptionPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Company_Jobs_Descriptions] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Company_Jobs_Descriptions] SET [Id] = @Id,[Job] = @Job,
                                           [Job_Name] = @Job_Name,[Job_Descriptions] = @Job_Descriptions
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Job", poco.Job);
                    scommand.Parameters.AddWithValue("@Job_Name", poco.JobName);
                    scommand.Parameters.AddWithValue("@Job_Descriptions", poco.JobDescriptions);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
