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
    public class ApplicantWorkHistoryRepository : IDataRepository<ApplicantWorkHistoryPoco>
    {
        private readonly string _connectionStr;
        public ApplicantWorkHistoryRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (ApplicantWorkHistoryPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Applicant_Work_History]
                                           ([Id],[Applicant],[Company_Name],[Country_Code],[Location],[Job_Title],
                                           [Job_Description],[Start_Month],[Start_Year],[End_Month],[End_Year])
                                           VALUES
                                           (@Id,@Applicant,@Company_Name,@Country_Code,@Location,@Job_Title,
                                            @Job_Description,@Start_Month,@Start_Year,@End_Month,@End_Year)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Company_Name", poco.CompanyName);
                    scommand.Parameters.AddWithValue("@Country_Code", poco.CountryCode);
                    scommand.Parameters.AddWithValue("@Location", poco.Location);
                    scommand.Parameters.AddWithValue("@Job_Title", poco.JobTitle);
                    scommand.Parameters.AddWithValue("@Job_Description", poco.JobDescription);
                    scommand.Parameters.AddWithValue("@Start_Month", poco.StartMonth);
                    scommand.Parameters.AddWithValue("@Start_Year", poco.StartYear);
                    scommand.Parameters.AddWithValue("@End_Month", poco.EndMonth);
                    scommand.Parameters.AddWithValue("@End_Year", poco.EndYear);
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

        public IList<ApplicantWorkHistoryPoco> GetAll(params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                scommand.CommandText = @"SELECT [Id],[Applicant],[Company_Name],[Country_Code],[Location],[Job_Title],
                                       [Job_Description],[Start_Month],[Start_Year],[End_Month],[End_Year],[Time_Stamp]
                                       FROM [dbo].[Applicant_Work_History]";
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                ApplicantWorkHistoryPoco[] pocoitems = new ApplicantWorkHistoryPoco[500];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    ApplicantWorkHistoryPoco poco = new ApplicantWorkHistoryPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Applicant = readpointer.GetGuid(1);
                    poco.CompanyName = readpointer.GetString(2);
                    poco.CountryCode = readpointer.GetString(3);
                    poco.Location = readpointer.GetString(4);
                    poco.JobTitle = readpointer.GetString(5);
                    poco.JobDescription = readpointer.GetString(6);
                    poco.StartMonth = readpointer.GetInt16(7);
                    poco.StartYear = readpointer.GetInt32(8);
                    poco.EndMonth = readpointer.GetInt16(9);
                    poco.EndYear = readpointer.GetInt32(10);
                    poco.TimeStamp = (byte[])readpointer[11];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantWorkHistoryPoco> GetList(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantWorkHistoryPoco GetSingle(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantWorkHistoryPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantWorkHistoryPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Applicant_Work_History] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantWorkHistoryPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Applicant_Work_History] SET [Id] = @Id,[Applicant] = @Applicant,
                                           [Company_Name] = @Company_Name,[Country_Code] = @Country_Code,[Location] = @Location,
                                           [Job_Title] = @Job_Title,[Job_Description] = @Job_Description,[Start_Month] = @Start_Month,
                                           [Start_Year] = @Start_Year,[End_Month] = @End_Month,[End_Year] = @End_Year
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Company_Name", poco.CompanyName);
                    scommand.Parameters.AddWithValue("@Country_Code", poco.CountryCode);
                    scommand.Parameters.AddWithValue("@Location", poco.Location);
                    scommand.Parameters.AddWithValue("@Job_Title", poco.JobTitle);
                    scommand.Parameters.AddWithValue("@Job_Description", poco.JobDescription);
                    scommand.Parameters.AddWithValue("@Start_Month", poco.StartMonth);
                    scommand.Parameters.AddWithValue("@Start_Year", poco.StartYear);
                    scommand.Parameters.AddWithValue("@End_Month", poco.EndMonth);
                    scommand.Parameters.AddWithValue("@End_Year", poco.EndYear);

                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
