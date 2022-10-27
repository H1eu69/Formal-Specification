using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom;
using System.Diagnostics;
using Microsoft.CSharp;
using MyCODEDOM;
using System.CodeDom.Compiler;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Formal_Specification
{
    public partial class Form1 : Form
    {
        public FSComponent FSComponent { get; set; }
        public Form1()
        {
            InitializeComponent();
            toolStrip1.ImageScalingSize = new Size(40, 40);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputRichTextBox.Clear();
            classNameTextBox.Clear();
            ExeFileNameTextBox.Clear();
        }

        private void newToolStripItem_Click(object sender, EventArgs e)
        {
            inputRichTextBox.Clear();
            classNameTextBox.Clear();
            ExeFileNameTextBox.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputRichTextBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
            classNameTextBox.Text = "Program";
            ExeFileNameTextBox.Text = "Application.exe";
        }

        private void openToolStripItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                classNameTextBox.Text = "Program";
                ExeFileNameTextBox.Text = "Application.exe";
                inputRichTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                FSComponent = new FSComponent(inputRichTextBox.Text);

                //Highlight input
                HighlightInput();
            }
        }
        public void HighlightInput()
        {
            int start = inputRichTextBox.Find(FSComponent.func_name);
            inputRichTextBox.Select(start, FSComponent.func_name.Length);
            inputRichTextBox.SelectionColor = Color.OrangeRed;

            List<int> found = new List<int>();
            found.Add(start);
            foreach (var para in FSComponent.parameters)// hightlight parameter type
            {
                start = inputRichTextBox.Find(para.var_type, RichTextBoxFinds.MatchCase);
                int funcNameLength = FSComponent.func_name.Length;
                while (found.Contains(start))
                    start = inputRichTextBox.Find(para.var_type, start + 1, RichTextBoxFinds.MatchCase);
                found.Add(start);
                inputRichTextBox.Select(start, para.var_type.Length);
                inputRichTextBox.SelectionColor = Color.Red;
            }

            start = inputRichTextBox.Find(FSComponent.output.var_type, RichTextBoxFinds.MatchCase); // hightlight output type
            while (found.Contains(start))
                start = inputRichTextBox.Find(FSComponent.output.var_type, start + 1, RichTextBoxFinds.MatchCase);
            found.Add(start);
            inputRichTextBox.Select(start, FSComponent.output.var_type.Length);
            inputRichTextBox.SelectionColor = Color.Red;


            found.Clear(); //Highlight "&&"
            int count = inputRichTextBox.Text.Count(x => x == '&');
            start = inputRichTextBox.Find("&&"); 

            for (int i = 0; i < count; i += 2)
            {
                while (found.Contains(start))
                    start = inputRichTextBox.Find("&&", start + 1, RichTextBoxFinds.MatchCase);
                found.Add(start);
                inputRichTextBox.Select(start, 2);
                inputRichTextBox.SelectionColor = Color.Brown;
            }

            found.Clear(); //Highlight "||"
            count = inputRichTextBox.Text.Count(x => x == '|');
            start = inputRichTextBox.Find("||"); 

            for (int i = 0; i < count; i += 2)
            {
                while (found.Contains(start))
                    start = inputRichTextBox.Find("||", start + 1, RichTextBoxFinds.MatchCase);
                found.Add(start);
                inputRichTextBox.Select(start, 2);
                inputRichTextBox.SelectionColor = Color.Brown;
            }
        }

        // namespace, using, public, private, class, datatype, System.Console, return, if, else, func name
        public void HighlightOutput() 
        {
            int start = solutionRichTextBox.Find("namespace", RichTextBoxFinds.MatchCase);

            solutionRichTextBox.Select(start, 9);
            solutionRichTextBox.SelectionColor = Color.Blue;

            start = solutionRichTextBox.Find("using", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 5);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("using", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("public", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 6);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("public", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("private", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 7);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("private", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("class", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 5);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("class", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("System", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 6);
                solutionRichTextBox.SelectionColor = Color.CadetBlue;
                start = solutionRichTextBox.Find("System", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("Console", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 7);
                solutionRichTextBox.SelectionColor = Color.CadetBlue;
                start = solutionRichTextBox.Find("Console", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("Convert", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 7);
                solutionRichTextBox.SelectionColor = Color.CadetBlue;
                start = solutionRichTextBox.Find("Convert", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("return", RichTextBoxFinds.MatchCase);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 6);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("return", start + 1, RichTextBoxFinds.MatchCase);
            }

            start = solutionRichTextBox.Find("if", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 2);
                solutionRichTextBox.SelectionColor = Color.DeepPink;
                start = solutionRichTextBox.Find("if", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("else", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 4);
                solutionRichTextBox.SelectionColor = Color.DeepPink;
                start = solutionRichTextBox.Find("else", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find(FSComponent.func_name, RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, FSComponent.func_name.Length);
                solutionRichTextBox.SelectionColor = Color.OrangeRed;
                start = solutionRichTextBox.Find(FSComponent.func_name, start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("static", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 6);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("static", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("void", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 4);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("void", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("int", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 3);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("int", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("double", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 6);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("double", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("string", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 6);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("string", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("bool", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 4);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("bool", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("for", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 3);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("for", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("new", RichTextBoxFinds.WholeWord);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, 3);
                solutionRichTextBox.SelectionColor = Color.Blue;
                start = solutionRichTextBox.Find("new", start + 1, RichTextBoxFinds.WholeWord);
            }

            start = solutionRichTextBox.Find("\"");
            int end = solutionRichTextBox.Find("\"",start + 1, RichTextBoxFinds.None);
            while (start != -1)
            {
                solutionRichTextBox.Select(start, end - start + 1);
                solutionRichTextBox.SelectionColor = Color.Brown;
                start = solutionRichTextBox.Find("\"", end + 1, RichTextBoxFinds.None);
                end = solutionRichTextBox.Find("\"", start + 1, RichTextBoxFinds.None);
            }
        }


        private void saveToolStripItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "CSharp (*.cs)|*.cs|All files (*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(saveFileDialog1.FileName.ToString());
                file.WriteLine(inputRichTextBox.Text);
                file.Close();   
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

         private void generateButton_Click(object sender, EventArgs e)
        {
            if (inputRichTextBox.Text != "")
            {
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                MyCodeDom.GenerateCode(provider, MyCodeDom.BuildHelloWorldGraph(classNameTextBox.Text, FSComponent));

                // Build the source file name with the appropriate
                // language extension.
                String sourceFile;
                if (provider.FileExtension[0] == '.')
                {
                    sourceFile = "FormalSpecification" + provider.FileExtension;
                }
                else
                {
                    sourceFile = "FormalSpecification." + provider.FileExtension;
                }

                // Read in the generated source file and
                // display the source text.
                StreamReader sr = new StreamReader(sourceFile);
                solutionRichTextBox.Text = sr.ReadToEnd();
                sr.Close();

                HighlightOutput();
            }
        }

        private void buildButton_Click(object sender, EventArgs e)
        {

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            // Build the source file name with the appropriate
            // language extension.
            String sourceFile;
            if (provider.FileExtension[0] == '.')
            {
                sourceFile = "FormalSpecification" + provider.FileExtension;
            }
            else
            {
                sourceFile = "FormalSpecification." + provider.FileExtension;
            }

            // Compile the source file into an executable output file.
            CompilerResults cr = MyCodeDom.CompileCode(provider,
                                                            sourceFile,
                                                            ExeFileNameTextBox.Text);

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                MessageBox.Show(cr.Errors[0].ToString());
            }
            else
            {
                Process.Start(ExeFileNameTextBox.Text);
            }

        }


        

    }

}
