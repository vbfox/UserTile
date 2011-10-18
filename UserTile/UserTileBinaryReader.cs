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
    using System.Text;

    class UserTileBinaryReader
    {
        readonly Stream stream;
        readonly UserTileBinary userTile;
        BinaryReader reader;

        public UserTileBinaryReader(Stream stream, UserTileBinary userTile)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (userTile == null) throw new ArgumentNullException("userTile");

            this.stream = stream;
            this.userTile = userTile;
        }

        public void Read()
        {
            reader = new BinaryReader(stream);

            ReadHeader();
            userTile.ImageData = ReadImageData();
            userTile.UnknownBytes1 = reader.ReadBytes(4);
            userTile.Format = ReadFormat();
            userTile.UnknownBytes2 = reader.ReadBytes(4);
            userTile.SourcePath = ReadSourcePath();

            reader = null;
        }

        string ReadSourcePath()
        {
            var size = reader.ReadUInt32();
            if (size > int.MaxValue) throw new IOException("Not supported source path string size");

            var bytes = reader.ReadBytes((int)size);
            return Encoding.Unicode.GetString(bytes).TrimEnd('\0');
        }

        string ReadFormat()
        {
            var size = reader.ReadUInt32();
            if (size > int.MaxValue) throw new IOException("Not supported format string size");

            var bytes = reader.ReadBytes((int)size);
            return Encoding.Unicode.GetString(bytes).TrimEnd('\0');
        }

        byte[] ReadImageData()
        {
            var size = reader.ReadUInt32();
            if (size > int.MaxValue) throw new IOException("Not supported image data size");

            return reader.ReadBytes((int)size);
        }

        void ReadHeader()
        {
            var expectedHeader = UserTileBinary.Header;
            var header = reader.ReadBytes(expectedHeader.Length);

            for (var i = 0; i < expectedHeader.Length; i++)
            {
                if (expectedHeader[i] != header[i]) throw new IOException("Invalid header binary");
            }
        }
    }
}
