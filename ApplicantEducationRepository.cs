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
    public class ApplicantEducationRepository : IDataRepository<ApplicantEducationPoco>
    {
        private readonly string _connectionStr;
        //Data Source=DESKTOP-LC31E5U\HUMBERBRIDGING;Initial Catalog=JobPortal_Local;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        public ApplicantEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }


        public void Add(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (ApplicantEducationPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO[dbo].[Applicant_Educations]
                                          ([Id],[Applicant],[Major],[Certificate_Diploma],[Start_Date],
                                           [Completion_Date],[Completion_Percent])
                                          VALUES
                                          (@Id,@Applicant,@Major,@Certificate_Diploma,@Start_Date,
                                            @Completion_Date,@Completion_Percent)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Major", poco.Major);
                    scommand.Parameters.AddWithValue("@Certificate_Diploma", poco.CertificateDiploma);
                    scommand.Parameters.AddWithValue("@Start_Date", poco.StartDate);
                    scommand.Parameters.AddWithValue("@Completion_Date", poco.CompletionDate);
                    scommand.Parameters.AddWithValue("@Completion_Percent", poco.CompletionPercent);
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

        public IList<ApplicantEducationPoco> GetAll(params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Applicant],[Major],[Certificate_Diploma],[Start_Date],[Completion_Date],
                                       [Completion_Percent],[Time_Stamp]
                                       FROM [dbo].[Applicant_Educations]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                ApplicantEducationPoco[] pocoitems = new ApplicantEducationPoco[500];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    ApplicantEducationPoco poco = new ApplicantEducationPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Applicant = readpointer.GetGuid(1);
                    poco.Major = readpointer.GetString(2);
                    poco.CertificateDiploma = readpointer.IsDBNull(3) ? null : readpointer.GetString(3);
                    poco.StartDate = readpointer.IsDBNull(4) ? (DateTime?)null : readpointer.GetDateTime(4);
                    poco.CompletionDate = readpointer.IsDBNull(5) ? (DateTime?) null : readpointer.GetDateTime(5);
                    poco.CompletionPercent = readpointer.IsDBNull(6) ? (byte?) null : readpointer.GetByte(6);
                    poco.TimeStamp = (byte[])readpointer[7];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantEducationPoco> GetList(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantEducationPoco GetSingle(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantEducationPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Applicant_Educations] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Applicant_Educations] SET [Id] = @Id,[Applicant] = @Applicant,
                                           [Major] = @Major,[Certificate_Diploma] = @Certificate_Diploma,[Start_Date] = @Start_Date,
                                           [Completion_Date] = @Completion_Date,[Completion_Percent] = @Completion_Percent
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Major", poco.Major);
                    scommand.Parameters.AddWithValue("@Certificate_Diploma", poco.CertificateDiploma);
                    scommand.Parameters.AddWithValue("@Start_Date", poco.StartDate);
                    scommand.Parameters.AddWithValue("@Completion_Date", poco.CompletionDate);
                    scommand.Parameters.AddWithValue("@Completion_Percent", poco.CompletionPercent);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
