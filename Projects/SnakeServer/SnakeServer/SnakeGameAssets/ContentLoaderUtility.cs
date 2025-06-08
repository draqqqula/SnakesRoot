using SnakeGameAssets.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameAssets;

public static class ContentLoaderUtility
{
    public static string GetPath(string name)
    {
        string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        return Path.Combine(strWorkPath, "Content", name);
    }
}
