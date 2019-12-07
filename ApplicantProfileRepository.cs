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
    public class ApplicantProfileRepository : IDataRepository<ApplicantProfilePoco>
    {
        private readonly string _connectionStr;
        public ApplicantProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;

        }
        public void Add(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (ApplicantProfilePoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Applicant_Profiles]
                                           ([Id],[Login],[Current_Salary],[Current_Rate],[Currency],[Country_Code],
                                            [State_Province_Code],[Street_Address],[City_Town],[Zip_Postal_Code])
                                           VALUES
                                           (@Id,@Login,@Current_Salary,@Current_Rate,@Currency,@Country_Code,
                                            @State_Province_Code,@Street_Address,@City_Town,@Zip_Postal_Code)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Current_Salary", poco.CurrentSalary);
                    scommand.Parameters.AddWithValue("@Current_Rate", poco.CurrentRate);
                    scommand.Parameters.AddWithValue("@Currency", poco.Currency);
                    scommand.Parameters.AddWithValue("@Country_Code", poco.Country);
                    scommand.Parameters.AddWithValue("@State_Province_Code", poco.Province);
                    scommand.Parameters.AddWithValue("@Street_Address",poco.Street);
                    scommand.Parameters.AddWithValue("@City_Town", poco.City);
                    scommand.Parameters.AddWithValue("@Zip_Postal_Code", poco.PostalCode);
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

        public IList<ApplicantProfilePoco> GetAll(params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                scommand.CommandText = @"SELECT [Id],[Login],[Current_Salary],[Current_Rate],[Currency],[Country_Code],
                                       [State_Province_Code],[Street_Address],[City_Town],[Zip_Postal_Code],[Time_Stamp]
                                       FROM [dbo].[Applicant_Profiles]";
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                ApplicantProfilePoco[] pocoitems = new ApplicantProfilePoco[500];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    ApplicantProfilePoco poco = new ApplicantProfilePoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Login = readpointer.GetGuid(1);
                    poco.CurrentSalary = readpointer.IsDBNull(2) ? (Decimal?)null : readpointer.GetDecimal(2);
                    poco.CurrentRate = readpointer.IsDBNull(3) ? (Decimal?)null : readpointer.GetDecimal(3);
                    poco.Currency = readpointer.IsDBNull(4) ? null : readpointer.GetString(4);
                    poco.Country = readpointer.IsDBNull(5) ? null : readpointer.GetString(5);
                    poco.Province = readpointer.IsDBNull(6) ? null : readpointer.GetString(6);
                    poco.Street = readpointer.IsDBNull(7) ? null : readpointer.GetString(7);
                    poco.City = readpointer.IsDBNull(8) ? null : readpointer.GetString(8);
                    poco.PostalCode = readpointer.IsDBNull(9) ? null : readpointer.GetString(9);
                    poco.TimeStamp = (byte[])readpointer[10];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantProfilePoco> GetList(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantProfilePoco GetSingle(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantProfilePoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (ApplicantProfilePoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Applicant_Profiles] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantProfilePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (ApplicantProfilePoco poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Applicant_Profiles]
                                           SET [Id] = @Id,[Login] = @Login,[Current_Salary] = @Current_Salary,
                                           [Current_Rate] = @Current_Rate,[Currency] = @Currency,[Country_Code] = @Country_Code,
                                           [State_Province_Code] = @State_Province_Code,[Street_Address] = @Street_Address,
                                           [City_Town] = @City_Town,[Zip_Postal_Code] = @Zip_Postal_Code
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Login", poco.Login);
                    scommand.Parameters.AddWithValue("@Current_Salary", poco.CurrentSalary);
                    scommand.Parameters.AddWithValue("@Current_Rate", poco.CurrentRate);
                    scommand.Parameters.AddWithValue("@Currency", poco.Currency);
                    scommand.Parameters.AddWithValue("@Country_Code", poco.Country);
                    scommand.Parameters.AddWithValue("@State_Province_Code", poco.Province);
                    scommand.Parameters.AddWithValue("@Street_Address", poco.Street);
                    scommand.Parameters.AddWithValue("@City_Town", poco.City);
                    scommand.Parameters.AddWithValue("@Zip_Postal_Code", poco.PostalCode);
                    connection.Open();
                    int rows_updated = scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}

