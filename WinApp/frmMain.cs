using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdfTool
{
    public partial class frmMain : Form
    {
        public string[] appArgs = null;
        public bool ConsoleMode = false;

        //todo - finish making this command line capable - all the code is in a class library now so  should be easier...
        public const string ArgPrefix_ConsoleMode = "/C"; //=";
        public const string ArgPrefix_Help = "/H";
        public const string ArgPrefix_OverWriteSafetyMode = "/O";

        public string ArgVal_ConsoleMode1 = ArgPrefix_ConsoleMode + "true";
        public string ArgVal_ConsoleMode2 = ArgPrefix_ConsoleMode + "1";

        public string HelpString = "Command Line parameters:\r\n/H --- help\r\n/C --- Console mode.";

        public const string act_Extract    = "Extract Page       " ; public const string param_Extract     = "ex";
        public const string act_AddBlank   = "Add Blank Page     " ; public const string param_AddBlank    = "ab";
        public const string act_Merge2     = "Merge Two PDFs     " ; public const string param_Merge2      = "mg";
        public const string act_AddText    = "Add Text           " ; public const string param_AddText     = "at";
        public const string act_FillForm   = "Fill Form          " ; public const string param_FillForm    = "ff";
        public const string act_GetFields  = "Extract Form Fields" ; public const string param_GetFields   = "gf";
        public const string act_PDFFromTXT = "PDF From Text"       ; public const string param_PFDFromText = "pt";
        public const string act_MultiMerge = "MultiMerge from JSON"; public const string param_MultiMerge  = "mj";
        public const string act_InsertImg  = "Insert Image"        ; public const string param_InsertImg   = "ii";
        public const string act_InsertQR   = "Insert QR code"      ; public const string param_InsertQR    = "iq";
        public string[] acts = { 
            act_MultiMerge,
            act_Extract,
            act_AddBlank,
            act_Merge2,
            act_AddText,
            act_FillForm,
            act_GetFields,
            act_PDFFromTXT,
            act_InsertImg,
            act_InsertQR
        };
        public List<ActPrep> actPrep = new List<ActPrep>();
        public List<ActSet> actSet = new List<ActSet>();

        public frmMain(string[] args)
        {
            InitializeComponent();
            pdfToolz.globularSettings.OverwriteSafetyMode = false; // this is a truly incredible hack... changing static values man!!
            appArgs = args;
            actPrep.Add(new ActPrep() { ActString = act_GetFields  , ParamString = param_GetFields , Labels = "PDF file:" });
            actPrep.Add(new ActPrep() { ActString = act_FillForm   , ParamString = param_FillForm  , Labels = "PDF in:|Form Data (field<LF>value<LF> pairs) file:|Output PDF:" });
            actPrep.Add(new ActPrep() { ActString = act_InsertImg  , ParamString = param_GetFields , Labels = "PDF in:|Image file:|8.5\"=605 lower left= 0,0 right,up(,width,height):|PDF Out:" });
            actPrep.Add(new ActPrep() { ActString = act_InsertQR   , ParamString = param_GetFields , Labels = "PDF in:|QR value:|x,y(,width,height):|PDF Out:" });
            actPrep.Add(new ActPrep() { ActString = act_AddBlank   , ParamString = param_AddBlank  , Labels = "PDF in:|Insert Blank after Page #:|PDF out:" });
            actPrep.Add(new ActPrep() { ActString = act_Extract    , ParamString = param_Extract   , Labels = "PDF in:|Extract Page #'s:|PDF out:"});
            actPrep.Add(new ActPrep() { ActString = act_AddText    , ParamString = param_AddText   , Labels = "PDF in:|Text:|page#,x,y,width,height,angle,font,fontsize,fontcolor|PDF out:" });
            actPrep.Add(new ActPrep() { ActString = act_PDFFromTXT , ParamString = param_GetFields , Labels = "Lines Separated by ^:|PDF Out:|(optional) font,fontsize,fontcolor:" });
            actPrep.Add(new ActPrep() { ActString = act_MultiMerge , ParamString = param_MultiMerge, Labels = "Source PDF Forms Folder:|Merge Data (JSON) file:|Output Folder:|(optional) Single flattened Output PDF:"});

            actSet.Add(new ActSet() {ActLabel = label1, ActTextBox = textBox1});
            actSet.Add(new ActSet() {ActLabel = label2, ActTextBox = textBox2});
            actSet.Add(new ActSet() {ActLabel = label3, ActTextBox = textBox3});
            actSet.Add(new ActSet() {ActLabel = label4, ActTextBox = textBox4});
            actSet.Add(new ActSet() {ActLabel = label5, ActTextBox = textBox5});
            ActionPrep(""); 
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ProcessArgs();
            foreach (ActPrep thisAct in actPrep) { cboAction.Items.Add(thisAct.ActString ); }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            myLogger("---------------------------- New Action ------------------------------------");
            var pdfTule = new pdfToolz.pdfToool(); 
            var SelectedAction = cboAction.SelectedItem.ToString();
            if ( SelectedAction == act_Extract) 
                pdfTule.PageExtract(textBox1.Text, textBox2.Text, textBox3.Text, myLogger); 
            if (SelectedAction == act_AddBlank)            
                pdfTule.InsertBlankPDF(textBox1.Text, textBox2.Text, textBox3.Text, myLogger); 
            if (SelectedAction == act_AddText)            
                pdfTule.InsertAngledText(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, myLogger); 
            if (SelectedAction == act_PDFFromTXT) if (string.IsNullOrEmpty(textBox3.Text))  
                     pdfTule.MakePDFFromText(textBox1.Text, textBox2.Text, myLogger); 
                else pdfTule.MakePDFFromText(textBox1.Text, textBox2.Text, myLogger, textBox3.Text); 
            if (SelectedAction == act_MultiMerge) if (string.IsNullOrEmpty(textBox4.Text)) 
                     pdfTule.MultiMergeAndFlatten(textBox1.Text, textBox2.Text, textBox3.Text, myLogger);
                else pdfTule.MultiMergeAndFlatten(textBox1.Text, textBox2.Text, textBox3.Text, myLogger, textBox4.Text);
            if (SelectedAction == act_FillForm)
                pdfTule.MergeFormFromInputTextFile(textBox1.Text, textBox2.Text, textBox3.Text, myLogger);
            if (SelectedAction == act_InsertImg )
                pdfTule.InsertImageFile(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, myLogger);
            if (SelectedAction == act_GetFields)
                pdfTule.PDFFieldExtract(textBox1.Text, myLogger);
            if (SelectedAction == act_InsertQR)
                pdfTule.InsertQR(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, myLogger);
        }

        private void cboAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActionPrep(cboAction.SelectedItem.ToString());
// these help for testing
#if DEBUG
            var SelectedAction = cboAction.SelectedItem.ToString();
            if (SelectedAction == act_GetFields)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\AARGform.pdf"; // "D:\\delsoon\\ToxicTest\\forms\\exampleForm.pdf";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                chkSafetyOverwrite.Checked = true;
            }
            if (SelectedAction == act_FillForm)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\AARGform.pdf";
                textBox2.Text = "..\\..\\..\\UnitTests\\aaargFieldsGrump1.txt";
                textBox3.Text = "..\\..\\..\\UnitTests\\aaargFieldsGrump1.pdf";
                textBox4.Text = "";
                chkSafetyOverwrite.Checked = true;
            }
            if (SelectedAction == act_InsertImg)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\AARGform.pdf";
                textBox2.Text = "..\\..\\..\\UnitTests\\SomeImg.png"; //                textBox2.Text = "D:\\delsoon\\ToxicTest\\Forms\\SomeImg.png";
                textBox3.Text = "420,593,93,71";
                textBox4.Text = "..\\..\\..\\UnitTests\\ExampleWithImg.pdf";
                chkSafetyOverwrite.Checked = true;
            }
            if (SelectedAction == act_InsertQR)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\AARGform.pdf";
                textBox2.Text = $"Grumplegump,{Guid.NewGuid()}";
                textBox3.Text = "72,72,72,72";
                textBox4.Text = "..\\..\\..\\UnitTests\\ExampleWithQR.pdf";
                chkSafetyOverwrite.Checked = true;
            }
            if (SelectedAction == act_AddBlank)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\AARGform.pdf";
                textBox2.Text = $"1";
                textBox3.Text = "..\\..\\..\\UnitTests\\ExampleWithBlank2.pdf";
                textBox4.Text = "";
                chkSafetyOverwrite.Checked = true;                
            }
            if (SelectedAction == act_Extract)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\LengthyLatinDocument.pdf";
                textBox2.Text = $"6";
                textBox3.Text = "..\\..\\..\\UnitTests\\ExamplExtractFromLength.pdf";
                textBox4.Text = "";
                chkSafetyOverwrite.Checked = true;
                //pdfToool.PageExtract(textBox1.Text, textBox2.Text, textBox3.Text, myLogger);
            }
            if (SelectedAction == act_MultiMerge)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\";
                textBox2.Text = "..\\..\\..\\UnitTests\\MultiSource.json";
                textBox3.Text = "..\\..\\..\\UnitTests\\testdump\\";
                textBox4.Text = "OUTPUT.PDF";
                chkSafetyOverwrite.Checked = true;
            }
            if (SelectedAction == act_AddText)
            {
                textBox1.Text = "..\\..\\..\\UnitTests\\AARGform.pdf";
                textBox2.Text = "Angle Test";
                textBox3.Text = "1,123,123,20,50,40,times-roman,16,red";
                textBox4.Text = "..\\..\\..\\UnitTests\\AngleTest.pdf";
                chkSafetyOverwrite.Checked = true;
            }
            if (SelectedAction == act_PDFFromTXT)
            {
                textBox1.Text = "Now is the time ^ for all good people to come to the ^aid of their country^^two lines later^^^^four lines later.";
                textBox2.Text = $"..\\..\\..\\UnitTests\\{SelectedAction.Replace(" ", "")}.pdf";
                textBox3.Text = "";
                textBox4.Text = "";
                chkSafetyOverwrite.Checked = true;
            }
