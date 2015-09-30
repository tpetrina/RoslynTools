using System.Threading.Tasks;
using System.Web.Http;

namespace RoslynTools.Controllers.Api
{
    public class ConvertToHtmlController : ApiController
    {
        public Task<string> Post([FromBody] string code)
        {
            return RoslynTools.Common.CSharpToHtml.Convert(code);
        }
    }
}
