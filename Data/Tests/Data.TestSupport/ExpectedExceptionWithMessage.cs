using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntLibContrib.Data.TestSupport
{
    public class ExpectedExceptionWithMessageAttribute : ExpectedExceptionBaseAttribute
    {
        public Type ExceptionType { get; set; }

        public string ExpectedMessage { get; set; }


        public ExpectedExceptionWithMessageAttribute(Type exceptionType, string expectedMessage)
        {
            this.ExceptionType = exceptionType;

            this.ExpectedMessage = expectedMessage;
        }

        protected override void Verify(Exception e)
        {
            if (e.GetType() != this.ExceptionType)
            {
                Assert.Fail(String.Format(
                                "ExpectedExceptionWithMessageAttribute failed. Expected exception type: {0}. Actual exception type: {1}. Exception message: {2}",
                                this.ExceptionType.FullName,
                                e.GetType().FullName,
                                e.Message
                                )
                            );
            }

            var actualMessage = e.Message.Trim();

            if (this.ExpectedMessage != null)
            {
                Assert.IsTrue(actualMessage.IndexOf(this.ExpectedMessage, comparisonType: StringComparison.OrdinalIgnoreCase) >= 0);
            }

            //Console.Write("ExpectedExceptionWithMessageAttribute:" + e.Message);
        }
    }
}
