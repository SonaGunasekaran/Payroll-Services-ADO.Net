using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeePayroll
{
    class EmployeeRepo
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=employee_payroll";
        //creating sql connection 
        SqlConnection sqlConnection = new SqlConnection(connectionString);
    }
}
