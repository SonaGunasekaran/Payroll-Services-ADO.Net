using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeePayroll;
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
        public void TestMethod1()
        {
            int expected = 1;
            int actual = emp.UpdateSalary();
            Assert.AreEqual(actual, expected);
        }
    }
}