#endif
        }

        void ActionPrep(string givenActionText)
        {
            string PrepLabels = "";
            btnExecute.Enabled = false;
            var thisActPrep = actPrep.Find(ap => ap.ActString == givenActionText);
            if (!(thisActPrep is null)) { btnExecute.Enabled = true; PrepLabels = thisActPrep.Labels; } //invalid action blanks all labels and textboxes
            var PrepString = (PrepLabels + "||||||").Split('|');
            for (int g = 0; g < actSet.Count(); g++)
            {
                actSet[g].ActLabel.Text = PrepString[g];
                actSet[g].ActTextBox.Visible = !string.IsNullOrEmpty(PrepString[g]);
            }
        }

        void ProcessArgs()
        {
            foreach (ActPrep thisPrep in actPrep)
            {
                HelpString += "\r\n" + thisPrep.HelpText;
            }
            foreach (string thisArgument in appArgs)
            {
                Console.WriteLine("yea");
                if (ArgPrefix(ArgPrefix_Help, thisArgument)) { Console.WriteLine(HelpString); if (!ConsoleMode) { MessageBox.Show(HelpString); } this.Close(); }
                //if (ArgPrefix(ArgPrefix_ConsoleMode, thisArgument)) { ConsoleMode = ConsoleMode || ArgEval(thisArgument, ArgVal_ConsoleMode1) || ArgEval(thisArgument, ArgVal_ConsoleMode2); }
                //if (ArgPrefix(ArgPrefix_ConsoleMode, thisArgument)) { ConsoleMode = ConsoleMode || ArgEval(thisArgument, ArgVal_ConsoleMode1) || ArgEval(thisArgument, ArgVal_ConsoleMode2); }
                if (ArgPrefix(ArgPrefix_ConsoleMode, thisArgument)) { ConsoleMode = true; }
                if (ArgPrefix(ArgPrefix_OverWriteSafetyMode, thisArgument)) { pdfToolz.globularSettings.OverwriteSafetyMode = true; }
                //if (ArgPrefix(param_Extract, thisArgument)) { cboAction.SelectedIndex = 1; }
            }
        }

        private bool ArgEval(string givenArg, string givenArgVal, bool givenDefault = false)
        {
            if (givenArg == givenArgVal) { return true; }
            return givenDefault;
        }

        private bool ArgPrefix(string givenPrefix, string givenArg)
        {
            if (givenArg.Length < givenPrefix.Length) { return false; }
            return givenArg.ToLower().Substring(0, givenPrefix.Length) == givenPrefix.ToLower();
        }

        public int myLogger(string logItem)
        {
            if (ConsoleMode)
            {
                Console.WriteLine(logItem);
            }
            else
            {
                txtConsole.AppendText(logItem + "\r\n");
                txtConsole.Refresh();
            }            
            return 1;
        }

        private void chkSafetyOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            pdfToolz.globularSettings.OverwriteSafetyMode = chkSafetyOverwrite.Checked; // sweet hack to get globals
        }

        private void labeldefocus(object sender, EventArgs e)
        {
            txtConsole.Focus(); // my workaround to get textboxes that act like labels - so I can right justify...
        }

    }

    public class ActPrep
    {
        public string ActString { get; set; } 
        public string ParamString { get; set; } 
        public string Labels { get; set; } // delimited by | (so don't use | in the label ext huh???!!!
        public string HelpText { get; set; }
    }

    public class ActSet
    {
        public TextBox ActLabel { get; set; }   // lazy kludge - use text boxes to get right aligned "easily"
        public TextBox ActTextBox { get; set; }
    }

}
