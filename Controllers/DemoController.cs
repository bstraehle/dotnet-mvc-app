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
            demoAPIapp = config.GetValue("DemoAPIapp", "");

            Console.Out.WriteLineAsync(string.Format("URI=[{0}{1}]", demoAPIhost, demoAPIapp));
        }

        public async Task<IActionResult> List()
        {
            await Console.Out.WriteLineAsync("List");

            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<List<Demo>>(content);
            return View(result);
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await GetAsync(id));
        }

        public async Task<IActionResult> Update(int id)
        {
            return View(await GetAsync(id));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await GetAsync(id));
        }

        public IActionResult Add()
        {
            Console.Out.WriteLineAsync("Add");

            return View(new Demo());
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("UpdateObject {0}", demo));

            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id), content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("List");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("DeleteObject {0}", demo));

            var client = new HttpClient();
            var response = await client.DeleteAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("List");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("AddObject {0}", demo));

            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(new Uri(demoAPIhost + demoAPIapp), content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("List");
        }

        private async Task<Demo> GetAsync(int id)
        {
            await Console.Out.WriteLineAsync(string.Format("Get Id=[{0}]", id));

            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp + "/" + id)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Demo>(content);
        }
    }
}
