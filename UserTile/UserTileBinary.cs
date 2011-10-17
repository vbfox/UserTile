/*
 * Copyright (c) 2011, Roncaglia Julien <julien@roncaglia.fr>
 * 
 * This program is open source; you can redistribute it and/or modify it under the terms of the BSD 2-Clause license 
 * as specified in COPYING.txt
 */
namespace BlackFox.UserTile
{
    using System;
    using System.IO;

    class UserTileBinary
    {
        public static byte[] Header
        {
            get
            {
                return new byte[]
                {
                    0x01, 0x00, 0x00, 0x00,
                    0x03, 0x00, 0x00, 0x00,
                    0x01, 0x00, 0x00, 0x00,
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

        byte[] unknownBytes1 = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        byte[] unknownBytes2 = new byte[] { 0x02, 0x00, 0x00, 0x00 };
    }
}