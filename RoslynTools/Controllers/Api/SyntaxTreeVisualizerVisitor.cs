using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynTools.Models;

namespace RoslynTools.Controllers.Api
{
    public class SyntaxTreeVisualizerVisitor : CSharpSyntaxWalker
    {
        private NodeModel _current;

        public NodeModel Root { get; set; }

        public SyntaxTreeVisualizerVisitor()
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
        }

        public override void Visit(SyntaxNode node)
        {
            var temp = _current;
            var nodeModel = NodeModel.FromSyntaxNode(node);

            if (_current == null)
            {
                _current = Root = nodeModel;
            }
            else
            {
                _current.children.Add(nodeModel);
                _current = nodeModel;
            }

            base.Visit(node);

            _current = temp;
        }

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            var temp = _current;
            var nodeModel = NodeModel.FromTrivia(trivia);

            if (_current == null)
            {
                _current = Root = nodeModel;
            }
            else
            {
                _current.children.Add(nodeModel);
                _current = nodeModel;
            }

            base.VisitTrivia(trivia);

            _current = temp;
        }

        public override void VisitToken(SyntaxToken token)
        {
            var temp = _current;
            var nodeModel = NodeModel.FromToken(token);

            if (_current == null)
            {
                _current = Root = nodeModel;
            }
            else
            {
                _current.children.Add(nodeModel);
                _current = nodeModel;
            }

            base.VisitToken(token);

            _current = temp;
        }
    }
}