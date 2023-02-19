using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BrandCompareBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandsController : ControllerBase
    {
        private HttpClient client;

        public BrandsController() : base()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "API_KEY_TEST");
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.BaseAddress = new Uri("https://app.socialinsider.io/api");

        }

        [HttpGet]
        public IEnumerable<BrandData> GetBrandData(string date) 
        {
            //transform string to json (param. api)
            string param = "{\"jsonrpc\": \"2.0\",\"id\": 0,\"method\":\"socialinsider_api.get_brands\",\"params\": {\"projectname\": \"API_test\"}}";
            var contentData = new StringContent(param, Encoding.UTF8, "application/json");

            //call api
            var response = client.PostAsync("https://app.socialinsider.io/api", contentData).Result;

            try
            {
                var result = response.Content.ReadAsStringAsync().Result;
                //deserialize result
                var brandResult = JsonSerializer.Deserialize<BrandResult>(result);
                if(brandResult.error == null)
                {
                    List<BrandData> list = new List<BrandData>();
                    if(brandResult.result != null)
                    {
                        foreach(var b in brandResult.result)
                        {

                            var brandData = GetInfo(date, b);
                            list.Add(brandData);
                        }
                    }
                    return list;
                }
                else
                {
                    throw new Exception("Error api");
                }
            }

            catch(Exception e)
            {
                throw e;
            }
        }

        private BrandData GetInfo(string date, Brand b)
        {
            BrandData brandData = new BrandData();
            brandData.BrandName = b.brandname;

            
            int totalProfiles = 0, totalFans = 0, totalEngagement = 0;

            var d = (DateTime.Parse(date + " 12:00:00"));
            //transform date to unix milliseconds
            long start = (long)(d.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            try
            {
                foreach(var p in b.profiles)
                {
                   
                    if(p.profile_added.Date <= DateTime.Parse(date))
                    {
                        totalProfiles += 1;

                        string param = "{\"id\" : \"1\"," + 
                            "\"method\" : \"socialinsider_api.get_profile_data\"," + 
                            "\"params\":{\"id\":\"" + p.id + "\",\"profile_type\": \"" + p.profile_type+ "\"," + 
                            "\"date\": {\"start\": "+ start +",\"end\": " + start+ "," + 
                            "\"timezone\": \"Europe/London\"}}}";
                        var contentData = new StringContent(param, Encoding.UTF8, "application/json");

                        //call api
                        var response = client.PostAsync("https://app.socialinsider.io/api", contentData).Result;

                        var result = response.Content.ReadAsStringAsync().Result;
                        //deserialize result
                        var profileResult = JsonSerializer.Deserialize<ProfileResult>(result);

                        if(profileResult.resp != null)
                        {
                            var profileInfo = profileResult.resp.First().Value.First().Value;
                            totalFans += profileInfo.followers;
                            totalEngagement += profileInfo.engagement;

                        }
                    }
                }

            }

            catch(Exception e)
            {

            }
            
            brandData.TotalProfiles = totalProfiles;
            brandData.TotalFans = totalFans;
            brandData.TotalEngagement = totalEngagement;

            return brandData;

        }
    }
}
