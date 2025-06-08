
using System.Drawing;
using var fs = new FileStream("patterns", FileMode.Create);

foreach (var file in Directory.EnumerateFiles("Input"))
{
    var map = (Bitmap)Bitmap.FromFile(file);
    var gu = GraphicsUnit.Pixel;
    var bounds = map.GetBounds(ref gu);
    var width = Convert.ToInt32(bounds.Width);
    var height = Convert.ToInt32(bounds.Height);
    if (width != 8 || height != 8)
    {
        Console.WriteLine($"file {file} does not match required 8x8 format");
        continue;
    }
    for (int y = 0; y < 8; y++)
    {
        byte b = 0;
        for (int x = 0; x < 8; x++)
        {
            var color = map.GetPixel(x, y).ToArgb();
            if (color == -16777216)
            {
                b += (byte)Math.Pow(2, x);
            }
        }
        fs.WriteByte(b);
    }
}
fs.Close();