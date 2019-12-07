using CareerCloud.DataAccessLayer;
using System;
using System.Collections.Generic;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Linq;

namespace CareerCloud.ADODataAccessLayer
{
    public class ApplicantResumeRepository : IDataRepository<ApplicantResumePoco>
    {
        private readonly string _connectionStr;
        public ApplicantResumeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params ApplicantResumePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (ApplicantResumePoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Applicant_Resumes]
                                           ([Id],[Applicant],[Resume],[Last_Updated])
                                           VALUES
                                           (@Id,@Applicant,@Resume,@Last_Updated)";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Resume", poco.Resume);
                    scommand.Parameters.AddWithValue("@Last_Updated", poco.LastUpdated);
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

        public IList<ApplicantResumePoco> GetAll(params System.Linq.Expressions.Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                scommand.CommandText = @"SELECT [Id],[Applicant],[Resume],[Last_Updated]
                                        FROM [dbo].[Applicant_Resumes]";
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                ApplicantResumePoco[] pocoitems = new ApplicantResumePoco[500];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    ApplicantResumePoco poco = new ApplicantResumePoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Applicant = readpointer.GetGuid(1);
                    poco.Resume = readpointer.GetString(2);
                    poco.LastUpdated = readpointer.IsDBNull(3) ? (DateTime?)null : readpointer.GetDateTime(3);
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantResumePoco> GetList(System.Linq.Expressions.Expression<Func<ApplicantResumePoco, bool>> where, params System.Linq.Expressions.Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantResumePoco GetSingle(System.Linq.Expressions.Expression<Func<ApplicantResumePoco, bool>> where, params System.Linq.Expressions.Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantResumePoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantResumePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Applicant_Resumes] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantResumePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Applicant_Resumes] 
                                           SET [Id] = @Id,[Applicant] = @Applicant,[Resume] = @Resume,[Last_Updated] = @Last_Updated
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Resume", poco.Resume);
                    scommand.Parameters.AddWithValue("@Last_Updated", poco.LastUpdated);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
