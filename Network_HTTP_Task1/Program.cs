using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Network_HTTP_Task1
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.Write("Введіть назву міста (наприклад, Kyiv): ");
            string city = Console.ReadLine() ?? string.Empty;

            string apiKey = "d5f6cf0491de7bd7b85fc17379c3966d";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=ua";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Місто не знайдено або невірний API ключ (Код: {response.StatusCode})");
                    return;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = doc.RootElement;

                    string cityName = root.GetProperty("name").GetString();
                    double temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                    int humidity = root.GetProperty("main").GetProperty("humidity").GetInt32();
                    string description = root.GetProperty("weather")[0].GetProperty("description").GetString();

                    Console.WriteLine($"\n--- Погода в місті {cityName} ---");
                    Console.WriteLine($"Температура: {temp} °C");
                    Console.WriteLine($"Опис: {description}");
                    Console.WriteLine($"Вологість: {humidity}%");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
