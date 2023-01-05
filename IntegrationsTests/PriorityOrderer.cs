using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationsTests
{
    public class PriorityOrderer: ITestCaseOrderer
    {
        // Implement the OrderTestCases method of the ITestCaseOrderer interface
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            // Create a sorted dictionary to store the test cases by priority
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

            // Iterate over the test cases
            foreach (TTestCase testCase in testCases)
            {
                // Set the default priority to 0
                int priority = 0;

                // Check if the test method has a TestPriorityAttribute attribute
                foreach (IAttributeInfo attr in testCase.TestMethod.Method.GetCustomAttributes(
                             typeof(TestPriorityAttribute).AssemblyQualifiedName))
                {
                    // If the attribute is present, get the priority value from it
                    priority = attr.GetNamedArgument<int>("Priority");
                }
                    
                // Add the test case to the list for its priority level
                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            // Iterate over the sorted dictionary and yield the test cases in the correct order
            foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
            {
                // Sort the test cases within each priority level alphabetically by method name
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                foreach (TTestCase testCase in list) yield return testCase;
            }
        }

        // Helper function to get the value for a key in a dictionary, or create a new value if the key is not present
        static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            TValue result;

            // Try to get the value for the key
            if (dictionary.TryGetValue(key, out result)) return result;

            // If the key is not present, create a new value
            result = new TValue();
            dictionary[key] = result;

            return result;
        }
    }

}