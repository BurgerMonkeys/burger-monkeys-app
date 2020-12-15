using System;
using System.IO;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Droid.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BurgerMonkeys.Droid.Helper
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}