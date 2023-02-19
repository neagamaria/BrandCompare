namespace BrandCompareBackend
{
    public class BrandResult
    {
        public int id { get; set; }
        public object error { get; set; }
        public List<Brand> result { get; set; }

    }

    public class Brand
    {
        public string brandname { get; set; }   
        public List<Profile> profiles { get; set; }
    }

    public class Profile
    {
        public string name { get; set; }
        public DateTime profile_added { get; set; }
        public string id { get; set; }
        public string profile_type { get; set; }
    }
}
