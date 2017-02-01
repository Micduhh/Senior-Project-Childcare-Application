using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildcareApplication.DatabaseController
{
    public class AdminDB
    {

        public static void DisplayDirectory()
        {
            WPFMessageBox.Show($"Current Directory: {Directory.GetCurrentDirectory()}");
        }
    }
}
