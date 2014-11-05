using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynTools.Models;

namespace RoslynTools.Controllers.Api
{
    public class DumpTreeVisitor : CSharpSyntaxWalker
    {
        private NodeModel _current;

        public NodeModel Root { get; set; }

        public DumpTreeVisitor()
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
            }

            base.Visit(node);

            _current = temp;
        }
    }
}