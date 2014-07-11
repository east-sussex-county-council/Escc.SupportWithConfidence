using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace Escc.SupportWithConfidence.ETL
{
    [TestFixture]
    public class MyClass
    {
        [Test]
        public void MyMethod()
        {

            string hello = "hello".Select(char.IsLetter).ToString();

            for (int  i = 0; i < hello.Count(); i++)
            {
                var test = i.ToString(CultureInfo.InvariantCulture);
            }

            Assert.That("1", Is.GreaterThan("0"));
            
        }
    }
    

}