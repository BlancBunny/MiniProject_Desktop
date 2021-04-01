using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleNaverMovieFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientID = "K3syzZS40w59ePgVVyE1";
            string clientSecret = "9tbm63LFyX";
            string search = "starwars"; // 검색 키워드 
            string openApiUrl = $"https://openapi.naver.com/v1/search/movie?query={search}";

            var responseJson = GetApiResult(openApiUrl, clientID, clientSecret);

            //Console.WriteLine(responseJson);

            JObject parsedJson = JObject.Parse(responseJson);

            int total = Convert.ToInt32(parsedJson["total"]);
            Console.WriteLine($"총 검색결과 : {total}건");
            int display = Convert.ToInt32(parsedJson["display"]);
            Console.WriteLine($"화면 출력 : {display}건");

            var items = parsedJson["items"];
            JArray jArray = (JArray)items;

            foreach (var item in jArray)
            {
                Console.WriteLine($"{item["title"]} / {item["image"]} / {item["subtitle"]} / {item["actor"]}");
            }
            

        }

        private static string GetApiResult(string openApiUrl, string clientID, string clientSecret)
        {
            var result = "";
            try
            {
                WebRequest request = WebRequest.Create(openApiUrl);
                request.Headers.Add("X-Naver-Client-Id", clientID);
                request.Headers.Add("X-Naver-Client-Secret", clientSecret);

                WebResponse response = request.GetResponse();
                
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생 : {ex.Message}");
            }
            return result;
        }
    }
}
