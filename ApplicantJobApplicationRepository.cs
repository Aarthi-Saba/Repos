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
    public class ApplicantJobApplicationRepository : IDataRepository<ApplicantJobApplicationPoco>
    {
        private readonly string _connectionStr;
        public ApplicantJobApplicationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (ApplicantJobApplicationPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Applicant_Job_Applications]
                                           ([Id],[Applicant],[Job],[Application_Date])
                                           VALUES
                                           (@Id,@Applicant,@Job,@Application_Date)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Job", poco.Job);
                    scommand.Parameters.AddWithValue("@Application_Date", poco.ApplicationDate);
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

        public IList<ApplicantJobApplicationPoco> GetAll(params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                scommand.CommandText = @"SELECT [Id],[Applicant],[Job],[Application_Date],[Time_Stamp]
                                        FROM [dbo].[Applicant_Job_Applications]";
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                ApplicantJobApplicationPoco[] pocoitems = new ApplicantJobApplicationPoco[500];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    ApplicantJobApplicationPoco poco = new ApplicantJobApplicationPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Applicant = readpointer.GetGuid(1);
                    poco.Job = readpointer.GetGuid(2);
                    poco.ApplicationDate = readpointer.GetDateTime(3);
                    poco.TimeStamp = (byte[])readpointer[4];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantJobApplicationPoco> GetList(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantJobApplicationPoco GetSingle(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
                IQueryable<ApplicantJobApplicationPoco> pocoitems = GetAll().AsQueryable();
                return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Applicant_Job_Applications] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public void Update(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Applicant_Job_Applications]
                                            SET [Id] = @Id,[Applicant] = @Applicant,[Job] = @Job,[Application_Date] = @Application_Date
                                            WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Job", poco.Job);
                    scommand.Parameters.AddWithValue("@Application_Date", poco.ApplicationDate);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
