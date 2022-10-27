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

            DataType arraySize = new DataType();
            foreach (var para in fs.parameters)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(para.GetTypeFormat(), para.var_name));

                //get argument input in main func
                if (para.GetTypeFormat() == "System.Double")
                {
                    arraySize = para;
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
                    arraySize = para;

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
                    arraySize = para;

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
                else if(para.GetTypeFormat() == "System.Boolean")
                {
                    arraySize = para;

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
                foreach(var item in fs.post.cases) // post handle
                {
                    if (!hasArray(fs))
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
                    else
                    {

                    }
                }
                ifElse.TrueStatements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", new CodePrimitiveExpression("Ket qua la {0}"), new CodeSnippetExpression(fs.output.var_name)));

                ifElse.TrueStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(fs.output.var_name)));

                ifElse.FalseStatements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", new CodePrimitiveExpression("Sai input")));
                ifElse.FalseStatements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(fs.output.var_name)));

                method.Statements.Add(ifElse);
            }
            
            else //pre not contain condition
            {
                foreach (var item in fs.post.cases)
                {
                    if (!hasArray(fs))
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
                    else
                    {

                    }
                }
                method.Statements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"),
                "WriteLine", new CodePrimitiveExpression("Ket qua la {0}"), new CodeSnippetExpression(fs.output.var_name)));

                method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(fs.output.var_name)));
            }
            class1.Members.Add(method);

            //Main function
            List<CodeExpression> argumentList = new List<CodeExpression>();

            foreach (var name in fs.parameters)
            {
                argumentList.Add(new CodeArgumentReferenceExpression(name.var_name));
            }

            if (hasArray(fs))
            {
                CodeMemberMethod initArrayMethod = new CodeMemberMethod();
                initArrayMethod.Name = "NhapMang";
                initArrayMethod.ReturnType = new CodeTypeReference();
                initArrayMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;
                class1.Members.Add(initArrayMethod);
                foreach (var para in fs.parameters)
                {
                    initArrayMethod.Parameters.Add(new CodeParameterDeclarationExpression(para.GetTypeFormat(), para.var_name));

                    if (para.GetTypeFormat() == "System.Double[]")
                    {
                        CodeVariableDeclarationStatement codeVariableDeclaration
                            = new CodeVariableDeclarationStatement(para.GetTypeFormat(),
                            para.var_name,
                            new CodeArrayCreateExpression("System.Double", 1000));

                        start.Statements.Add(codeVariableDeclaration);

                        CodeVariableDeclarationStatement testInt =
                    new CodeVariableDeclarationStatement(typeof(int),
                    "i",
                    new CodePrimitiveExpression(0));

                        // Creates a for loop that sets testInt to 0 and continues incrementing testInt by 1 each loop until testInt is not less than 10.
                        CodeIterationStatement forLoop = new CodeIterationStatement(

                            new CodeAssignStatement(
                                new CodeVariableReferenceExpression("i"),
                            new CodePrimitiveExpression(0)),

                            new CodeBinaryOperatorExpression(
                                new CodeVariableReferenceExpression("i"),
                                CodeBinaryOperatorType.LessThan,
                                new CodeVariableReferenceExpression(arraySize.var_name)),

                            new CodeAssignStatement(
                                new CodeVariableReferenceExpression("i"),
                            new CodeBinaryOperatorExpression(
                                new CodeVariableReferenceExpression("i"),
                                CodeBinaryOperatorType.Add,
                                new CodePrimitiveExpression(1))),


                            new CodeStatement[] { 
                                new CodeSnippetStatement("\ta[i] = System.Convert.ToDouble(Console.ReadLine());\r\n") 
                            });

                        initArrayMethod.Statements.Add(testInt);
                        initArrayMethod.Statements.Add(forLoop);
                        var initArrayMethodInvoke = new CodeMethodInvokeExpression(null,
                            "NhapMang", argumentList.ToArray());

                        start.Statements.Add(initArrayMethodInvoke);
                    }
                    else if (para.GetTypeFormat() == "System.Int32[]")
                    {
                        CodeVariableDeclarationStatement codeVariableDeclaration
                            = new CodeVariableDeclarationStatement(para.GetTypeFormat(),
                            para.var_name,
                            new CodeArrayCreateExpression("System.Double", 1000));

                        start.Statements.Add(codeVariableDeclaration);

                        CodeVariableDeclarationStatement testInt =
                    new CodeVariableDeclarationStatement(typeof(int),
                    "i",
                    new CodePrimitiveExpression(0));

                        // Creates a for loop that sets testInt to 0 and continues incrementing testInt by 1 each loop until testInt is not less than 10.
                        CodeIterationStatement forLoop = new CodeIterationStatement(

                            new CodeAssignStatement(
                                new CodeVariableReferenceExpression("i"),
                            new CodePrimitiveExpression(0)),

                            new CodeBinaryOperatorExpression(
                                new CodeVariableReferenceExpression("i"),
                                CodeBinaryOperatorType.LessThan,
                                new CodeVariableReferenceExpression(arraySize.var_name)),

                            new CodeAssignStatement(
                                new CodeVariableReferenceExpression("i"),
                            new CodeBinaryOperatorExpression(
                                new CodeVariableReferenceExpression("i"),
                                CodeBinaryOperatorType.Add,
                                new CodePrimitiveExpression(1))),


                            new CodeStatement[] {
                                new CodeSnippetStatement("\ta[i] = System.Convert.ToInt32(Console.ReadLine());\r\n")
                            });

                        initArrayMethod.Statements.Add(testInt);
                        initArrayMethod.Statements.Add(forLoop);
                        var initArrayMethodInvoke = new CodeMethodInvokeExpression(null,
                            "NhapMang", argumentList.ToArray());

                        start.Statements.Add(initArrayMethodInvoke);
                    }
                }

                

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
                sourceFile = "FormalSpecification" + provider.FileExtension;
            }
            else
            {
                sourceFile = "FormalSpecification." + provider.FileExtension;
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

        public static bool hasArray(FSComponent fs)
        {
            foreach(var paras in fs.parameters)
            {
                if (paras.GetTypeFormat() == "System.Double[]" || (paras.GetTypeFormat() == "System.Int32[]"))
                    return true;
            }
            return false;
        }

    }
 }
