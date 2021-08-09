using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeePayroll;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PayRollServicesTest
{
    [TestClass]
    public class UnitTest1
    {
        EmployeeRepo emp;
        EmployeeData data;
        [TestInitialize]
        public void SetUp()
        {
            emp = new EmployeeRepo();
            data = new EmployeeData();
        }
        [TestMethod]
        public void TestUpdateSalary()
        {
            int expected = 1;
            int actual = emp.UpdateSalary();
            Assert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void TestRetrieveDataOnDateRange()
        {
            data.EmployeeName = "Chandler";
            var actual = emp.RetrieveDataOnDateRange();
            Assert.AreEqual(actual.EmployeeName, data.EmployeeName);
        }
        [TestMethod]
        public void TestMethodForAggregateFunction_GroupByFemale()
        {
            string expected = "3018000 8000 3000000 1006000";
            string actual = emp.AggregateFunctionsGroupByGender();
            Assert.AreEqual(actual, expected);

        }
        [TestMethod]
        public void TestInsertNewRecord()
        {
           
            int expected = 1;
            data.EmployeeName = "Stefan";
            data.Gender = "Salvatore";
            data.PhoneNumber = 3245678798;
            data.Address = "Garden";
            data.Department = "HR";
            data.BasicPay = 400000;
            data.Deductions = 2000;
            data.TaxablePay = 1000;
            data.IncomeTax = 1000;
            int actual = emp.InsertNewRecord(data);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestInsertIntoTables()
        {
            int expected = 1;
            EmployeeData employee = new EmployeeData { EmployeeId = 7, EmployeeName = "Stefan", CompanyId = 1, DepartmentId = 2, PhoneNumber = 987654321, Address = "Garden", City="Mexico",State="kargo", StartDate = "2021-07-29", Gender = "M", BasicPay = 400000 };
            Transaction transaction = new Transaction();
            int actual = transaction.AddRecord(employee);
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public void TestCascadeDelete()
        {
            int expected = 1;
            int actual = new Transaction().CascadeDelete();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestInsertWithoutThread()
        {
            long actual = new Transaction().InsertWithoutThread();
            Console.WriteLine("" + actual);
        }
        [TestMethod]
        public void TestInsertWithThread()
        {
            long actual = new Transaction().InsertWithThread();
            Console.WriteLine("" + actual);
        }



    }
}
