namespace BlackFox.UserTile
{
    using System.IO;

    class UserTileBinary
    {
        byte[] header = new byte[]
        {
            0x01, 0x00, 0x00, 0x00,
            0x03, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00,
        };

        string format;
        string sourcePath;
        byte[] imageData;

        void LoadFromFile(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {

            }
        }
    }
}