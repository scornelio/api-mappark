using mappark.api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System;
using System.Text.Json.Nodes;

namespace mappark.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineLearningController : ControllerBase
    {
        [HttpPost("ProbabilidadEstacionamiento")]
        public async Task<string> ProbabilidadEstacionamiento([FromBody] RequestProbabilidadEstacionamiento request)
        {
            

            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://v2-miguel-aparca.eastus2.inference.ml.azure.com/score");
            httpRequest.Headers.Add("Authorization", "Bearer oVxmLDliHMZSl2JbsHzV4Q6ziVZ3TLie");

            using (var manipulador = new RequestManipulador())
            {
                try
                {
                    request.DensidadPoblacion = await manipulador.ObtenerYModificarCodigoPostal(request.CodigoPostal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    request.DensidadPoblacion = null;                    
                }
                
            }


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

    public class RequestManipulador : IDisposable
    {
        public async Task<decimal?> ObtenerYModificarCodigoPostal(int? codigoPostal)
        {
            return await ObtenerDensidadPoblacion(codigoPostal);
        }

        public async Task<Decimal?> ObtenerDensidadPoblacion(int? codigoPostal)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "srv-db-atmira-ia.database.windows.net";
                builder.UserID = "administrador";
                builder.Password = "@tm1r4IA2024";
                builder.InitialCatalog = "db-atmira-ia-v2";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    String sql = "SELECT Densidad FROM dbo.UbicacionDensidadPoblacion WHERE dbo.UbicacionDensidadPoblacion.CodigoPostal = " + codigoPostal;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // Verificar si hay al menos una fila
                            if (reader.Read())
                            {
                                Console.WriteLine(reader[0]);
                                return (Decimal)reader[0];
                            }
                            else
                            {
                                // No se encontraron filas
                                return 0;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Error");
            throw new NotImplementedException();
        }
    }
  
}
