using System.Net.Http.Headers;
using System.Text.Json;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.DefaultRequestHeaders.Add("User-Agent", "watmildon non english tag finder");
await ProcessTagInfoAsync(client);
        

static async Task ProcessTagInfoAsync(HttpClient client)
{
    var tagCounts = new Dictionary<string, int>();
    var lines = File.ReadAllLines(@"..\..\..\WordList.tsv");

    // Get counts for tags skipping ones already in the set (the translations for various languages may collide)
    // print non zero values
    // The indexing looks weird becuase I wanted to flip the rows and columns to translated terms
    // would stay together in the final output.
    for (int i = 0; i < 5; i++)
    {
        foreach (var line in lines)
        {
            var keys = line.Split("\t");

            if (!tagCounts.ContainsKey(keys[i]))
            {
                await using Stream stream = await client.GetStreamAsync("https://taginfo.openstreetmap.org/api/4/key/" + $"stats?key={keys[i]}");
                var tagInfo = await JsonSerializer.DeserializeAsync<TagInfoData>(stream);
                tagCounts.Add(keys[i], tagInfo.data[0].count);
                if (tagInfo.data[0].count != 0)
                {
                    Console.WriteLine($"{keys[i]},{tagInfo.data[0].count}");
                }
            }            
        }
    }
}

