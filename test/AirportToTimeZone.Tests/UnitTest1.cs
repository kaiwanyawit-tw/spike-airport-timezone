using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AirportToTimeZone.Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var eol = Environment.NewLine;
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

            Assert.AreEqual($"NRT Asia/Tokyo +09{eol}", sb.ToString());
        }
    }
}