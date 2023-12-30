using Microsoft.AspNetCore.Mvc;
using DndDungeons.UI.Models.DTO;
using System.Runtime.CompilerServices;
using DndDungeons.UI.Models;
using System.Text.Json;
using System.Text;
using System.Reflection;

namespace DndDungeons.UI.Controllers
{
    public class DndRegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public DndRegionsController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<DndRegionDTO> response = new List<DndRegionDTO>();

            try
            {
                // Get All Regions from Web API
                HttpClient client = _httpClientFactory.CreateClient();

                HttpResponseMessage httpResponseMessage = await client.GetAsync("https://localhost:7288/api/DndRegions");

                httpResponseMessage.EnsureSuccessStatusCode();

                //IEnumerable<DndRegionDTO> stringResponseBody = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DndRegionDTO>>();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DndRegionDTO>>());

                //ViewBag.Response = stringResponseBody;
            }
            catch (Exception ex)
            {

            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDndRegionViewModel model)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7288/api/DndRegions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };

            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            DndRegionDTO response = await httpResponseMessage.Content.ReadFromJsonAsync<DndRegionDTO>();

            if (response is not null)
            {
                return RedirectToAction("Index", "DndRegions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) // "id" has to match the name of the route. We named it
                                                       // asp-route-id, so the last "id" bit has to be the same
        {
            //ViewBag.Id = id;
            HttpClient client = _httpClientFactory.CreateClient();

            DndRegionDTO response = await client.GetFromJsonAsync<DndRegionDTO>($"https://localhost:7288/api/DndRegions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DndRegionDTO request)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7288/api/DndRegions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
            };

            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            DndRegionDTO response = await httpResponseMessage.Content.ReadFromJsonAsync<DndRegionDTO>();

            if (response is not null)
            {
                return RedirectToAction("Edit", "DndRegions");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DndRegionDTO request)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();

                HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"https://localhost:7288/api/DndRegions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "DndRegions");
            }
            catch (Exception ex)
            {

            }

            return View("Edit");
        }
    }
}
