﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cw5.Models;

namespace Cw5.Dal
{
    public interface IStudentDbService
    {
        public Task<IEnumerable<Student>> GetStudents();

        Task<bool> Exists(string indexNumber);
        
        Task Create(EnrollStudent model, SqlTransaction sqlTransaction, int idEnrollment);
    }
}
