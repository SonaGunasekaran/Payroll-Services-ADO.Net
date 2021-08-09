using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;

namespace EmployeePayroll
{
    public class Transaction
    {
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=employee_payroll";
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        private Object myLock = new Object();
        public int AddRecord(EmployeeData employee)
        {
            PayRollData payRoll = new PayRollData(employee.BasicPay);
            using (sqlConnection)
            {
                sqlConnection.Open();

                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    string emp = "Insert into Employee(Employee_Name,Company_Id,PhoneNumber,Address,City,State,StartDate,Gender) values ('" + employee.EmployeeName + "'," + employee.CompanyId + "," + employee.PhoneNumber + ",'" + employee.Address + "','" + employee.City + "','" + employee.State + "','" + employee.StartDate + "','" + employee.Gender + "')";
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
        public int CascadeDelete()
        {
            int count = 0;
            using (sqlConnection)
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = "Delete from Employee where Employee_Id='3'";
                    sqlCommand.ExecuteNonQuery();
                    count++;
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    sqlTransaction.Rollback();
                }
            }
            return count;
        }

        public string AddIsActiveColumn()
        {
            string result = null;
            using (sqlConnection)
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = "Alter table Employee add IsActive int NOT NULL default 1";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    result = "Active Column Added";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    sqlTransaction.Rollback();
                    result = "Column not Updated";
                }
            }
            sqlConnection.Close();
            return result;
        }
        public List<EmployeeData> RetriveDataForAudit(string name)
        {
            using (sqlConnection)
            {
                sqlConnection.Open();
                EmployeeData employee = new EmployeeData();
                List<EmployeeData> employeeList = new List<EmployeeData>();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(name, sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employee.EmployeeId = Convert.ToInt32(reader["Employee_Id"] == DBNull.Value ? default : reader["Employee_Id"]);
                            employee.EmployeeName = reader["Employee_name"] == DBNull.Value ? default : reader["Employee_name"].ToString();
                            employee.Gender = reader["Gender"] == DBNull.Value ? default : reader["Gender"].ToString();
                            employee.StartDate = reader["StartDate"] == DBNull.Value ? default : reader["StartDate"].ToString(); ;
                            employee.PhoneNumber = Convert.ToDouble(reader["PhoneNumber"] == DBNull.Value ? default : reader["PhoneNumber"]);
                            employee.Address = reader["Address"] == DBNull.Value ? default : reader["Address"].ToString();
                            employee.NetPay = Convert.ToDouble(reader["NetPay"] == DBNull.Value ? default : reader["NetPay"]);
                            employee.Department = reader["Department_Name"] == DBNull.Value ? default : reader["Department_Name"].ToString();
                            Thread thread = new Thread(() =>
                            {
                                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} ", employee.EmployeeId, employee.EmployeeName, employee.Gender, employee.StartDate, employee.PhoneNumber, employee.Address, employee.NetPay);
                                Console.WriteLine("Current thread:" + Thread.CurrentThread.Name);

                            }
                            );
                            thread.Start();

                            employeeList.Add(employee);
                        }
                        reader.Close();
                        return employeeList;
                    }
                    else
                    {
                        reader.Close();
                        return employeeList;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        public long InsertWithoutThread()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AddData();
            stopwatch.Stop();
            return (stopwatch.ElapsedMilliseconds);

        }
        public long InsertWithThread()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                lock (myLock)
                {
                    List<EmployeeData> details = RetriveDataForAudit("dbo.RetriveData");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
            }
            return stopwatch.ElapsedMilliseconds;
        }
        void AddData()
        {
            AddRecord(new EmployeeData { EmployeeId = 16, EmployeeName = "Guna", CompanyId = 2, Address = "Zora Palace", City = "Madurai", State = "TamilNadu", StartDate = "2021-07-19", Gender = "M", PhoneNumber = 1234346547, DepartmentId = 3, BasicPay = 65000 });
            AddRecord(new EmployeeData { EmployeeId = 13, EmployeeName = "Adit", CompanyId = 2, Address = "Incara", City = "Cbe", State = "Kerala", StartDate = "2021-07-18", Gender = "M", PhoneNumber = 8877664422, DepartmentId = 2, BasicPay = 6000 });
            AddRecord(new EmployeeData { EmployeeId = 14, EmployeeName = "Klaus", CompanyId = 1, Address = "GagaStreet", City = "Merton", State = "Mystic", StartDate = "2021-07-17", Gender = "M", PhoneNumber = 8877691323, DepartmentId = 3, BasicPay = 60000 });
            AddRecord(new EmployeeData { EmployeeId = 15, EmployeeName = "Helen", CompanyId = 1, Address = "Lartin", City = "Astro", State = "Keldo", StartDate = "2021-07-16", Gender = "F", PhoneNumber = 887709165, DepartmentId = 1, BasicPay = 7000 });
        }

    }
}



