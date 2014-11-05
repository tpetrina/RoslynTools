using System.Web.Http;

namespace RoslynTools.Controllers.Api
{
    public class CompilerController : ApiController
    {
        public string Get()
        {
            return "Hi";
        }

        public string Post([FromBody]string text)
        {
            return text ?? string.Empty;
        }
    }
}