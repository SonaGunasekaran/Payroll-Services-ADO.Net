using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace EmployeePayroll
{
    public class Transaction
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=employee_payroll";

        SqlConnection sqlConnection = new SqlConnection(connectionString);
        public int AddingRecord(EmployeeData employee)
        {
            PayRollData payRoll = new PayRollData(employee.BasicPay);
            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    string emp = "Insert into Employee(Employee_name,Company_Id,PhoneNumber,Address,City,State,StartDate,Gender) values ('" + employee.EmployeeName + "'," + employee.CompanyId + "," + employee.PhoneNumber + ",'" + employee.Address + "','" + employee.City + "','" + employee.State + "','" + employee.StartDate + "','" + employee.Gender + "')";
                    string payroll = "Insert into PayRoll(Employee_id,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay) values(" + employee.EmployeeId + "," + payRoll.BasicPay + "," + payRoll.Deduction + "," + payRoll.TaxablePay + "," + payRoll.IncomeTax + "," + payRoll.NetPay + ")";
                    string dep = "Insert into Department values (" + employee.EmployeeId + "," + employee.DepartmentId + ")";
                    new SqlCommand(emp, sqlConnection, transaction).ExecuteNonQuery();
                    new SqlCommand(payroll, sqlConnection, transaction).ExecuteNonQuery();
                    new SqlCommand(dep, sqlConnection, transaction).ExecuteNonQuery();
                    transaction.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return 0;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

    }
}
