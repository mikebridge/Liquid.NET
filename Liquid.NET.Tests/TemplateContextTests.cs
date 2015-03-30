using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Tests.Filters;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class TemplateContextTests
    {
        [Test]
        public void It_Should_Reference_A_Defined_Name()
        {
            // Arrange
            const string varname = "hello";
            var templateContext = new TemplateContext();            
            templateContext.Define(varname, new StringValue("HELLO"));

            // Act
            var result = templateContext.Reference(varname);

            // Assert
            Assert.That(result.Value, Is.EqualTo("HELLO"));

        }

        [Test]
        public void It_Should_Construct_A_Filter_Registry()
        {
            // Arrange
            const string varname = "hello";
            var templateContext = new TemplateContext();
            templateContext.WithFilter<FilterFactoryTests.MockStringToStringFilter>("mockfilter");
            
            // Act

            // Assert
            Assert.Fail("Not Implemented Yet");

        }
    }
}
