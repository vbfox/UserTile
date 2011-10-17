namespace BlackFox.UserTile
{
    using System;
    using System.IO;

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
            userTile.Format = ReadFormat();
            userTile.SourcePath = ReadSourcePath();
        }

        string ReadSourcePath()
        {
            throw new NotImplementedException();
        }

        string ReadFormat()
        {
            throw new NotImplementedException();
        }

        byte[] ReadImageData()
        {
            throw new NotImplementedException();
        }

        void ReadHeader()
        {
            var expectedHeader = UserTileBinary.Header;
            var header = new byte[expectedHeader.Length];

            if (reader.Read(header, 0, header.Length) != header.Length)
            {
                throw new EndOfStreamException("The end of the stream was reached in the header");
            }

            for (int i = 0; i < expectedHeader.Length; i++)
            {
                if (expectedHeader[i] != header[i])
                {
                    throw new IOException("Invalid header binary");
                }
            }
        }
    }
}