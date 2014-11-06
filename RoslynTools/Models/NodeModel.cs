using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
        public object raw { get; set; }

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
                },
                raw = ToJson(syntaxNode, new List<string> { "Parent" })
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
                type = NodeType.trivia,
                raw = ToJson(trivia, new List<string> { "Parent" })
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
                type = NodeType.token,
                raw = ToJson(token, new List<string> { "Parent" })
            };
        }

        private static string ToJson<T>(T t, List<string> ignore)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            var properties = t.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (var i = 0; i < properties.Length; ++i)
            {
                var property = properties[i];
                if (ignore.Contains(property.Name)) continue;

                if (i > 0) sb.Append(",");

                try
                {
                    sb.AppendFormat("\"{0}\":{1}", property.Name, JsonConvert.SerializeObject(property.GetValue(t)));
                }
                catch (Exception ex)
                {
                    // swallow this exception
                }
            }

            sb.Append("}");
            return sb.ToString();
        }
    }
}