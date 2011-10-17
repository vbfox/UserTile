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
            var bytes = Encoding.Unicode.GetBytes(userTile.SourcePath + '\0');
            WriteSizePrefixedArray(bytes);
        }
    }
}