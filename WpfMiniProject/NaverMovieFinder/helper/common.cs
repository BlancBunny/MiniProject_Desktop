using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NaverMovieFinder.helper
{
    class common
    {
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();


        public static string GetApiResult(string openApiUrl, string clientID, string clientSecret)
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
                LOGGER.Error($"예외 발생 : {ex}");
            }
            return result;
        }
    }
}


