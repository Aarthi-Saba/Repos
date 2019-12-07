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
    public class SecurityLoginsRoleRepository : IDataRepository<SecurityLoginsRolePoco>
    {
        private readonly string _connectionStr;
        public SecurityLoginsRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params SecurityLoginsRolePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (SecurityLoginsRolePoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Security_Logins_Roles]
                                           ([Id],[Login],[Role])
                                           VALUES
                                           (@Id,@Login,@Role)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Role", poco.Role);
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

        public IList<SecurityLoginsRolePoco> GetAll(params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Login],[Role],[Time_Stamp]
                                  FROM [dbo].[Security_Logins_Roles]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                SecurityLoginsRolePoco[] pocoitems = new SecurityLoginsRolePoco[100];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    SecurityLoginsRolePoco poco = new SecurityLoginsRolePoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Login = readpointer.GetGuid(1);
                    poco.Role = readpointer.GetGuid(2);
                    poco.TimeStamp = (byte[]?)readpointer[3];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<SecurityLoginsRolePoco> GetList(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsRolePoco GetSingle(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsRolePoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsRolePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (SecurityLoginsRolePoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Security_Logins_Roles] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginsRolePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (SecurityLoginsRolePoco poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Security_Logins_Roles]
                                           SET [Id] = @Id,[Login] = @Login,[Role] = @Role
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Role", poco.Role);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
