namespace edusync.Models
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public Dictionary<string, string> Containers { get; set; }
    }
}
