using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTools.Controllers.Api
{
    public class SimpleVisualizerVisitor : CSharpSyntaxWalker
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private int _indent;

        public string Content
        {
            get { return _sb.ToString(); }
        }

        public SimpleVisualizerVisitor()
            : base(SyntaxWalkerDepth.StructuredTrivia)
        { }

        public override void Visit(SyntaxNode node)
        {
            _sb.AppendFormat(new string(' ', _indent * 2) + "{0} ({1}-{2})", node.CSharpKind(), node.Span.Start, node.Span.Start + node.Span.Length);
            _sb.AppendLine();

            this._indent++;
            base.Visit(node);
            this._indent--;
        }

        public override void VisitToken(SyntaxToken token)
        {
            _sb.AppendFormat(new string(' ', _indent * 2) + "{0} ({1}-{2})", token.CSharpKind(), token.Span.Start, token.Span.Start + token.Span.Length);
            _sb.AppendLine();

            this._indent++;
            base.VisitToken(token);
            this._indent--;
        }

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            _sb.AppendFormat(new string(' ', _indent * 2) + "{0} ({1}-{2})", trivia.CSharpKind(), trivia.Span.Start, trivia.Span.Start + trivia.Span.Length);
            _sb.AppendLine();

            this._indent++;
            base.VisitTrivia(trivia);
            this._indent--;
        }
    }
}