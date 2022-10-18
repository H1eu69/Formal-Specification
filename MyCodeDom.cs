using System;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formal_Specification;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;

namespace MyCODEDOM
{
    class MyCodeDom
    {
        // Build a Hello World program graph using
        // System.CodeDom types.
        public static CodeCompileUnit BuildHelloWorldGraph(string className, FSComponent fs)
        {
            CodeEntryPointMethod start = new CodeEntryPointMethod();

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace samples = new CodeNamespace("FormalSpecification");

            compileUnit.Namespaces.Add(samples);


            samples.Imports.Add(new CodeNamespaceImport("System"));

            CodeTypeDeclaration class1 = new CodeTypeDeclaration(className);
            class1.IsClass = true;
            class1.TypeAttributes = TypeAttributes.Public;
            samples.Types.Add(class1);

            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = fs.func_name;
            method.ReturnType = new CodeTypeReference(fs.output.GetTypeFormat());
            method.Attributes = MemberAttributes.Static| MemberAttributes.Public;

            foreach (var para in fs.parameters)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(para.GetTypeFormat(), para.var_name));

                //get argument input in main func
                if (para.GetTypeFormat() == "System.Double")
                {
                    CodeVariableDeclarationStatement codeVariableDeclaration
                        = new CodeVariableDeclarationStatement(para.GetTypeFormat(),
                        para.var_name,
                        new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Convert)),
                    "ToDouble",
                    new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("Console"),
                    "ReadLine")));
                    start.Statements.Add(codeVariableDeclaration);
                }
                else if (para.GetTypeFormat() == "System.Int32")
                {
                    CodeVariableDeclarationStatement codeVariableDeclaration
                        = new CodeVariableDeclarationStatement(para.GetTypeFormat(),
                        para.var_name,
                        new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Convert)),
                    "ToInt32",
                    new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("Console"),
                    "ReadLine")));
                    start.Statements.Add(codeVariableDeclaration);
                }
                else if (para.GetTypeFormat() == "System.Boolean")
                {
                    CodeVariableDeclarationStatement codeVariableDeclaration
                        = new CodeVariableDeclarationStatement(para.GetTypeFormat(),
                        para.var_name,
                        new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Convert)),
                    "ToDouble",
                    new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("Console"),
                    "ReadLine")));
                    start.Statements.Add(codeVariableDeclaration);
                }
                else
                {
                    CodeVariableDeclarationStatement codeVariableDeclaration
                        = new CodeVariableDeclarationStatement(para.GetTypeFormat(),
                        para.var_name,
                        new CodeMethodInvokeExpression(                 
                    new CodeTypeReferenceExpression("Console"),
                    "ReadLine"));
                    start.Statements.Add(codeVariableDeclaration);
                }                           
            }

            //Func definition
            CodeVariableDeclarationStatement variable = new CodeVariableDeclarationStatement();
            variable.Type = new CodeTypeReference(fs.output.GetTypeFormat());
            variable.Name = fs.output.var_name;
            
            //Get var init
            if (fs.output.GetTypeFormat() == "System.String")
                variable.InitExpression = new CodePrimitiveExpression("");
            if (fs.output.GetTypeFormat() == "System.Double")
                variable.InitExpression = new CodePrimitiveExpression(0);
            if (fs.output.GetTypeFormat() == "System.Int32")
                variable.InitExpression = new CodePrimitiveExpression(0);
            if (fs.output.GetTypeFormat() == "System.Boolean")
                variable.InitExpression = new CodePrimitiveExpression(false);

            method.Statements.Add(variable);

            //Pre, post handle
            if (!fs.pre.isEmptyOrWhiteSpace()) //pre contain condition
            {
                CodeConditionStatement ifElse = new CodeConditionStatement(
                    new CodeSnippetExpression(fs.pre.condition));
                foreach(var item in fs.post.cases)
                {
                    if (item.Contains("&&"))
                    {
                        CodeConditionStatement insideIfElse = new CodeConditionStatement(
                        new CodeSnippetExpression(fs.post.GetCondition(item)));
                        insideIfElse.TrueStatements.Add(
                            new CodeSnippetExpression(fs.post.GetStringInit(item)));
                        ifElse.TrueStatements.Add(insideIfElse);
                    }
                    else
                    {
                       ifElse.TrueStatements.Add(new CodeSnippetExpression(fs.post.GetStringInit(item)));
                    }
                }
                ifElse.TrueStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(fs.output.var_name)));

                ifElse.FalseStatements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", new CodePrimitiveExpression("Sai input")));
                ifElse.FalseStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(fs.output.var_name)));

                method.Statements.Add(ifElse);
            }
            //
            else //pre not contain condition
            {
                foreach (var item in fs.post.cases)
                {
                    if (item.Contains("&&"))
                    {
                        CodeConditionStatement insideIfElse = new CodeConditionStatement(
                            new CodeSnippetExpression(fs.post.GetCondition(item)));
                        insideIfElse.TrueStatements.Add(
                        new CodeSnippetExpression(fs.post.GetStringInit(item)));
                        method.Statements.Add(insideIfElse);
                    }
                    else
                    {
                        method.Statements.Add(new CodeSnippetExpression(fs.post.GetStringInit(item)));
                    }
                }
                method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(fs.output.var_name)));
            }
            class1.Members.Add(method);

            //Main function
            List<CodeExpression> argumentList = new List<CodeExpression>();

            foreach (var name in fs.parameters)
            {
                argumentList.Add(new CodeArgumentReferenceExpression(name.var_name));
            }

            var methodInvokeMain = new CodeMethodInvokeExpression(null,
                fs.func_name, argumentList.ToArray());

            start.Statements.Add(methodInvokeMain);

            start.Statements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", new CodePrimitiveExpression("Press any key to exit program!")));
            start.Statements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "ReadKey"));









            // Declare a new code entry point method.

            // Create a type reference for the System.Console class.

            // Build a Console.WriteLine statement.


            // Add the WriteLine call to the statement collection.


            // Add the ReadLine statement.


            // Add the code entry point method to
            // the Members collection of the type.
            class1.Members.Add(start);

            return compileUnit;
        }

        public static void GenerateCode(CodeDomProvider provider,
            CodeCompileUnit compileunit)
        {
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

            // Create an IndentedTextWriter, constructed with
            // a StreamWriter to the source file.
            IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
            // Generate source code using the code generator.
            provider.GenerateCodeFromCompileUnit(compileunit, tw, new CodeGeneratorOptions());
            // Close the output file.
            tw.Close();
        }

        public static CompilerResults CompileCode(CodeDomProvider provider,
                                                  String sourceFile,
                                                  String exeFile)
        {
            // Configure a CompilerParameters that links System.dll
            // and produces the specified executable file.
            String[] referenceAssemblies = { "System.dll" };
            CompilerParameters cp = new CompilerParameters(referenceAssemblies,
                                                           exeFile, false);
            // Generate an executable rather than a DLL file.
            cp.GenerateExecutable = true;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            // Return the results of compilation.
            return cr;
        }


    }
 }
