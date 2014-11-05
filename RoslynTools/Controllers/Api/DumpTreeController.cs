using System;
using System.Web.Http;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTools.Controllers.Api
{
    public class DumpTreeController : ApiController
    {
        public object Post([FromBody] string text)
        {
            try
            {
                var tree = SyntaxFactory.ParseSyntaxTree(text ?? string.Empty, new CSharpParseOptions());

                var visitor = new DumpTreeVisitor();
                visitor.Visit(tree.GetRoot());
                return visitor.Root;
            }
            catch (Exception e)
            {
                return string.Format("Exception caught: '{0}'", e);
            }
        }
    }
}