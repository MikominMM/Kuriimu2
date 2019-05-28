﻿using System;
using System.IO;
using System.Linq;
using Kompression.LempelZiv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KompressionUnitTests
{
    [TestClass]
    public class LempelZivTests
    {
        [TestMethod]
        public void FindOccurences_IsCorrect()
        {
            var input = new byte[]
                {0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00};

            var results = Kompression.LempelZiv.Common.FindOccurrences(input, 8, 4, 16);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(7, results[0].Position);
            Assert.AreEqual(8, results[0].Length);
            Assert.AreEqual(7, results[0].Displacement);
        }

        [TestMethod]
        public void LZ10_CompressDecompress()
        {
            var input = new byte[]
            {
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
            };

            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            LZ10.Compress(new MemoryStream(input), compStream);
            compStream.Position = 0;
            LZ10.Decompress(compStream, decompStream);
            compStream.Position = decompStream.Position = 0;

            Assert.AreEqual(0x10, compStream.ToArray()[0]);
            Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        }

        [TestMethod]
        public void LZ11_CompressDecompress()
        {
            var input = new byte[]
            {
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
            };

            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            LZ11.Compress(new MemoryStream(input), compStream);
            compStream.Position = 0;
            LZ11.Decompress(compStream, decompStream);
            compStream.Position = decompStream.Position = 0;

            Assert.AreEqual(0x11, compStream.ToArray()[0]);
            Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        }

        [TestMethod]
        public void LZ40_CompressDecompress()
        {
            var input = new byte[]
            {
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
            };

            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            LZ40.Compress(new MemoryStream(input), compStream);
            compStream.Position = 0;
            LZ40.Decompress(compStream, decompStream);
            compStream.Position = decompStream.Position = 0;

            Assert.AreEqual(0x40, compStream.ToArray()[0]);
            Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        }

        [TestMethod]
        public void LZ60_CompressDecompress()
        {
            var input = new byte[]
            {
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
            };

            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            LZ60.Compress(new MemoryStream(input), compStream);
            compStream.Position = 0;
            LZ60.Decompress(compStream, decompStream);
            compStream.Position = decompStream.Position = 0;

            Assert.AreEqual(0x60, compStream.ToArray()[0]);
            Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        }

        [TestMethod]
        public void LZ77_CompressDecompress()
        {
            var input = new byte[]
            {
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
            };

            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            LZ77.Compress(new MemoryStream(input), compStream);
            compStream.Position = 0;
            LZ77.Decompress(compStream, decompStream);
            compStream.Position = decompStream.Position = 0;

            Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        }
    }
}
