using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynTools.Models;

namespace RoslynTools.Controllers.Api
{
    public class SimpleExpressionCompilerController : ApiController
    {
        public object Post([FromBody] string expression)
        {
            try
            {
                const string preCode = @"public class Submission
{
    public static object Run()
    {
";
                const string postCode = @"
    } 
}";

                var fullCode = preCode + expression + postCode;

                var tree = SyntaxFactory.ParseSyntaxTree(fullCode);
                var compilation = CSharpCompilation.Create(
                    "submission.dll",
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                    syntaxTrees: new[] { tree },
                    references: new MetadataReference[]
                    {
                        new MetadataFileReference(typeof(object).Assembly.Location), 
                    });

                var diagnostics = compilation.GetDiagnostics().ToList();
                if (diagnostics.Any())
                {
                    return FormatError(diagnostics, preCode.Length);
                }

                Assembly compiledAssembly;
                using (var stream = new MemoryStream())
                {
                    var compileResult = compilation.Emit(stream);
                    compiledAssembly = Assembly.Load(stream.GetBuffer());
                }

                var calculator = compiledAssembly.GetType("Submission");
                var evaluate = calculator.GetMethod("Run");
                var result = evaluate.Invoke(null, null).ToString();

                return result;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex;
            }
        }

        private static CompilationResultModel FormatError(List<Diagnostic> diagnostics, int offset)
        {
            var result = new CompilationResultModel();

            foreach (var diagnostic in diagnostics)
            {
                result.errors.Add(new ErrorModel
                {
                    message = diagnostic.GetMessage(),
                    span = new SpanModel
                    {
                        start = diagnostic.Location.SourceSpan.Start - offset,
                        length = diagnostic.Location.SourceSpan.Length
                    }
                });
            }

            return result;
        }
    }
}
