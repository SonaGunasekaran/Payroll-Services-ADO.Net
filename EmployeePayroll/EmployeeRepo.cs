using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeePayroll
{
    public class EmployeeRepo
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
                        employee.StartDate = reader.GetDateTime(2).ToString();
                        employee.Address = reader.GetString(5);
                        employee.Department = reader.GetString(6);
                        employee.BasicPay = Convert.ToDouble(reader["BasicPay"]);
                        employee.Deductions = Convert.ToDouble(reader["Deductions"]);
                        employee.TaxablePay = Convert.ToDouble(reader["TaxablePay"]);
                        employee.IncomeTax = Convert.ToDouble(reader["IncomeTax"]);
                        employee.NetPay = Convert.ToDouble(reader["NetPay"]);
                        Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} ", employee.EmployeeId, employee.EmployeeName, employee.Gender, employee.PhoneNumber, employee.StartDate, employee.Address, employee.Department, employee.BasicPay, employee.Deductions, employee.TaxablePay, employee.IncomeTax, employee.NetPay);
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
        public int UpdateSalary()
        {
            EmployeeData employee = new EmployeeData();
            employee.EmployeeName = "Terissa";
            employee.EmployeeId = 5;
            employee.BasicPay = 3000000;
            try
            {
                using (this.sqlConnection)
                {
                    SqlCommand command = new SqlCommand("dbo.UpdateSalaryPayRoll_Salary", this.sqlConnection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    this.sqlConnection.Open();
                    command.Parameters.AddWithValue("@Employee_ID", employee.EmployeeId);
                    command.Parameters.AddWithValue("@Employee_Name", employee.EmployeeName);
                    command.Parameters.AddWithValue("@BasicPay", employee.BasicPay);
                    int result = command.ExecuteNonQuery();
                    if (result != 0)
                    {
                        Console.WriteLine("Salary Updated ");
                    }
                    else
                    {
                        Console.WriteLine("Salary Not Updated");
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
            finally
            {
                this.sqlConnection.Close();

            }

        }
        public EmployeeData RetrieveDataOnDateRange()
        {
            EmployeeData employee = new EmployeeData();
            employee.EmployeeName = "Chandler";
            try
            {
                using (this.sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("dbo.RetriveDataByName", this.sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    this.sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@name", employee.EmployeeName);
                    string query = @"select * from employee_payroll where startDate between('2021-07-23') and getdate()";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employee.EmployeeId = Convert.ToInt32(reader["id"]);
                            employee.EmployeeName = reader["Name"].ToString();
                            employee.Gender = reader["Gender"].ToString();
                            employee.PhoneNumber = Convert.ToDouble(reader["PhoneNumber"]);
                            employee.StartDate = reader.GetDateTime(2).ToString(); 
                            employee.Address = reader.GetString(5);
                            employee.Department = reader.GetString(6);
                            employee.BasicPay = Convert.ToDouble(reader["BasicPay"]);
                            employee.Deductions = Convert.ToDouble(reader["Deductions"]);
                            employee.TaxablePay = Convert.ToDouble(reader["TaxablePay"]);
                            employee.IncomeTax = Convert.ToDouble(reader["IncomeTax"]);
                            employee.NetPay = Convert.ToDouble(reader["NetPay"]);

                            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} ", employee.EmployeeId, employee.EmployeeName, employee.Gender, employee.PhoneNumber, employee.StartDate, employee.Address, employee.Department, employee.BasicPay, employee.Deductions, employee.TaxablePay, employee.IncomeTax, employee.NetPay);
                        }
                        reader.Close();
                        return employee;
                    }
                    else
                    {
                        reader.Close();
                        return employee;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }

            finally
            {
                this.sqlConnection.Close();
            }
        }
        public string AggregateFunctionsGroupByGender()
        {
            string result = null;
            try
            {
                string query = @"select sum(BasicPay) as TotalSalary,min(BasicPay) as MinSalary,max(BasicPay) as MaxSalary, avg(BasicPay) as AvgSalary from payroll_table where Gender ='M' group by Gender";
                SqlCommand sqlCommand = new SqlCommand(query, this.sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("Total Salary {0}", reader[0]);
                        Console.WriteLine("Min Salary {0}", reader[1]);
                        Console.WriteLine("Max Salary {0}", reader[2]);
                        Console.WriteLine("Average Salary {0}", reader[3]);
                        result += reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3];
                    }
                }
                else
                {
                    result = "empty";
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.sqlConnection.Close();
            }
            return result;
        }
        public int InsertNewRecord(EmployeeData employee)
        {
            int count = 0;
            try
            {
                using (sqlConnection)
                {

                    SqlCommand sqlCommand = new SqlCommand("dbo.InsertAddressBookTable1", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                    sqlCommand.Parameters.AddWithValue("@Phonenumber", employee.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Address", employee.Address);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@BasicPay", employee.BasicPay);
                    sqlCommand.Parameters.AddWithValue("@Deduction", employee.Deductions);
                    sqlCommand.Parameters.AddWithValue("@IncomeTax", employee.IncomeTax);
                    sqlCommand.Parameters.AddWithValue("@TaxablePay", employee.TaxablePay);
                   int result = sqlCommand.ExecuteNonQuery();
                    if (result != 0)
                    {
                        count++;
                        Console.WriteLine("Inserted Successfully");
                    }

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
            return count;
         
        }
    }
}
