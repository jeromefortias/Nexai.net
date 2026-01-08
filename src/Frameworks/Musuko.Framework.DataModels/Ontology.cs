// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Musuko.Framework.DataModels
{


    public class Ontology
    {
        public string id { get; set; }
        public string dateCreated { get; set; }
        public string username { get; set; }
        public string machinename { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string nodeparentId { get; set; }
        public List<string> synonyms { get; set; }
        
    }
}
