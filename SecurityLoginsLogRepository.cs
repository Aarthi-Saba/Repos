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
    public class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        private readonly string _connectionStr;
        public SecurityLoginsLogRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (SecurityLoginsLogPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Security_Logins_Log]
                                           ([Id],[Login],[Source_IP],[Logon_Date],[Is_Succesful])
                                           VALUES
                                           (@Id,@Login,@Source_IP,@Logon_Date,@Is_Succesful)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Source_IP", poco.SourceIP);
                    scommand.Parameters.AddWithValue("@Logon_Date", poco.LogonDate);
                    scommand.Parameters.AddWithValue("@Is_Succesful", poco.IsSuccesful);
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

        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Login],[Source_IP],[Logon_Date],[Is_Succesful]
                                  FROM [dbo].[Security_Logins_Log]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                SecurityLoginsLogPoco[] pocoitems = new SecurityLoginsLogPoco[1900];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    SecurityLoginsLogPoco poco = new SecurityLoginsLogPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Login = readpointer.GetGuid(1);
                    poco.SourceIP = readpointer.GetString(2);
                    poco.LogonDate = readpointer.GetDateTime(3);
                    poco.IsSuccesful = readpointer.GetBoolean(4);
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {

            IQueryable<SecurityLoginsLogPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (SecurityLoginsLogPoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Security_Logins_Log] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                           SET [Id] = @Id,[Login] = @Login,[Source_IP] = @Source_IP,[Logon_Date] = @Logon_Date,
                                          [Is_Succesful] = @Is_Succesful
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Source_IP", poco.SourceIP);
                    scommand.Parameters.AddWithValue("@Logon_Date", poco.LogonDate);
                    scommand.Parameters.AddWithValue("@Is_Succesful", poco.IsSuccesful);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
