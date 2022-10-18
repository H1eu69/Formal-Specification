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
                inputRichTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                classNameTextBox.Text = "Program";
                ExeFileNameTextBox.Text = "Application.exe";
                FSComponent = new FSComponent(inputRichTextBox.Text);
            }
            
        }


        private void saveToolStripItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Text Document (*.txt)|*.txt|All files (*.*)|*.*";

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
                    sourceFile = "TestGraph" + provider.FileExtension;
                }
                else
                {
                    sourceFile = "TestGraph." + provider.FileExtension;
                }

                // Read in the generated source file and
                // display the source text.
                StreamReader sr = new StreamReader(sourceFile);
                solutionRichTextBox.Text = sr.ReadToEnd();
                sr.Close();
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
                sourceFile = "TestGraph" + provider.FileExtension;
            }
            else
            {
                sourceFile = "TestGraph." + provider.FileExtension;
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
