using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationsTests.SeedTests
{
    public class TestCaseOrdering : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            // Order the test cases by the order parameter
            return testCases.OrderBy(x => x.TestMethod.Method.GetCustomAttributes(typeof(TestCaseAttribute)));
        }
    }
}