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

    class UserTileBinaryWriter
    {
        readonly Stream stream;
        readonly UserTileBinary userTile;
        BinaryWriter writer;

        public UserTileBinaryWriter(Stream stream, UserTileBinary userTile)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (userTile == null) throw new ArgumentNullException("userTile");

            this.stream = stream;
            this.userTile = userTile;
        }

        public void Write()
        {
            writer = new BinaryWriter(stream);

            WriteHeader();
            WriteImageData();
            writer.Write(userTile.UnknownBytes1);

            WriteFormat();
            writer.Write(userTile.UnknownBytes2);
            WriteSourcePath();

            writer.Flush();
            writer = null;
        }

        void WriteHeader()
        {
            writer.Write(UserTileBinary.Header);
        }

        void WriteImageData()
        {
            WriteSizePrefixedArray(userTile.ImageData);
        }

        void WriteSizePrefixedArray(byte[] array)
        {
            if (array == null) throw new ArgumentNullException("array");

            writer.Write(array.Length);
            writer.Write(array);
        }

        void WriteFormat()
        {
            var bytes = Encoding.Unicode.GetBytes(userTile.Format + '\0');
            WriteSizePrefixedArray(bytes);
        }

        void WriteSourcePath()
        {
            var pathToWrite = userTile.SourcePath + '\0'; // Zero-Terminated
            if (pathToWrite.Length%2 != 0)
            {
                // I don't know the deep reason but without this padding byte the file will be invalid
                pathToWrite += '\0';
            }
            var bytes = Encoding.Unicode.GetBytes(pathToWrite);
            WriteSizePrefixedArray(bytes);
        }
    }
}
