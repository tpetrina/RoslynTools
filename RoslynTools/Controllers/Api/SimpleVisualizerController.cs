using System;
using System.Web.Http;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTools.Controllers.Api
{
    public class SimpleVisualizerController : ApiController
    {
        public string Post([FromBody]string text)
        {
            try
            {
                var tree = SyntaxFactory.ParseSyntaxTree(text ?? string.Empty, new CSharpParseOptions());

                var visitor = new SimpleVisualizerVisitor();
                visitor.Visit(tree.GetRoot());
                return visitor.Content;
            }
            catch (Exception e)
            {
                return string.Format("Exception caught: '{0}'", e);
            }
        }
    }
}