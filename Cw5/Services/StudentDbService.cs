﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cw5.Dto;

namespace Cw5.Services
{
    public class StudentDbService : IStudentDbService
    {
        private const string GetStudentsSql = @"SELECT 
	                                        S.FirstName,
	                                        S.LastName,
	                                        S.BirthDate,
	                                        ST.Name AS StudyName,
	                                        E.Semester
                                        FROM 
	                                        [Student] AS S JOIN 
	                                        [Enrollment] AS E ON S.IdEnrollment = E.IdEnrollment JOIN
	                                        [Studies] AS ST ON E.IdStudy = ST.IdStudy
                                        ";

        private const string ExistsQuery = @"SELECT TOP 1 1
                                                FROM 
	                                                [Student] AS ST 
                                                WHERE
	                                                ST.[IndexNumber] = @IndexNumber";


        private const string InsertStudentQuery = @"INSERT INTO [dbo].[Student]
                           ([IndexNumber]
                           ,[FirstName]
                           ,[LastName]
                           ,[BirthDate]
                           ,[IdEnrollment])
                     VALUES
                           (@IndexNumber
                           ,@FirstName
                           ,@LastName
                           ,@BirthDate
                           ,@IdEnrollment)";


        private readonly IConfig config;

        public StudentDbService(IConfig config)
        {
            this.config = config;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            await using var sqlConnection = new SqlConnection(config.ConnectionString);
            await using var command = new SqlCommand(GetStudentsSql,sqlConnection) {CommandType = CommandType.Text};
            await sqlConnection.OpenAsync();

            await using var sqlDataReader = await command.ExecuteReaderAsync();
            var students = new List<Student>();
            while (await sqlDataReader.ReadAsync())
            {
                var student = new Student
                {
                    BirthDate = DateTime.Parse(sqlDataReader[nameof(Student.BirthDate)]?.ToString()),
                    FirstName = sqlDataReader[nameof(Student.FirstName)].ToString(),
                    LastName = sqlDataReader[nameof(Student.LastName)].ToString(),
                    Semester = int.Parse(sqlDataReader[nameof(Student.Semester)].ToString()),
                    StudyName = sqlDataReader[nameof(Student.StudyName)].ToString()
                };
                students.Add(student);
            }

            return students;
        }

        public async Task<bool> Exists(string indexNumber)
        {
            await using var sqlConnection = new SqlConnection(config.ConnectionString);

            await using var command = new SqlCommand(ExistsQuery, sqlConnection) { CommandType = CommandType.Text };
            command.Parameters.AddWithValue("IndexNumber", indexNumber);

            await sqlConnection.OpenAsync();

            await using var sqlDataReader = await command.ExecuteReaderAsync();
            return await sqlDataReader.ReadAsync();
        }

        public async Task Create(EnrollStudent model, SqlTransaction sqlTransaction, int idEnrollment)
        {
            await using var command = new SqlCommand(InsertStudentQuery, sqlTransaction.Connection)
            {
                CommandType = CommandType.Text,
                Transaction = sqlTransaction
            };

            command.Parameters.AddWithValue("@IndexNumber", model.IndexNumber);
            command.Parameters.AddWithValue("@FirstName", model.FirstName);
            command.Parameters.AddWithValue("@LastName", model.LastName);
            command.Parameters.AddWithValue("@BirthDate", model.BirthDate);
            command.Parameters.AddWithValue("@IdEnrollment", idEnrollment);

            await command.ExecuteNonQueryAsync();
        }
    }
}
