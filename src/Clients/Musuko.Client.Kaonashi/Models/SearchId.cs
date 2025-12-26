namespace Musuko.Kaonashi
{
    public class SearchId
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("criteria")]
        public string Criteria { get; set; } = string.Empty;
    }
}