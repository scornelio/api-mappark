using mappark.api.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace mappark.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineLearningController : ControllerBase
    {
        [HttpPost]
        public async Task<string> ProbabilidadEstacionamiento([FromBody] RequestProbabilidadEstacionamiento request)
        {
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://az-ml-atmira-reboots-conn-point.westeurope.inference.ml.azure.com/score");
            httpRequest.Headers.Add("Authorization", "Bearer qfSc0byLka3RB5a3LJUNNyhwGYuTErqs");

            var requestWrapper = new RequestWrapper
            {
                InputData = new InputData
                {
                    Data = new List<RequestProbabilidadEstacionamiento> { request }
                }
            };

            string json = JsonSerializer.Serialize(requestWrapper);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            httpRequest.Content = content;
            var response = await client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            var probabilities = JsonSerializer.Deserialize<List<double>>(result);
            if (probabilities == null || probabilities.Count == 0)
            {
                return "No se ha podido obtener la probabilidad";
            }

            double probability = probabilities[0];
            string category;

            if (probability < 0.50)
            {
                category = "Baja";
            }
            else if (probability <= 0.85)
            {
                category = "Media";
            }
            else
            {
                category = "Alta";
            }

            return category;
        }
       
    }
}
