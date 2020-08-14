using System.IO;
using NUnit.Framework;

namespace UsingIo
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var text = "ciao";
            using (var ms = new MemoryStream())
            {
                // writing
                // using var sw = new StreamWriter(ms);
                using (var sw = new StreamWriter(ms)) 
                {
                    sw.WriteLine(text);
                    sw.Flush();

                    // first reading from the very beginning
                    ms.Position = 0;
                    using (var sr = new StreamReader(ms))
                    {
                        Assert.True(sr.ReadToEnd().StartsWith(text));
                    }
                    /* DOES NOT WORK BECAUSE THE STREAM IS CLOSED
                    // second reading from the first byte skipped
                    const int shift = 1;
                    ms.Position = shift; // to check if ms is still open
                    using (var sr = new StreamReader(ms))
                    {
                        Assert.True(sr.ReadToEnd().StartsWith(text.Substring(shift)));
                    }
                    */
                }    
            }
        }
    }
}