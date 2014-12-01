using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTools.Common
{
    public static class CSharpToHtml
    {
        public static async Task<string> Convert(string code)
        {

            code = code ?? string.Empty;

            try
            {
                using (var workspace = new CustomWorkspace())
                {
                    var solution = workspace.CurrentSolution;
                    var project = solution.AddProject("newproject", "newassembly", LanguageNames.CSharp);
                    var document = project.AddDocument("newfile.cs", code);

                    document = await Formatter.FormatAsync(document);
                    var text = await document.GetTextAsync();

                    var sourceText = await document.GetTextAsync();
                    code = sourceText.ToString();

                    var classifiedSpans =
                        (await Classifier.GetClassifiedSpansAsync(document, TextSpan.FromBounds(0, text.Length)))
                            .ToList();

                    var sb = new StringBuilder();
                    var current = 0;
                    foreach (var classifiedSpan in classifiedSpans)
                    {
                        var span = classifiedSpan.TextSpan;
                        if (span.Start > current)
                        {
                            sb.Append(code.Substring(current, span.Start - current)
                                .Replace(" ", "&nbsp")
                                .Replace("\n", "<br />"
                                ));
                        }

                        sb.AppendFormat("<span class='{1}'>{0}</span>", code.Substring(span.Start, span.Length)
                            .Replace(" ", "&nbsp")
                            .Replace("\n", "<br />"),
                            classifiedSpan.ClassificationType.Replace(' ', '-'));

                        current = span.End;
                    }

                    return sb.ToString();
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(ex.ToString());
                foreach (var e in ex.LoaderExceptions)
                {
                    sb.AppendLine("------------");
                    sb.AppendLine(e.ToString());
                }
                return sb.ToString();
            }

            catch (Exception ex)
            {
                return "Exception: " + ex;
            }
        }
    }
}
