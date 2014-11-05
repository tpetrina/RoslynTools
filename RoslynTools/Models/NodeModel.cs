using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace RoslynTools.Models
{
    public enum NodeType
    {
        Node,
        Trivia,
        Token
    }

    public class NodeModel
    {
        public string name { get; set; }
        public NodeType type { get; set; }
        public List<NodeModel> children { get; set; }
        public SpanModel span { get; set; }

        private NodeModel()
        {
            children = new List<NodeModel>();
        }

        public static NodeModel FromSyntaxNode(SyntaxNode syntaxNode)
        {
            return new NodeModel
            {
                name = syntaxNode.CSharpKind().ToString(),
                span = new SpanModel
                {
                    start = syntaxNode.Span.Start,
                    length = syntaxNode.Span.Length
                }
            };
        }
    }
}