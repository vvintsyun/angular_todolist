using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Todolist.Dtos
{
    public class ZipItem
    {
        public string Name { get; set; }
        public Stream Content { get; set; }
        public ZipItem(string name, Stream content)
        {
            Name = name;
            Content = content;
        }
        public ZipItem(string name, string contentStr, Encoding encoding)
        {
            // convert string to stream
            var byteArray = encoding.GetBytes(contentStr);
            var memoryStream = new MemoryStream(byteArray);
            Name = name;
            Content = memoryStream;
        }
    }

    public static class Zipper
    {
        public static Stream Zip(List<ZipItem> zipItems)
        {
            var zipStream = new MemoryStream();

            using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var zipItem in zipItems)
                {
                    var entry = zip.CreateEntry(zipItem.Name);
                    using (var entryStream = entry.Open())
                    {
                        zipItem.Content.CopyTo(entryStream);
                    }
                }
            }
            zipStream.Position = 0;
            return zipStream;
        }
    }
}