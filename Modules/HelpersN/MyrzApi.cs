using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoEat.Modules.HelpersN;

public class MyrzApi : IDisposable
{
    private readonly HttpClient _client;
    private readonly string _key;
    private const string BaseUrl = "https://antipublic.one/api/v2/";

    public MyrzApi(string key)
    {
        _key = key;

        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };

        // Добавляем заголовки, чтобы имитировать браузер
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    }

    private async Task<T> GetAsync<T>(string path)
    {
        string url = $"{BaseUrl}{path}?key={_key}"; // Теперь полный URL
        Console.WriteLine($"Запрос к API: {url}"); // Вывод запроса

        var response = await _client.GetAsync(url);
        var responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Ответ от API: {responseString}"); // Лог ответа

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Ошибка API: {response.StatusCode} - {responseString}");
        }

        return JsonConvert.DeserializeObject<T>(responseString);
    }

    private async Task<T> PostAsync<T>(string path, Dictionary<string, string> parameters)
    {
        parameters["key"] = _key;
        var content = new FormUrlEncodedContent(parameters);
        
        string url = $"{BaseUrl}{path}"; // Исправление формирования URL
        Console.WriteLine($"POST запрос к API: {url}");

        var response = await _client.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Ответ от API: {responseString}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Ошибка API: {response.StatusCode} - {responseString}");
        }

        return JsonConvert.DeserializeObject<T>(responseString);
    }

    public async Task<int> GetLineCount()
    {
        var response = await GetAsync<CountLinesResponse>("countLines");
        return response.Count;
    }

    public async Task<CheckAccessResponse> CheckAccess()
    {
        return await GetAsync<CheckAccessResponse>("checkAccess");
    }

    public async Task<AvailableQueriesResponse> GetAvailableQueries()
    {
        return await GetAsync<AvailableQueriesResponse>("availableQueries");
    }

    public async Task<CheckLinesResponse> CheckLines(string[] lines, bool insert = false)
    {
        var parameters = new Dictionary<string, string>
        {
            { "lines[]", string.Join(",", lines) },
            { "insert", insert ? "yes" : "no" }
        };
        return await PostAsync<CheckLinesResponse>("checkLines", parameters);
    }

    public async Task<EmailSearchResponse> SearchEmail(string email)
    {
        var parameters = new Dictionary<string, string> { { "email", email } };
        return await PostAsync<EmailSearchResponse>("emailSearch", parameters);
    }

    public async Task<EmailSearchResponse> GetEmailPasswords(string[] emails, int passwordLimit = 100)
    {
        var parameters = new Dictionary<string, string>
        {
            { "emails[]", string.Join(",", emails) },
            { "limit", passwordLimit.ToString() }
        };
        return await PostAsync<EmailSearchResponse>("emailPasswords", parameters);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public class CountLinesResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class CheckAccessResponse
    {
        public bool Success { get; set; }
        public bool Plus { get; set; }
    }

    public class AvailableQueriesResponse
    {
        public bool Success { get; set; }
        public int EmailSearch { get; set; }
        public int PasswordSearch { get; set; }
    }

    public class CheckLinesResponse
    {
        public bool Success { get; set; }
        public List<LineResult> Result { get; set; }
    }

    public class LineResult
    {
        public string Line { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class EmailSearchResponse
    {
        public bool Success { get; set; }
        public int AvailableQueries { get; set; }
        public int ResultCount { get; set; }
        public List<string> Results { get; set; }
    }
}
