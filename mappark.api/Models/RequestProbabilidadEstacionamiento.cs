using System.Text.Json.Serialization;

namespace mappark.api.Models
{

    public class RequestProbabilidadEstacionamiento
    {
        [JsonPropertyName("idProbabilidadEstacionamiento")]
        public int IdProbabilidadEstacionamiento { get; set; }

        [JsonPropertyName("latitud")]
        public double Latitud { get; set; }

        [JsonPropertyName("longitud")]
        public double Longitud { get; set; }

        [JsonPropertyName("pais")]
        public string Pais { get; set; }

        [JsonPropertyName("ciudad")]
        public string Ciudad { get; set; }

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; }

        [JsonPropertyName("barrio")]
        public string Barrio { get; set; }

        [JsonPropertyName("nombreVia")]
        public string NombreVia { get; set; }

        [JsonPropertyName("codigoPostal")]
        public int CodigoPostal { get; set; }

        [JsonPropertyName("fechaCompleta")]
        public DateTime FechaCompleta { get; set; }

        [JsonPropertyName("año")]
        public int Año { get; set; }

        [JsonPropertyName("nombreDiaSemana")]
        public string NombreDiaSemana { get; set; }

        [JsonPropertyName("nombreMes")]
        public string NombreMes { get; set; }

        [JsonPropertyName("horaInicio")]
        public string HoraInicio { get; set; }

        [JsonPropertyName("horaFin")]
        public string HoraFin { get; set; }

        [JsonPropertyName("temperatura")]
        public double Temperatura { get; set; }

        [JsonPropertyName("precipitacion")]
        public double Precipitacion { get; set; }

        [JsonPropertyName("densidadTrafico")]
        public double? DensidadTrafico { get; set; }
    }

    public class RequestWrapper
    {
        [JsonPropertyName("input_data")]
        public InputData InputData { get; set; }
    }

    public class InputData
    {
        [JsonPropertyName("data")]
        public List<RequestProbabilidadEstacionamiento> Data { get; set; }
    }
}
