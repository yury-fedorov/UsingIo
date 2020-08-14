using System.IO;
using System.Xml;
using NUnit.Framework;

namespace UsingIo
{
    public class Tests
    {
        [Test]
        public void TestWithStreamWriter()
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

        [Test]
        public void TestXmlWriter()
        {
            var text = "ciao";
            using (var ms = new MemoryStream())
            {
                // writing
                // using var sw = new StreamWriter(ms);
                using (var sr = new StreamReader(ms))
                {
                    using (var sw = XmlWriter.Create(ms))
                    {
                        sw.WriteElementString(text, "hi!");
                        sw.Flush();

                        // first reading from the very beginning
                        ms.Position = 0;

                        Assert.True(sr.ReadToEnd().IndexOf(text) >= 0);
                    }
                }
            }
        }

        [Test]
        public void TestXmlWriterBroken()
        {
            var text = "ciao";
            using (var ms = new MemoryStream())
            {
                // writing
                using (var sw = XmlWriter.Create(ms))
                {
                    sw.WriteElementString(text, "hi!");
                    sw.Close();
                    // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.Xml/src/System/Xml/Core/XmlWriter.cs
                    // Dispose() calls Close() if the object is not yet closed
                }

                // first reading from the very beginning
                ms.Position = 0;

                using (var sr = new StreamReader(ms))
                {
                    Assert.True(sr.ReadToEnd().IndexOf(text) >= 0);
                }
            }
        }
    }
}
