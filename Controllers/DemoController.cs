using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace DemoApp.Controllers
{
    public class DemoController : Controller
    {
        private string demoAPIhost = "";
        private readonly string demoAPIapp = "/Demo";

        public DemoController(IConfiguration config)
        {
            var host = "DEMO_API_HOST";

            demoAPIhost = Environment.GetEnvironmentVariable(host) ?? "";

            if (string.IsNullOrEmpty(demoAPIhost))
            {
                demoAPIhost = config.GetValue(host, "");
            }

            Console.Out.WriteLineAsync(string.Format("URI=[{0}{1}]", demoAPIhost, demoAPIapp));
        }

        public async Task<IActionResult> List()
        {
            await Console.Out.WriteLineAsync("List");

            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp)).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                dynamic data = JObject.Parse(content);
                return View("Error", new ErrorViewModel { ExceptionMessage = data.Result });
            }

            return View(JsonConvert.DeserializeObject<List<Demo>>(content));
        }

        public async Task<IActionResult> View(int id)
        {
            return await GetAsync(id);
        }

        public IActionResult Add()
        {
            Console.Out.WriteLineAsync("Add");

            return View(new Demo());
        }

        public async Task<IActionResult> Update(int id)
        {
            return await GetAsync(id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await GetAsync(id);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("Add Demo=[{0}]", demo));

            var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PostAsync(new Uri(demoAPIhost + demoAPIapp), content).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                dynamic data = JObject.Parse(error);
                return View("Error", new ErrorViewModel { ExceptionMessage = data.Result });
            }

            return RedirectToAction("List");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("Update Demo=[{0}]", demo));

            var content = new StringContent(JsonConvert.SerializeObject(demo), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PutAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id), content).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                dynamic data = JObject.Parse(error);
                return View("Error", new ErrorViewModel { ExceptionMessage = data.Result });
            }

            return RedirectToAction("List");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteObject(Demo demo)
        {
            await Console.Out.WriteLineAsync(string.Format("Delete Demo=[{0}]", demo));

            var client = new HttpClient();
            var response = await client.DeleteAsync(new Uri(demoAPIhost + demoAPIapp + "/" + demo.Id)).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                dynamic data = JObject.Parse(error);
                return View("Error", new ErrorViewModel { ExceptionMessage = data.Result });
            }

            return RedirectToAction("List");
        }

        private async Task<IActionResult> GetAsync(int id)
        {
            await Console.Out.WriteLineAsync(string.Format("Get Id=[{0}]", id));

            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(demoAPIhost + demoAPIapp + "/" + id)).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                dynamic data = JObject.Parse(content);
                return View("Error", new ErrorViewModel { ExceptionMessage = data.Result });
            }

            return View(JsonConvert.DeserializeObject<Demo>(content));
        }

        private string GetErrMsg(HttpResponseMessage response)
        {
            return "";
        }
    }
}
