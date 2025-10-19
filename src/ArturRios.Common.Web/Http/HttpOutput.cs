using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ArturRios.Common.Web.Http;

public class HttpOutput<TBody>(HttpResponseMessage responseMessage)
{
    public HttpStatusCode StatusCode { get; set; } = responseMessage.StatusCode;
    public HttpResponseHeaders Headers { get; set; } = responseMessage.Headers;
    public TBody? Body { get; set; }

    public async Task ReadContent()
    {
        var body = await responseMessage.Content.ReadAsStringAsync();

        Body = JsonConvert.DeserializeObject<TBody>(body);
    }
}
