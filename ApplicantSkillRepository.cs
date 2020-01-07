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
    public class ApplicantSkillRepository : IDataRepository<ApplicantSkillPoco>
    {
        private readonly string _connectionStr;
        public ApplicantSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connectionStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
        public void Add(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach(ApplicantSkillPoco poco in items)
                {
                    scommand.CommandText = @"INSERT INTO [dbo].[Applicant_Skills]
                                           ([Id],[Applicant],[Skill],[Skill_Level],[Start_Month],[Start_Year],[End_Month],[End_Year])
                                           VALUES
                                           (@Id,@Applicant,@Skill,@Skill_Level,@Start_Month,@Start_Year,@End_Month,@End_Year)";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Skill", poco.Skill);
                    scommand.Parameters.AddWithValue("@Skill_Level", poco.SkillLevel);
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

        public IList<ApplicantSkillPoco> GetAll(params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                scommand.CommandText = @"SELECT [Id],[Applicant],[Skill],[Skill_Level],[Start_Month],[Start_Year],
                                       [End_Month],[End_Year],[Time_Stamp]
                                       FROM [dbo].[Applicant_Skills]";
                connection.Open();
                ApplicantSkillPoco[] pocoitems = new ApplicantSkillPoco[550];
                SqlDataReader readpointer = scommand.ExecuteReader();
                int pocoitemsindex = 0;
                while (readpointer.Read())
                {
                    ApplicantSkillPoco poco = new ApplicantSkillPoco();
                    poco.Id = readpointer.GetGuid(0);
                    poco.Applicant = readpointer.GetGuid(1);
                    poco.Skill = readpointer.GetString(2);
                    poco.SkillLevel = readpointer.GetString(3);
                    poco.StartMonth = readpointer.GetByte(4);
                    poco.StartYear = readpointer.GetInt32(5);
                    poco.EndMonth = readpointer.GetByte(6);
                    poco.EndYear = readpointer.GetInt32(7);
                    poco.TimeStamp = (byte[])readpointer[8];
                    pocoitems[pocoitemsindex] = poco;
                    pocoitemsindex++;
                }
                connection.Close();
                return pocoitems.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantSkillPoco> GetList(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantSkillPoco GetSingle(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantSkillPoco> pocoitems = GetAll().AsQueryable();
            return pocoitems.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"DELETE FROM [dbo].[Applicant_Skills] WHERE [Id]=@Id";
                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    connection.Open();
                    scommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStr))
            {
                SqlCommand scommand = new SqlCommand();
                scommand.Connection = connection;
                foreach (var poco in items)
                {
                    scommand.CommandText = @"UPDATE [dbo].[Applicant_Skills] SET [Id] = @Id,[Applicant] = @Applicant,
                                           [Skill] = @Skill, [Skill_Level] = @Skill_Level,[Start_Month] = @Start_Month,
                                           [Start_Year] = @Start_Year,[End_Month] = @End_Month,[End_Year] = @End_Year
                                           WHERE [Id]=@Id";

                    scommand.Parameters.AddWithValue("@Id", poco.Id);
                    scommand.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    scommand.Parameters.AddWithValue("@Skill", poco.Skill);
                    scommand.Parameters.AddWithValue("@Skill_Level", poco.SkillLevel);
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
