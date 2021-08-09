using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace EmployeePayroll
{
    public class Transaction
    {
        public static string connectionString = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=Employee_Payroll_Database";
        //creating the object for sql connection class
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        public int AddingRecord(EmployeeData employee)
        {
            EmployeeData data = employee;
            PayRollData payRoll = new PayRollData(employee.BasicPay);
            using (sqlConnection)
            {
                sqlConnection.Open();
                //creating object for transaction class and begin transaction
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    //executing the query
                    string employeInsertion = "insert into Employee(Emp_Name,Company_Id,PhoneNumber,Address,StartDate,Gender) values ('" + employee.EmployeeName + "'," + employee.CompanyId + "," + employee.PhoneNumber + ",'" + employee.Address + "','" + employee.StartDate + "','" + employee.Gender + "')";
                    new SqlCommand(employeInsertion, sqlConnection, transaction).ExecuteNonQuery();
                    string retriveEmpId = "select emp_id from Employee where emp_name='" + employee.EmployeeName + "' and phoneNumber=" + employee.PhoneNumber;
                    int empId = RetriveId(retriveEmpId, transaction);
                    string payRollInsertion = "insert into PayRoll(EmpLoyee_id,BasicPay,Deduction,TaxablePay,IncomeTax,NetPay) values(" + empId + "," + payRoll.BasicPay + "," + payRoll.Deduction + "," + payRoll.TaxablePay + "," + payRoll.IncomeTax + "," + payRoll.NetPay + ")";
                    string employeeDepartmentInsertion = "insert into Employee_Department values (" + empId + "," + employee.DepartmentId + ")";

                    new SqlCommand(payRollInsertion, sqlConnection, transaction).ExecuteNonQuery();
                    new SqlCommand(employeeDepartmentInsertion, sqlConnection, transaction).ExecuteNonQuery();
                    //if all query is successfull commit
                    transaction.Commit();
                    return 1;
                }
                catch (Exception e)
                {
                    //else roll back
                    transaction.Rollback();
                    return 0;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        public int RetriveId(string query, SqlTransaction transaction)
        {
            SqlDataReader reader = new SqlCommand(query, sqlConnection, transaction).ExecuteReader();
            int id = 0;
            if (reader.HasRows)
            {
                id = Convert.ToInt32(reader["emp_id"]);
                reader.Close();
                return id;
            }
            throw new Exception("No data available");
        }
    }
}
