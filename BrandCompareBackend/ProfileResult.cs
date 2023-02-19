namespace BrandCompareBackend
{
    public class ProfileResult
    {
        public int id { get; set; }
        public object error { get; set; }
        public Dictionary<string, Dictionary<string, ProfileInfo>> resp { get; set; }
    }

    public class ProfileInfo
    {
        public int engagement { get; set; }
        public int followers { get; set; }
    }
}

