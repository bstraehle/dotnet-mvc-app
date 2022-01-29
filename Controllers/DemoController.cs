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

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<List<Demo>>(content);
                return View(result);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ExceptionMessage = ex.Message });
            }
        }

        public async Task<IActionResult> View(int id)
        {
            return await GetAsync(id);
        }

        public async Task<IActionResult> Update(int id)
        {
            return await GetAsync(id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await GetAsync(id);
        }

        public IActionResult Add()
        {
            Console.Out.WriteLineAsync("Add");

            try
            {
                return View(new Demo());
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ExceptionMessage = ex.Message });
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("UpdateObject {0}", demo));

            try
            {
                var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id), content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ExceptionMessage = ex.Message });
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("DeleteObject {0}", demo));

            try
            {
                var client = new HttpClient();
                var response = await client.DeleteAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ExceptionMessage = ex.Message });
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("AddObject {0}", demo));

            try
            {
                var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(new Uri(demoAPIhost + demoAPIapp), content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ExceptionMessage = ex.Message });
            }
        }

        private async Task<IActionResult> GetAsync(int id)
        {
            await Console.Out.WriteLineAsync(string.Format("Get Id=[{0}]", id));

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp + "/" + id)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return View(JsonConvert.DeserializeObject<Demo>(content));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ExceptionMessage = ex.Message });
            }
        }
    }
}
