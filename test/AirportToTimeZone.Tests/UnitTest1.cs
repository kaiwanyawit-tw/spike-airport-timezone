using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AirportToTimeZone.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var input = new StringReader("NRT");
            System.Console.SetOut(sw);
            var t = Task.Factory.StartNew(() =>
            {
                System.Console.SetIn(input);
                System.Console.SetOut(sw);
                Program.Main(null);
            });

            t.Wait(3000);
       
            Assert.AreEqual("TZDB: 2021a (mapping: $Revision$)\r\nNRT Asia/Tokyo +09\r\n", sb.ToString());
        }
    }
}