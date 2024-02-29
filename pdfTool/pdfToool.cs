using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfToolz
{
    public class pdfToool
    {
        public void PageExtract(string givenInputPDF, string givenPageNumbers, string givenOutputPDF, Func<string, int> lawg)
        {
            lawg($"Starting Page Extract with {givenInputPDF},{givenPageNumbers},{givenOutputPDF} ");
            if (string.IsNullOrEmpty(givenPageNumbers)) { lawg("At least one page number must be provided."); return; }
            var PageList = givenPageNumbers.Split(',');
            var validCount = 0;
            var pageQueue = new List<int>();
            foreach (string page in PageList)
            {
                int pgnumber = 0;
                int.TryParse(page, out pgnumber);
                if (pgnumber > 0)
                {
                    pageQueue.Add(pgnumber); //Do not check for duplicates - is a feature
                    validCount += 1;
                }
            }
            if (validCount == 0) { lawg("No Valid Pages."); return; } 
            if (!File.Exists(givenInputPDF)) { lawg($"File ({givenInputPDF}) does not exist"); return; }
            if (ExistCantOverwrite(givenOutputPDF,lawg)) { return; }
            
            // extract pages out
            PdfImportedPage tmpPage = null;
            Document outfile = new Document();
            PdfCopy outWriter = new PdfCopy(outfile, new FileStream(givenOutputPDF, FileMode.Create));
            if (outWriter == null) { return; }
            outfile.Open();
            PdfReader reader = new PdfReader(givenInputPDF);
            var BadError = false;
            foreach (int pg in pageQueue)
            {
                try
                {
                    tmpPage = outWriter.GetImportedPage(reader, pg);
                    outWriter.AddPage(tmpPage);
                }
                catch (Exception ex)
                {
                    if (pg > reader.NumberOfPages)
                        lawg($"Page {pg} is greater than number of pages: {reader.NumberOfPages}");
                    else
                        lawg($"Unknown error getting page: {ex.Message}");
                    BadError = true;
                    break;
                }
            }
            PRAcroForm tmpForm = reader.AcroForm;
            if (tmpForm != null) { outWriter.CopyAcroForm(reader); }
            reader.Close();
            outfile.Close();
            if (BadError) { File.Delete(givenOutputPDF); }
            lawg($"Completed Page Extract.");
        }

        public bool PdfFromText(List<pdfMergeField> givenFieldsToMerge, string givenPDFOutputPathAndName, Func<string, int> lawg)
        {
            return PdfFromText(givenFieldsToMerge[0].FieldValue.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList(), givenPDFOutputPathAndName, lawg);
        }

        public bool MakePDFFromText(
            string givenDelimitedLines,
            string givengivenPDFOutputPathAndName,
            Func<string, int> lawg,
            string givenFontOptions = "Courier,8,BLACK"
            )
        {
            List<string> ParagraphString = givenDelimitedLines.Split('^').ToList();
            var details = givenFontOptions.Split(',');
            if (details.Length != 3) { lawg($"details [{givenFontOptions}] must provide font,fontsize,fontcolor"); return false; }
            string gF = fontTry(details[0], "x"); if (gF == "x") { lawg($"[{details[0]} not valid font value"); return false; }
            float gS = floatTry(details[1], -1); if (gS == -1) { lawg($"[{details[1]} not valid font size value"); return false; }
            string gC = colrTry(details[2], "x"); if (gC == "x") { lawg($"[{details[2]} not valid front color value"); return false; }
            var defaultFont = FontFactory.GetFont(gF, gS, BaseColorFromName(gC));
            return PdfFromText(ParagraphString, givengivenPDFOutputPathAndName, defaultFont, lawg);
        }
        public bool PdfFromText(
           List<string> givenParagraphStrings,
           string givengivenPDFOutputPathAndName,
           Func<string, int> lawg,
           string givenFontOptions = "Courier,8,BLACK"
           )
        {
            var details = givenFontOptions.Split(',');
            if (details.Length != 3) { lawg($"details [{givenFontOptions}] must provide font,fontsize,fontcolor"); return false; }
            string gF = fontTry(details[0], "x"); if (gF == "x") { lawg($"[{details[0]} not valid font value"); return false; }
            float gS = floatTry(details[1], -1); if (gS == -1) { lawg($"[{details[1]} not valid font size value"); return false; }
            string gC = colrTry(details[2], "x"); if (gC == "x") { lawg($"[{details[2]} not valid front color value"); return false; }
            var defaultFont = FontFactory.GetFont(gF, gS, BaseColorFromName(gC));
            return PdfFromText(givenParagraphStrings, givengivenPDFOutputPathAndName, defaultFont, lawg);
        }
        public bool PdfFromText(
            List<string> givenParagraphStrings,
            string givenPDFOutputPathAndName,
            iTextSharp.text.Font givenFont,
            Func<string, int> lawg)
        {
            try
            {
                if (ExistCantOverwrite(givenPDFOutputPathAndName, lawg)) { return false; }
                Document pdfDoc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(givenPDFOutputPathAndName, FileMode.Create));
                pdfDoc.Open();
                foreach (string theLine in givenParagraphStrings)
                {
                    pdfDoc.Add(new Paragraph(string.IsNullOrEmpty(theLine) ? " " : theLine, givenFont)); 
                }
                pdfDoc.Close();
                return true;
            }
            catch (Exception ex)
            {
                lawg($"PdfFromText: Error: [{ex.Message}]");
                return false;
            }
        }

        public bool GenerateBlankPDF(string givenOutputFileNamePath, Func<string, int> lawg)
        {
            return PdfFromText(new List<string>() { " " }, givenOutputFileNamePath, lawg);
        }

        public bool InsertBlankPDF(string givenInputPDF, string givenPageToInsertPrior, string givenOutputPDF, Func<string, int> lawg)
        {
            lawg($"Starting InsertBlankPDF with {givenInputPDF},{givenPageToInsertPrior},{givenOutputPDF} ");
            int PageToInsert = -1;
            int.TryParse(givenPageToInsertPrior, out PageToInsert);
            if (PageToInsert == -1 || string.IsNullOrEmpty(givenPageToInsertPrior)) { lawg($"[{givenPageToInsertPrior}] is not a valid page number."); return false; }

            if (!File.Exists(givenInputPDF)) { lawg($"File ({givenInputPDF}) does not exist"); return false; }
            if (ExistCantOverwrite(givenOutputPDF, lawg)) { return false; }

            // --- make blankPDF and put in a temp file
            var tempFilename = $"{globularSettings.WorkingPath}{Guid.NewGuid()}.pdf";
            if (!GenerateBlankPDF(tempFilename, lawg)) { return false; }

            PdfReader reader = new PdfReader(givenInputPDF);
            if (reader.NumberOfPages < PageToInsert) { lawg($"ERROR: Attempting to insert {PageToInsert} within document with  {reader.NumberOfPages} pages."); return false; }

            PdfReader reader2Insert = new PdfReader(tempFilename);

            PdfImportedPage pgToAdd = null;
            Document outFile = new Document();
            PdfCopy outWriter = new PdfCopy(outFile, new FileStream(givenOutputPDF, FileMode.Create));
            if (outWriter == null) { lawg($"Problem creating output [{givenOutputPDF}]"); return false; }
            outFile.Open();
            if (PageToInsert == 0)
            {
                //Insert in front of page doesn't get int the below loop
                pgToAdd = outWriter.GetImportedPage(reader2Insert, 1);
                outWriter.AddPage(pgToAdd);
            }
            for (int pageIndex = 1; pageIndex <= reader.NumberOfPages; pageIndex++)
            {
                pgToAdd = outWriter.GetImportedPage(reader, pageIndex);
                outWriter.AddPage(pgToAdd);
                if (pageIndex == PageToInsert)
                {
                    pgToAdd = outWriter.GetImportedPage(reader2Insert, 1);
                    outWriter.AddPage(pgToAdd);
                }
            }
            PRAcroForm tmpForm = reader.AcroForm;
            if (tmpForm != null) { outWriter.CopyAcroForm(reader); }
            reader.Close();
            reader2Insert.Close();
            outFile.Close();
            lawg($"Finished InsertBlankPDF.");
            File.Delete(tempFilename); // clean up behind ourselves..
            return true;
        }

        public List<string> MultiMergeAndFlatten(
            string givenTemplatePath,
            string givenRawJsonFilePath,
            string givenOutputPath,
            Func<string, int> lawg,
            string givenOutputFile = ""
            )
        {
            if (File.Exists(givenRawJsonFilePath))
            {
                lawg("Starting merge" + DateTime.Now);
                string RawJson = File.ReadAllText(givenRawJsonFilePath);
                List<MultiMergeRecord> JsonObj = JsonConvert.DeserializeObject<List<MultiMergeRecord>>(RawJson);
                lawg("Json successfully deserialized ");
                var MergedFiles = new List<string>();
                try
                {
                    MergedFiles = MultiMerge(JsonObj, givenOutputPath, givenTemplatePath, lawg);  
                    lawg("Finished Complex Merge " + DateTime.Now);
                    lawg($"{MergedFiles.Count} files:");
                    foreach (string x in MergedFiles) { lawg($"{Path.GetFileName(x)},"); }
                    if (givenOutputFile != "" && MergedFiles.Count > 0)
                    {
                        Flatten(MergedFiles, Path.Combine(givenOutputPath, givenOutputFile), lawg);
                    }
                }
                catch (Exception ex)
                {
                    lawg($"MultiMergeAndFlatten error: {ex.Message}.");
                }
            }
            else
            { lawg($"MultiMerge [{givenRawJsonFilePath}] does not exist."); }
            return null;
        }

        public List<string> MultiMerge(
            List<MultiMergeRecord> givenMergeData,
            string givenOutputPath,
            string givenTemplatePath,
            Func<string, int> lawg
            )
        {
            var result = new List<string>();
            //Merge for each document
            foreach (string OutputFilename in givenMergeData.Select(x => x.DocumentName).Distinct())
            {
                lawg($"Merging [{OutputFilename}] starting:");
                // get first record so we can determine our template name 
                var firstRecord = givenMergeData.Find(x => x.DocumentName == OutputFilename); // SHOULD ALWAYS give a value
                var ThisMergeSet = givenMergeData.Where(x => x.DocumentName == OutputFilename).Select(y =>
                                  new pdfMergeField { FieldName = y.field, FieldValue = y.value }).ToList();
                var TemplatePathAndName = MultiMergeRecord.FileNameAndPath(firstRecord.form + ".pdf", givenTemplatePath);
                var OutputPathAndName = MultiMergeRecord.FileNameAndPath(OutputFilename, givenOutputPath);

                if (firstRecord.special == c_PageFromValue)
                { PdfFromText(ThisMergeSet, OutputPathAndName, lawg); lawg("Text Generated..."); }
                else
                { Merge(ThisMergeSet, TemplatePathAndName, OutputPathAndName, lawg); lawg("Merged..."); }

                if (ThisMergeSet.Count > 0 && firstRecord.pages != null)
                {
                    var tmpFile = globularSettings.FullTempPathFile(".pdf");
                    if (File.Exists(tmpFile)) File.Delete(tmpFile);
                    File.Move(OutputPathAndName, tmpFile);
                    ExtractSpecificPages(tmpFile, OutputPathAndName, firstRecord.pages);
                    if (File.Exists(tmpFile)) File.Delete(tmpFile);
                    lawg("adding to parent...");
                }
                result.Add(OutputPathAndName);
                lawg($"Merging [{OutputFilename}] complete.");
            }
            return result;
        }

        public bool MergeFormFromInputTextFile(string givenTemplate, string givenDatafileNamePath, string givenOutputPdf, Func<string, int> lawg)
        {
            string RawData = File.ReadAllText(givenDatafileNamePath);
            var LineData = RawData.Replace("\r","").Split('\n');
            List<pdfMergeField> rezult = new List<pdfMergeField>();
            string part1 = ""; //string part2 = "";
            foreach (string thisLine in LineData)
            {
                if (part1 == "")
                {
                    part1 = thisLine;                    
                }
                else
                {
                    rezult.Add(new pdfMergeField() { FieldName = part1, FieldValue = thisLine }); 
                    part1 = "";
                }                
            }
            return Merge(rezult, givenTemplate, givenOutputPdf, lawg);
        }

        public bool Merge(
            List<pdfMergeField> givenFieldsToMerge,
            string givenPDFInputPathAndName,
            string givenPDFOutputPathAndName,
            Func<string, int> lawg
            )
        {
            var result = false;
            PdfReader pdfReader = new PdfReader(givenPDFInputPathAndName);
            if (!System.IO.File.Exists(givenPDFInputPathAndName))
            { lawg($"Merge Error [{givenPDFInputPathAndName}] does not exist."); return false; }
            else
            {
                if (ExistCantOverwrite(givenPDFOutputPathAndName, lawg)) return false;
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(givenPDFOutputPathAndName, FileMode.CreateNew));

                AcroFields pdfFormFields = pdfStamper.AcroFields;
                foreach (pdfMergeField thisField in givenFieldsToMerge)
                {
                    pdfFormFields.SetField(thisField.FieldName, FixNewLine(thisField.FieldValue));
                    lawg($"Field:[{thisField.FieldName}]=[{thisField.FieldValue}]");
                }
                pdfStamper.FormFlattening = false;
                pdfStamper.Close();
                result = true;
            }
            return result;
        }

        public void Flatten(List<string> givenFileNameAndPaths, string givenOutputFileNameAndPath, Func<string, int> lawg)
        {
            lawg($"Flattening and Joining to: [{givenOutputFileNameAndPath}] ");
            if (ExistCantOverwrite(givenOutputFileNameAndPath, lawg)) { return; }
            FlattenAndCombinePDFs(givenFileNameAndPaths, givenOutputFileNameAndPath, lawg);
            lawg($"Finished Flattening and Join to {givenOutputFileNameAndPath} {DateTime.Now}");
        }

        public void FlattenAndCombinePDFs(List<string> givenPathAndFiles, string givenOutFile, Func<string, int> lawg, bool insertBlankWhenOdd = true)
        {
            var tmpFilename = $"{globularSettings.WorkingPath}{Guid.NewGuid()}.pdf";
            if (ExistCantOverwrite(tmpFilename,lawg)) { return; };
            string BlankPagePathFile = $"{globularSettings.WorkingPath}{Guid.NewGuid()}.pdf";
            if (!GenerateBlankPDF(BlankPagePathFile, lawg)) { return; }

            if (ExistCantOverwrite(givenOutFile, lawg)) { return; }
            PdfImportedPage pgToAdd = null;
            Document outFile = new Document();
            PdfCopy outWriter = new PdfCopy(outFile, new FileStream(givenOutFile, FileMode.Create));
            if (outWriter == null) { lawg($"FlattenAndCombinePDFs error - problem creating output file [{givenOutFile}]"); return; }
            outFile.Open();
            PdfImportedPage BlankPage = outWriter.GetImportedPage(new PdfReader(BlankPagePathFile), 1);
            foreach (string filePathName in givenPathAndFiles)
            {
                if (File.Exists(tmpFilename)) File.Delete(tmpFilename);
                FlattenPdfForm(filePathName, tmpFilename);
                PdfReader reader = new PdfReader(tmpFilename); 
                reader.ConsolidateNamedDestinations();
                for (int pageIndex = 1; pageIndex <= reader.NumberOfPages; pageIndex++)
                {
                    pgToAdd = outWriter.GetImportedPage(reader, pageIndex);
                    outWriter.AddPage(pgToAdd);
                }
                if (insertBlankWhenOdd && reader.NumberOfPages > 0 && reader.NumberOfPages % 2 == 1)
                {
                    outWriter.AddPage(BlankPage);
                }
                PRAcroForm tmpForm = reader.AcroForm;
                if (tmpForm != null) { outWriter.CopyAcroForm(reader); }
                reader.Close();
                lawg($"[{filePathName}] added to [{givenOutFile}]...");
            }
            outFile.Close();
            if (File.Exists(tmpFilename)) File.Delete(tmpFilename);
        }

        // stackoverflow 1942357 
        public static void FlattenPdfForm(string givenPdfPathFile, string givenFLAToutputPathFile) 
        {
            var givenReader = new PdfReader(givenPdfPathFile);
            if (File.Exists(givenFLAToutputPathFile)) File.Delete(givenFLAToutputPathFile);
            var tmpStream = new FileStream(givenFLAToutputPathFile, FileMode.CreateNew);
            var tmpStamper = new PdfStamper(givenReader, tmpStream) { FormFlattening = true };
            tmpStamper.Close();
        }

        public void ExtractSpecificPages(string givenFileToTrim, string givenFileOutput, string givenPages)
        {
            if (string.IsNullOrEmpty(givenPages)) return;
            var PageList = givenPages.Split(',');
            var validCount = 0;
            var pageQueue = new List<int>();
            foreach (string page in PageList)
            {
                int pgnumber = 0;
                int.TryParse(page, out pgnumber);
                if (pgnumber > 0)
                {
                    pageQueue.Add(pgnumber); // don't check for duplicates - is a "feature"
                    validCount += 1;
                }
            }
            if (validCount == 0) return; // must have been garbage - keep the whole pdf
            // extract pages out
            PdfImportedPage tmpPage = null;
            Document outfile = new Document();
            PdfCopy outWriter = new PdfCopy(outfile, new FileStream(givenFileOutput, FileMode.Create));
            if (outWriter == null) { return; }
            outfile.Open();
            PdfReader reader = new PdfReader(givenFileToTrim); // tmpFile);
            foreach (int pg in pageQueue)
            {
                tmpPage = outWriter.GetImportedPage(reader, pg);
                outWriter.AddPage(tmpPage);
            }
            PRAcroForm tmpForm = reader.AcroForm;
            if (tmpForm != null) { outWriter.CopyAcroForm(reader); }
            reader.Close();
            outfile.Close();            
        }

        public bool ExistCantOverwrite(string givenFilename, Func<string, int> lawg)
        {
            if (File.Exists(givenFilename))
            {
                lawg($"File ({givenFilename}) exists");
                if (globularSettings.OverwriteSafetyMode)
                {
                    string RenameToThis = $"{givenFilename}.{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}.BK.pdf"; 
                    System.IO.File.Move(givenFilename, RenameToThis);
                    lawg($" - renamed to ({RenameToThis}");
                    return false;
                }
                else
                {
                    lawg($"File ({givenFilename}) cannot be overwritten.");
                    return true;
                }
            }
            return false;
        }

        public bool InsertImageFile(string givenInputPDF, string givenImageFullPath, string givenXY, string givenOutputPdf, Func<string, int> lawg)
        {
            using (Stream inputImageStream = new FileStream(givenImageFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    lawg($"InsertImageFile Starting [{givenImageFullPath}].");
                    Image image = Image.GetInstance(inputImageStream);
                    lawg($"InsertImageFile converted to image");
                    return InsertImage(givenInputPDF, image, givenXY, givenOutputPdf, lawg);
                }
                catch (Exception ex)
                {
                    lawg($"InsertImageFile Error: [{ex.Message}]");
                    return false;
                }
            }                
        }

        public bool InsertImage(string givenInputPDF, Image givenImage, string givenXY, string givenOutputPdf, Func<string, int> lawg)
        {
            try
            {
                if (ExistCantOverwrite(givenOutputPdf, lawg)) return false;
                using (Stream inputPdfStream = new FileStream(givenInputPDF, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream outputPdfStream = new FileStream(givenOutputPdf, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    lawg($"InsertImage starting..");
                    var xydata = givenXY.Split(',');
                    float gx = -1; if (xydata.Length > 1) { float.TryParse(xydata[0], out gx); if (gx == -1) { lawg($"{xydata[0]} not a valid X value"); return false; } }
                    float gy = -1; if (xydata.Length > 1) { float.TryParse(xydata[1], out gy); if (gy == -1) { lawg($"{xydata[1]} not a valid Y value"); return false; } }
                    float gW = -1; if (xydata.Length > 3) { float.TryParse(xydata[2], out gW); if (gW == -1) { lawg($"{xydata[2]} not a valid WIDTH value"); return false; } }
                    float gH = -1; if (xydata.Length > 3) { float.TryParse(xydata[3], out gH); if (gH == -1) { lawg($"{xydata[3]} not a valid HEIGHT value"); return false; } }
                    var reader = new PdfReader(inputPdfStream);
                    var stamper = new PdfStamper(reader, outputPdfStream);
                    var pdfContentByte = stamper.GetOverContent(1);

                    givenImage.SetAbsolutePosition(gx, gy); 
                    if (gW >= 0 && gH >= 0)
                    {
                        givenImage.ScaleAbsoluteWidth(gW);
                        givenImage.ScaleAbsoluteHeight(gH);
                    }

                    pdfContentByte.AddImage(givenImage);
                    stamper.Close();
                }
                lawg($"InsertImage completed.");
                return true;
            }
            catch (Exception ex)
            {
                lawg($"InsertImage error [{ex.Message}]");
                return false;
            }
        }

        public void PDFFieldExtract(string givenPDFfullpath, Func<string, int> lawg) 
        {
            // later editorial - I had more stuff going on in this originally - would "fill" merge fields and put into another file etc..
            //  I commented that out after debugging was done
            lawg($"PDFFieldExtract starting with [{givenPDFfullpath}]");
            //string pdfExtension = "pdf";
            //string txtExtension = "txt";
            //string DefaultSuffix = ".FIELDS";
            string formatString = 
                    string.Format("[{0}]", ExtractFields.FieldOrder.ToString()) +
                    string.Format("[{0}]", ExtractFields.FieldName.ToString()) +
                    string.Format("[{0}]", ExtractFields.FileName.ToString()) +
                    string.Format("[{0}]", ExtractFields.FieldValue.ToString()) +
                    string.Format("[{0}]", ExtractFields.FieldAppearances.ToString());
            

            string realFormatString = formatString
                .Replace("][", "]\t[")
                .Replace(string.Format("[{0}]", ExtractFields.FieldOrder.ToString()), "{" + string.Format("{0}", (int)ExtractFields.FieldOrder) + "}")
                .Replace(string.Format("[{0}]", ExtractFields.FieldName.ToString()), "{" + String.Format("{0}", (int)ExtractFields.FieldName) + "}")
                .Replace(string.Format("[{0}]", ExtractFields.FileName.ToString()), "{" + String.Format("{0}", (int)ExtractFields.FileName) + "}")
                .Replace(string.Format("[{0}]", ExtractFields.FieldValue.ToString()), "{" + String.Format("{0}", (int)ExtractFields.FieldValue) + "}")
                .Replace(string.Format("[{0}]", ExtractFields.FieldAppearances.ToString()), "{" + String.Format("{0}", (int)ExtractFields.FieldAppearances) + "}");
            //pdfInvigilate mgrI = new pdfInvigilate();
            List<pdfMergeField> FieldList = GetFields(givenPDFfullpath); //mgrI.GetFields(fullPathAndFileName);
            var Keyoutput = new List<string>();
            foreach (pdfMergeField f in FieldList)
            {
                lawg($"{f.FieldName}|{f.FieldValue}");
                // we're going to merge the doc with it's field name in the field so we can visually see where they are
                // BUT - if that is super long (ie MOST corp/gov PDF) then we just enumerate them and then put a key in a text file)
                var OrigValue = f.FieldValue;
                f.FieldValue = "f" + f.FieldOrder.ToString();
                if (f.FieldAppearances.Length > 0)
                {
                    //this is a check box or something - let's set it to 1st or 2nd value depending on if the field order is odd or even
                    var choices = f.FieldAppearances.Split(',').ToList();
                    var OffVal = choices.FindIndex(a => a == "Off");
                    var OnVal = choices.FindIndex(a => a != "Off");
                    if (choices.Count > 1)
                    {
                        if (f.FieldOrder % 2 == 0)
                        {
                            //if odd - we try to find the "off" value and set to that                           
                            if (OffVal >= 0) f.FieldValue = choices[OffVal]; else f.FieldValue = choices[0];
                        }
                        else
                        {
                            // if even we try to find the on value and set to that
                            if (OnVal >= 0) f.FieldValue = choices[OnVal]; else f.FieldValue = choices[1];
                        }
                    }
                    else
                    {                        
                        if (f.FieldOrder % 2 == 0)
                        { f.FieldValue = OrigValue; }
                        else { f.FieldValue = choices[0]; } // the only "choice" available
                    }
                }
                Keyoutput.Add(string.Format(realFormatString, f.FieldOrder, f.FieldName, OrigValue, givenPDFfullpath, f.FieldAppearances)); // params must be in same order as enumeration
            }
            //Artifact from some debugging: System.IO.File.WriteAllText(givenPDFfullpath.ToUpper().Replace("." + pdfExtension.ToUpper(), DefaultSuffix + "." + txtExtension), string.Join(Environment.NewLine, Keyoutput));
            lawg($"PDFFieldExtract finished.");

            //pdfFieldMerge mgrM = new pdfFieldMerge();
            //mgrM.Merge(FieldList, fullPathAndFileName, fullPathAndFileName.ToUpper().Replace("." + pdfExtension.ToUpper(), DefaultSuffix + "." + pdfExtension), true);
        }

        public List<pdfMergeField> GetFields(string givenPdfNameAndPath)
        {
            List<pdfMergeField> result = new List<pdfMergeField>();
            PdfReader pdfReader = new PdfReader(givenPdfNameAndPath);
            string TempFilename = Path.GetTempFileName();
            AcroFields pdfFormFields = pdfReader.AcroFields;
            var fOrder = 0;
            foreach (KeyValuePair<string, AcroFields.Item> kvp in pdfFormFields.Fields)
            {
                fOrder += 1;
                string fieldName = kvp.Key.ToString();
                string fieldValue = pdfFormFields.GetField(kvp.Key.ToString());
                var fldAppearances = pdfFormFields.GetAppearanceStates(fieldName);
                if (fldAppearances == null) fldAppearances = new string[] { "" };
                result.Add(new pdfMergeField { FieldName = fieldName, FieldValue = fieldValue, FieldOrder = fOrder, FieldAppearances = string.Join(",", fldAppearances) });
            }
            pdfReader.Close();
            return result;
        }

        public bool InsertQR(string givenInputPDF, string givenQRcontent, string givenXY, string givenOutputPdf, Func<string, int> lawg)
        {
            try
            {
                lawg($"Insert QR started with [{givenQRcontent}]");
                Image theImage = GenerateQRCode(givenQRcontent, lawg);
                lawg($"QR image generated..");
                return InsertImage(givenInputPDF, theImage, givenXY, givenOutputPdf, lawg);
            }
            catch (Exception ex)
            {
                lawg($"Insert QR error: [{ex.Message}]");
                return false;
            }
        }

        public iTextSharp.text.Image GenerateQRCode(string givenContent, Func<string, int> lawg)
        {
            BarcodeQRCode barcodeQRCode = new BarcodeQRCode(givenContent, 20, 20, null);
            return barcodeQRCode.GetImage();
        }

        public void InsertAngledText(string givenInputPDF, string givenText, string givenDetails, string givenOutputPDF, Func<string, int> lawg)
        {
            // =======================================================
            // modified from here:https://kb.itextpdf.com/it5kb/how-to-rotate-a-paragraph
            // compiles - but doesn't work - never successfully tested
            // =======================================================
            lawg($"Starting InsertAngledText with {givenInputPDF},{givenText},{givenDetails},{givenOutputPDF} ");

            if (!File.Exists(givenInputPDF)) { lawg($"File ({givenInputPDF}) does not exist"); return; }
            if (File.Exists(givenOutputPDF))
            {
                lawg($"File ({givenOutputPDF}) exists");
                if (globularSettings.OverwriteSafetyMode)
                {
                    string RenameToThis = $"{givenOutputPDF}.{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}.BK.pdf"; // Guid.NewGuid()}.pdf";
                    System.IO.File.Move(givenOutputPDF, RenameToThis);
                    lawg($" - renamed to ({RenameToThis}");
                }
                else
                {
                    return;
                }
            }
            var details = givenDetails.Split(',');
            if (details.Length != 9) { lawg($"details [{givenDetails}] must provide page#,x,y,width,height,angle,font,fontsize,fontcolor"); return; }
            float gP = floatTry(details[0], -1); if (gP == -1) { lawg($"[{details[0]} not valid page value"); return; }
            float gX = floatTry(details[1], -1); if (gX == -1) { lawg($"[{details[1]} not valid x value"); return; }
            float gY = floatTry(details[2], -1); if (gY == -1) { lawg($"[{details[2]} not valid y value"); return; }
            float gW = floatTry(details[3], -1); if (gW == -1) { lawg($"[{details[3]} not valid width value"); return; }
            float gH = floatTry(details[4], -1); if (gH == -1) { lawg($"[{details[4]} not valid height value"); return; }
            float gA = floatTry(details[5], -1); if (gA == -1) { lawg($"[{details[5]} not valid angle value"); return; }
            string gF = fontTry(details[6], "x"); if (gF == "x") { lawg($"[{details[6]} not valid font value"); return; }
            float gS = floatTry(details[7], -1); if (gS == -1) { lawg($"[{details[7]} not valid font size value"); return; }
            string gC = colrTry(details[8], "x"); if (gC == "x") { lawg($"[{details[8]} not valid front color value"); return; }

            var defaultFont = FontFactory.GetFont(gF, gS, BaseColorFromName(gC));

            //proceed with creating the new file - and do our text insert when we get to the page involved...
            PdfReader reader = new PdfReader(givenInputPDF);
            if (reader.NumberOfPages < gP) { lawg($"Attempting to insert {gP} within document with  {reader.NumberOfPages} pages."); return; }


            PdfImportedPage pgToAdd = null;
            Document outFile = new Document();
            PdfCopy outWriter = new PdfCopy(outFile, new FileStream(givenOutputPDF, FileMode.Create));
            if (outWriter == null) { lawg($"Problem creating output [{givenOutputPDF}]"); return; }
            outFile.Open();
            for (int pageIndex = 1; pageIndex <= reader.NumberOfPages; pageIndex++)
            {
                pgToAdd = outWriter.GetImportedPage(reader, pageIndex);
                if (pageIndex == gP)
                {
                    // inspired from here:https://kb.itextpdf.com/it5kb/how-to-rotate-a-paragraph
                    var thisP = new Paragraph(givenText, defaultFont);
                    //Create the template that will contain the text
                    PdfContentByte canvas = outWriter.DirectContentUnder; 
                    PdfTemplate textTemplate = canvas.CreateTemplate(gW, gH);
                    ColumnText columnText = new ColumnText(textTemplate);
                    columnText.SetSimpleColumn(0, 0, gW, gH);
                    columnText.AddElement(thisP);
                    columnText.Go();
                    //Create the image wrapper for the template
                    iTextSharp.text.Image textImg = iTextSharp.text.Image.GetInstance(textTemplate);
                    //Assign the dimentions of the image, in this case, the text
                    textImg.Interpolation = true; 
                    textImg.ScaleAbsolute(gW, gH); 
                    textImg.Rotation = gA; 
                    textImg.SetAbsolutePosition(gX, gY); 
                    outWriter.Add(textImg);
                }
                outWriter.AddPage(pgToAdd);
            }
            PRAcroForm tmpForm = reader.AcroForm;
            if (tmpForm != null) { outWriter.CopyAcroForm(reader); }
            reader.Close();
            outFile.Close();
            lawg($"Finished InsertAngledText with {givenInputPDF},{givenText},{givenDetails},{givenOutputPDF} ");
        }

        #region "Tooling"

        public enum ExtractFields
        {
            FieldOrder,
            FieldName,
            FieldValue,
            FileName,
            FieldAppearances
        }

        class ColorNameHack
        {
            public string HumanColorName { get; set; }
            public BaseColor baseColor { get; set; }
        }

        private static List<ColorNameHack> BaseColors()
        {
            return new List<ColorNameHack>() {
                new ColorNameHack() {HumanColorName="WHITE"      , baseColor = BaseColor.WHITE      },
                new ColorNameHack() {HumanColorName="LIGHT_GRAY" , baseColor = BaseColor.LIGHT_GRAY },
                new ColorNameHack() {HumanColorName="GRAY"       , baseColor = BaseColor.GRAY       },
                new ColorNameHack() {HumanColorName="DARK_GRAY"  , baseColor = BaseColor.DARK_GRAY  },
                new ColorNameHack() {HumanColorName="BLACK"      , baseColor = BaseColor.BLACK      },
                new ColorNameHack() {HumanColorName="RED"        , baseColor = BaseColor.RED        },
                new ColorNameHack() {HumanColorName="PINK"       , baseColor = BaseColor.PINK       },
                new ColorNameHack() {HumanColorName="ORANGE"     , baseColor = BaseColor.ORANGE     },
                new ColorNameHack() {HumanColorName="YELLOW"     , baseColor = BaseColor.YELLOW     },
                new ColorNameHack() {HumanColorName="GREEN"      , baseColor = BaseColor.GREEN      },
                new ColorNameHack() {HumanColorName="MAGENTA"    , baseColor = BaseColor.MAGENTA    },
                new ColorNameHack() {HumanColorName="CYAN"       , baseColor = BaseColor.CYAN       },
                new ColorNameHack() {HumanColorName="BLUE"       , baseColor = BaseColor.BLUE       },
                }.ToList();
        }

        private BaseColor BaseColorFromName(string givenColorText)
        {
            foreach (ColorNameHack thisColor in BaseColors())
            {
                if (givenColorText.ToLower() == thisColor.HumanColorName.ToLower()) { return thisColor.baseColor; }
            }
            return null;
        }

        private string colrTry(string givenString, string failValue)
        {
            string retval = failValue;
            var tryval = BaseColorFromName(givenString);
            if (!(tryval is null)) { retval = givenString; }
            return retval;
        }

        private string fontTry(string givenString, string givenFail)
        {
            string retval = givenFail;
            List<string> validFonts = "Courier-Bold|Courier-BoldOblique|Courier-Oblique|Courier|Helvetica-Bold|Helvetica-BoldOblique|Helvetica-Oblique|Helvetica|Symbol|Times-Bold|Times-BoldItalic|Times-Italic|Times-Roman|ZapfDingbats".Split('|').ToList();
            if (validFonts.FindIndex(zz => zz.ToLower() == givenString.ToLower()) >= 0) { retval = givenString; }
            return retval;
        }

        private float floatTry(string givenString, int givenDefault)
        {
            float retval = givenDefault;
            if (!string.IsNullOrEmpty(givenString)) { float.TryParse(givenString, out retval); }
            return retval;
        }

        private string FixNewLine(string value)
        {
            return value.Replace("[newline]", "\r\n").Replace("^","\r\n");
        }

        public const string c_PageFromValue = "PageFromValue";

        #endregion
    }
}
