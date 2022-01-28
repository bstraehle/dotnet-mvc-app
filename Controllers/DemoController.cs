using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DemoApp.Controllers
{
    public class DemoController : Controller
    {
        private string demoAPIhost;
        private string demoAPIapp;

        public DemoController(IConfiguration config)
        {
            demoAPIhost = config.GetValue("DemoAPIhost", "");
            demoAPIapp = config.GetValue("DemoAPIapp", ""); ;
        }

        public async Task<IActionResult> List()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<List<Demo>>(content);
            return View(result);
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await GetDemoAsync(id));
        }

        public async Task<IActionResult> Update(int id)
        {
            return View(await GetDemoAsync(id));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await GetDemoAsync(id));
        }

        public IActionResult Add()
        {
            return View(new Demo());
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateObject(Demo demo)
        {
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id), content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("List");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteObject(Demo demo)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("List");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddObject(Demo demo)
        {
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(new Uri(demoAPIhost + demoAPIapp), content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("List");
        }

        private async Task<Demo> GetDemoAsync(int id)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp + "/" + id)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Demo>(content);
        }
    }
}
