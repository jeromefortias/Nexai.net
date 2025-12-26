namespace Musuko.Kaonashi
{
    public class SymbolicProcessor : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public List<string> InboundShould { get; set; } = new List<string>();
        public List<string> InboundShouldNot { get; set; } = new List<string>();
        public List<string> InboundMust { get; set; } = new List<string>();
        public List<string> InboundMustNot { get; set; } = new List<string>();
        public List<string> OutbounShould { get; set; } = new List<string>();
        public List<string> OutboundShouldNot { get; set; } = new List<string>();
        public List<string> OutboundMust { get; set; } = new List<string>();
        public List<string> OutboundMustNot { get; set; } = new List<string>();
        public string SystemPrompt { get; set; } = string.Empty;
    }
}
