using Umbraco.Cms.Web.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/blog")]
public class BlogController : UmbracoApiController
{
    private readonly HttpClient _httpClient;

    public BlogController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetData([FromQuery] int take, [FromQuery] int skip,[FromQuery] string tag = null)
    {
        string takeFilter = "&take=" + (take!=null?take.ToString():"1000");
        string skipFilter = skip !=null? "&skip=" + skip:"";
        if (!string.IsNullOrEmpty(tag))
        {
            // טיפול במקרה של קבלת תגית
            var url = $"https://localhost:44379/umbraco/delivery/api/v2/content?filter=tag:{tag}"+takeFilter+skipFilter;
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode);
        }
        else
        {
            // טיפול במקרה של קבלת נתונים ללא תגית
            var url = "https://localhost:44379/umbraco/delivery/api/v2/content?filter=contentType:post"+takeFilter+skipFilter;
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode);
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetDataById(string id)
    {
        var url = $"https://localhost:44379/umbraco/delivery/api/v2/content/item/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return Ok(data);
        }

        return StatusCode((int)response.StatusCode);
    }

   
}
