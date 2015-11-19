using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class LiquidExpressionVisitorTests
    {
        [Test]
        public void It_Should_Traverse_A_Tree()
        {
            // Arrange
            var ctx = new TemplateContext();
            var liquidString = LiquidString.Create("Test");
            var tree = new TreeNode<IExpressionDescription>(liquidString);

            var visitor = new LiquidExpressionVisitor(ctx);

            // Act
            var result = visitor.Traverse(tree).Result;

            // Assert
            Console.WriteLine(result);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.Value, Is.EqualTo(liquidString));
        }



    }
}
