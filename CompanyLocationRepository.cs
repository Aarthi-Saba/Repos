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
    public class CompanyLocationRepository : IDataRepository<CompanyLocationPoco>
    {
        private readonly string _connectionStr;
        public CompanyLocationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params CompanyLocationPoco[] items)
        {

            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand { Connection = connection };
                foreach (CompanyLocationPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Company_Locations] 
                                           ([Id],[Company],[Country_Code],[State_Province_Code],[Street_Address],[City_Town],
                                           [Zip_Postal_Code])
                                           VALUES
                                           (@Id,@Company,@Country_Code,@State_Province_Code,@Street_Address,@City_Town,
                                           @Zip_Postal_Code)";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Company", poco.Company);
                    scommand.Parameters.AddWithValue("@Country_Code", poco.CountryCode);
                    scommand.Parameters.AddWithValue("@State_Province_Code", poco.Province);
                    scommand.Parameters.AddWithValue("@Street_Address", poco.Street);
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

        public IList<CompanyLocationPoco> GetAll(params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT [Id],[Company],[Country_Code],[State_Province_Code],[Street_Address],[City_Town],
                                  [Zip_Postal_Code],[Time_Stamp]
                                  FROM [dbo].[Company_Locations]"
                };
                connection.Open();
                SqlDataReader readpointer = scommand.ExecuteReader();
                CompanyLocationPoco[] pocoitems = new CompanyLocationPoco[400];
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    CompanyLocationPoco poco = new CompanyLocationPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Company = readpointer.GetGuid(1);
                    poco.CountryCode = readpointer.GetString(2);
                    poco.Province = readpointer.IsDBNull(3) ? null: readpointer.GetString(3);
                    poco.Street = readpointer.IsDBNull(4) ? null : readpointer.GetString(4);
                    poco.City = readpointer.IsDBNull(5)? null : readpointer.GetString(5);
                    poco.PostalCode = readpointer.IsDBNull(6) ? null : readpointer.GetString(6);
                    poco.TimeStamp = (byte[])readpointer[7];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<CompanyLocationPoco> GetList(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyLocationPoco GetSingle(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyLocationPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyLocationPoco poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Company_Locations] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand
                {
                    Connection = connection
                };
                foreach (CompanyLocationPoco poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Company_Locations]
                                           SET [Id] = @Id, [Company] = @Company,[Country_Code] = @Country_Code,
                                           [State_Province_Code] = @State_Province_Code,[Street_Address] = @Street_Address,
                                           [City_Town] = @City_Town,[Zip_Postal_Code] = @Zip_Postal_Code
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Company", poco.Company);
                    scommand.Parameters.AddWithValue("@Country_Code", poco.CountryCode);
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
