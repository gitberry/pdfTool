using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfToolz
{
    public static class globularSettings
    {
        public static bool OverwriteSafetyMode { get; set; }
        public static string WorkingPath { get { return Path.GetTempPath(); }  }
        public static string FullTempPathFile(string givenSuffix = ".txt") { return $"{WorkingPath}{Guid.NewGuid()}{givenSuffix}"; }
    }
}
