﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RoslynTools.Models
{
    public enum NodeType
    {
        node,
        trivia,
        token
    }

    public class NodeModel
    {
        public string name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
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

        public static NodeModel FromTrivia(SyntaxTrivia trivia)
        {
            return new NodeModel
            {
                name = trivia.CSharpKind().ToString(),
                span = new SpanModel
                {
                    start = trivia.Span.Start,
                    length = trivia.Span.Length
                },
                type = NodeType.trivia
            };
        }

        public static NodeModel FromToken(SyntaxToken token)
        {
            return new NodeModel
            {
                name = token.CSharpKind().ToString(),
                span = new SpanModel
                {
                    start = token.Span.Start,
                    length = token.Span.Length
                },
                type = NodeType.token
            };
        }
    }
}