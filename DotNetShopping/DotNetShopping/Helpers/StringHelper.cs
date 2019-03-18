using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DotNetShopping.Helpers
{
    public class StringHelper
    {
        public static string ClearFileName(string FileName)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            FileName = r.Replace(FileName, "");
            FileName = FileName.Replace(" ", "_");
            return FileName;
        }

    }
}