namespace BlackFox.UserTile
{
    using System;
    using System.IO;

    class UserTileBinaryReader
    {
        readonly Stream stream;
        readonly UserTileBinary userTile;

        public UserTileBinaryReader(Stream stream, UserTileBinary userTile)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (userTile == null) throw new ArgumentNullException("userTile");

            this.stream = stream;
            this.userTile = userTile;
        }

        public void Read()
        {

        }
    }
}