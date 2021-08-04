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
        public void GetAllData()
        {
            //open connection
            this.sqlConnection.Open();
            //retrieve the query
            string query = @"select * from dbo.payroll_table";
            EmployeeData employee = new EmployeeData();
            try
            {
                SqlCommand command = new SqlCommand(query, sqlConnection);
                //returns data as rows
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.EmployeeId = Convert.ToInt32(reader["id"]);
                        employee.EmployeeName = reader["Name"].ToString();
                        employee.Gender = reader["Gender"].ToString();
                        employee.PhoneNumber = Convert.ToDouble(reader["PhoneNumber"]);
                        employee.StartDate = reader.GetDateTime(2);
                        employee.Address = reader.GetString(6);
                        employee.Department = reader.GetString(6);
                        employee.BasicPay = Convert.ToDouble(reader["BasicPay"]);
                        employee.Deductions = Convert.ToDouble(reader["Deductions"]);
                        employee.TaxablePay = Convert.ToDouble(reader["TaxablePay"]);
                        employee.IncomeTax = Convert.ToDouble(reader["IncomeTax"]);
                        employee.NetPay = Convert.ToDouble(reader["NetPay"]);
                        Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} ", employee.EmployeeId, employee.EmployeeName, employee.Gender, employee.PhoneNumber, employee.StartDate, employee.Address, employee.Department,employee.BasicPay, employee.Deductions, employee.TaxablePay, employee.IncomeTax, employee.NetPay);
                    }
                }
                else
                {
                    Console.WriteLine("No data vailable");
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //connection close
                this.sqlConnection.Close();
            }
        }
        public void UpdateSalary(EmployeeData data)
        {
            try
            {
                sqlConnection.Open();
                //query to update salary
                string query = @"update payroll_table set BasicPay=3000000 where name='Chandler'";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                int result = command.ExecuteNonQuery();
                if (result != 0)
                {
                    Console.WriteLine("Salary Updated ");
                }
                else
                {
                    Console.WriteLine("Salary Not Updated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();

            }

        }
    }
}
