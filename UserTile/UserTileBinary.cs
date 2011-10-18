/*
 * Copyright (c) 2011, Roncaglia Julien <julien@roncaglia.fr>
 * 
 * This program is open source; you can redistribute it and/or modify it under the terms of the BSD 2-Clause license 
 * as specified in COPYING.txt
 */

namespace BlackFox.UserTile
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    class UserTileBinary
    {
        const int IMAGE_SIZE = 126;
        const int EXPECTED_OFFSET = 0x50;
        byte[] unknownBytes1 = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        byte[] unknownBytes2 = new byte[] { 0x02, 0x00, 0x00, 0x00 };

        public static byte[] Header
        {
            get
            {
                return new byte[]
                {
                    0x01, 0x00, 0x00, 0x00,
                    0x03, 0x00, 0x00, 0x00,
                    0x01, 0x00, 0x00, 0x00
                };
            }
        }

        public string Format { get; set; }

        public string SourcePath { get; set; }

        public byte[] ImageData { get; set; }

        public byte[] UnknownBytes1
        {
            get { return unknownBytes1; }
            set { unknownBytes1 = value; }
        }

        public byte[] UnknownBytes2
        {
            get { return unknownBytes2; }
            set { unknownBytes2 = value; }
        }

        public static UserTileBinary LoadFrom(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException("file");

            using (var stream = file.OpenRead())
            {
                return LoadFrom(stream);
            }
        }

        public static UserTileBinary LoadFrom(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var result = new UserTileBinary();
            var reader = new UserTileBinaryReader(stream, result);
            reader.Read();
            return result;
        }

        public void SaveTo(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var writer = new UserTileBinaryWriter(stream, this);
            writer.Write();
        }

        public void SaveTo(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException("file");

            using (var stream = file.OpenWrite())
            {
                SaveTo(stream);
            }
        }

        /// <summary>
        /// Set the ImageData from an Image.
        /// The tile bitmaps are read in a very secure context, the consequence is that microsoft wrote a specific
        /// bitmap reader accepting only a (very) limited subset of the BMP format and a raw Image can't be used.
        /// </summary>
        public void SetImageData(Image image)
        {
            image = FixImageData(image);

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Bmp);

            ImageData = FixImageBytes(stream.ToArray());
        }

        /// <summary>
        /// Fix the image to force a 80 bytes header size by padding it. 
        /// </summary>
        static byte[] FixImageBytes(byte[] source)
        {
            // I don't know why this limitation exists but the parsing code seem pretty strict about it.

            var bfSize = BitConverter.ToUInt32(source, 2);
            var bfOffBits = BitConverter.ToUInt32(source, 10);

            if (bfOffBits > EXPECTED_OFFSET)
            {
                // With a fixed format it shouldn't happen except if the GDI+ guys screw up completly...
                throw new Exception("The converted bitmap header is too big");
            }

            var result = new MemoryStream(31832);
            var writer = new BinaryWriter(result);

            writer.Write(source, 0, (int)bfOffBits);
            writer.Seek(2, SeekOrigin.Begin);
            writer.Write(bfSize + EXPECTED_OFFSET - bfOffBits);
            writer.Seek(10, SeekOrigin.Begin);
            writer.Write(EXPECTED_OFFSET);
            writer.Seek(EXPECTED_OFFSET, SeekOrigin.Begin);
            writer.Write(source, (int)bfOffBits, (int)(source.Length - bfOffBits));

            writer.Flush();
            return result.ToArray();
        }

        /// <summary>
        /// Fix the image to be a 126x126 16bits bitmap in BI_BITFIELDS format (5bits of red, 6 of green and 5 of blue)
        /// </summary>
        static Bitmap FixImageData(Image image)
        {
            var result = new Bitmap(IMAGE_SIZE, IMAGE_SIZE, PixelFormat.Format16bppRgb565);
            var wh = Math.Min(image.Width, image.Height);

            var left = (int)Math.Floor(image.Width/2.0 - wh/2.0);
            left = Math.Max(left, 0);
            var top = (int)Math.Floor(image.Height/2.0 - wh/2.0);
            top = Math.Max(top, 0);

            var r = new Rectangle(left, top, wh, wh);
            var g = Graphics.FromImage(result);
            g.DrawImage(image, new Rectangle(0, 0, result.Width, result.Height), r, GraphicsUnit.Pixel);

            return result;
        }
    }
}
