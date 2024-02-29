using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        //I built this before I adopted TDD - all 
        // the code was tested manually through the UI... one of them never did work..  :(
        // I collected all the files for testing and put them in UnitTests project folder

        //todo: build tests for each public function in pdftoool.cs

        // for example:
        // get field list from AARGform.pdf
        // use that field list to create merge input file
        // merge (fill form) that document
        // add SomeImg.png image to AARGform.pdf
        // add QRcode to AARGform
        // insert blank page
        // grab page x from Lengthy document
        // multimerge AARGform a few times with different data inserting a page from Lengthy document
    }
}
