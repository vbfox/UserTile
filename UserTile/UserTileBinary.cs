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

        string format;
        string sourcePath;
        byte[] imageData;

        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        public string SourcePath
        {
            get { return sourcePath; }
            set { sourcePath = value; }
        }

        public byte[] ImageData
        {
            get { return imageData; }
            set { imageData = value; }
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
    }
}