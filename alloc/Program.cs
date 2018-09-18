using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alloc
{
    class Program
    {
        static int Main(string[] args)
        {
            var totalMemStr = args.Length > 0 ? args[0] : "1.3 GB";
            var blockSizeStr = args.Length > 1 ? args[1] : "64 MB";
            var totalMem = Parse(totalMemStr);
            var blockSize = Parse(blockSizeStr);
            Console.WriteLine($"Allocating {totalMemStr} in blocks of {blockSizeStr}...");
            var blocks = new List<byte[]>();
            long totalAllocated = 0;
            while (totalAllocated < totalMem)
            {
                var thisBlockSize = Math.Min(blockSize, totalMem - totalAllocated);
                try
                {
                    blocks.Add(new byte[thisBlockSize]);
                    totalAllocated += thisBlockSize;
                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine($"FAILED - Could allocate only {totalAllocated} out of {totalMem}. Failed block size: {thisBlockSize}");
                    return 1;
                }
            }

            blocks.Clear();
            Console.WriteLine("SUCCESS");
            return 0;
        }

        private static long Parse(string str)
        {
            const int KB = 1024;
            const int MB = KB * 1024;
            const int GB = MB * 1024;
            try
            {
                var split = str.Split(' ');
                var bytes = decimal.Parse(split[0], null);
                switch (split[1].ToLowerInvariant())
                {
                    case "kb": bytes *= KB; break;
                    case "mb": bytes *= MB; break;
                    case "gb": bytes *= GB; break;
                }
                return (long) bytes;
            }
            catch (Exception e)
            {
                throw new Exception($"Invalid input: {str}", e);
            }
        }
    }
}
