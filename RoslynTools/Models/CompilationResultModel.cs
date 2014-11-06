using System.Collections.Generic;

namespace RoslynTools.Models
{
    public class CompilationResultModel
    {
        public List<ErrorModel> errors { get; set; }

        public CompilationResultModel()
        {
            errors = new List<ErrorModel>();
        }
    }

    public class ErrorModel
    {
        public string message { get; set; }
        public SpanModel span { get; set; }
    }
}