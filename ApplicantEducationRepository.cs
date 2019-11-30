using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace CareerCloud.ADODataAccessLayer
{
    public class ApplicantEducationRepository : IDataRepository<ApplicantEducationPoco>
    {
        private string ConnectionStr;

        //Data Source=DESKTOP-LC31E5U\HUMBERBRIDGING;Initial Catalog=JobPortal_Local;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        //public ApplicantEducationRepository()
        //{
        //    var config = new ConfigurationBuilder();
        //    var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        //    config.AddJsonFile(path, false);
        //    var root = config.Build();
        //    _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        //}

        ApplicantEducationRepository appedurepo = new ApplicantEducationRepository();
        IDataRepository<ApplicantEducationPoco> idatarepo = IDataRepository < ApplicantEducationPoco > appedurepo;
        
        //ConnectionStr = ((IDataRepository<ApplicantEducationPoco>) appedurepo).ConnectionString();

        public void Add(params ApplicantEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                foreach (ApplicantEducationPoco poco in items)
                {
                    SqlCommand scommand = new SqlCommand();
                    scommand.Connection = connection;
                    scommand.CommandText = @"INSERT INTO[dbo].[Applicant_Educations]
                                          ([Id],[Applicant,[Major],[Certificate_Diploma],[Start_Date],[Completion_Date],[Completion_Percent])
                                          VALUES
                                          (@Id,@Applicant,@Major,@Certificate_Diploma,@Start_Date,@Completion_Date,@Completion_Percent)";

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
            throw new NotImplementedException();
        }

        public IList<ApplicantEducationPoco> GetList(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantEducationPoco GetSingle(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Remove(params ApplicantEducationPoco[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params ApplicantEducationPoco[] items)
        {
            throw new NotImplementedException();
        }
    }
}
