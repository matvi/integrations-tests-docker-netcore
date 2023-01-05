using System;


namespace IntegrationsTests
{
    // Attribute to specify the priority of a test method
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        // Constructor to set the priority value
        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        // Public property to get the priority value
        public int Priority { get; private set; }
    }
}