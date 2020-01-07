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
    public class CompanyJobRepository : IDataRepository<CompanyJobPoco>
    {
        private readonly string _connectionStr;
        public CompanyJobRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyJobPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (CompanyJobPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Company_Jobs]
                                           ([Id],[Company],[Profile_Created],[Is_Inactive],[Is_Company_Hidden])
                                           VALUES
                                           (@Id,@Company,@Profile_Created,@Is_Inactive,@Is_Company_Hidden)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Company", poco.Company);
                    scommand.Parameters.AddWithValue("@Profile_Created", poco.ProfileCreated);
                    scommand.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);
                    scommand.Parameters.AddWithValue("@Is_Company_Hidden", poco.IsCompanyHidden);
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

        public IList<CompanyJobPoco> GetAll(params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Company],[Profile_Created],[Is_Inactive],[Is_Company_Hidden],[Time_Stamp]
                                  FROM [dbo].[Company_Jobs]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                CompanyJobPoco[] pocoitems = new CompanyJobPoco[1100];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    CompanyJobPoco poco = new CompanyJobPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Company = readpointer.GetGuid(1);
                    poco.ProfileCreated = readpointer.GetDateTime(2);
                    poco.IsInactive = readpointer.GetBoolean(3);
                    poco.IsCompanyHidden = readpointer.GetBoolean(4);
                    poco.TimeStamp = (byte[])readpointer[5];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyJobPoco> GetList(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobPoco GetSingle(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyJobPoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Company_Jobs] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyJobPoco poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Company_Jobs] SET [Id] = @Id,[Company] = @Company,
                                           [Profile_Created] = @Profile_Created,[Is_Inactive] = @Is_Inactive,
                                           [Is_Company_Hidden] = @Is_Company_Hidden
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Company", poco.Company);
                    scommand.Parameters.AddWithValue("@Profile_Created", poco.ProfileCreated);
                    scommand.Parameters.AddWithValue("@Is_Inactive", poco.IsInactive);
                    scommand.Parameters.AddWithValue("@Is_Company_Hidden", poco.IsCompanyHidden);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
