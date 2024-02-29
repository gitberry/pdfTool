using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MultiMergeRecord
{
    public string form;
    public string prefix;
    public string suffix;
    public string pages;
    public string special;
    public string field;
    public string value;
    public string DocumentName { get { return FileName(prefix, form, suffix); } }
    private static string TrimSafe(string givenString)
    {
        if (givenString == null) return "";
        return givenString.Trim();
    }

    public static string FileName(string givenPrefix, string givenForm, string givenSuffix)
    {
        givenPrefix = TrimSafe(givenPrefix);
        givenForm = TrimSafe(givenForm);
        givenSuffix = TrimSafe(givenSuffix);
        if (givenForm == "") throw new Exception("MUST have a form name");
        string d1 = (givenPrefix != "" ? "." : "");
        string d2 = (givenSuffix != "" ? "." : "");
        return string.Format("{0}{1}{2}{3}{4}{5}", givenPrefix, d1, givenForm, d2, givenSuffix, ".pdf");
    }

    //todo - use more standard path handling techniques that I was too lazy to do back then...
    public static string FileNameAndPath(string givenPrefix, string givenForm, string givenSuffix, string givenPath)
    {
        return FileNameAndPath(FileName(givenPrefix, givenForm, givenSuffix), givenPath);
    }

    public static string FileNameAndPath(string givenName, string givenPath)
    {
        while (givenPath.Substring(givenPath.Length - 1) == "\\") { givenPath = givenPath.Substring(0, givenPath.Length - 1); }
        return string.Format("{0}\\{1}", givenPath, givenName);
    }
}
