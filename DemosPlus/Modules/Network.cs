using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
    public class Network
    {
        public string GetResult(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = WebRequest.CreateHttp(uri);

            var respon = request.GetResponse();
            var stream = respon.GetResponseStream();

            StreamReader reader = new StreamReader(stream);
            var result = reader.ReadToEnd();

            return result;
        }

    }

    public class QNetwork : Query<Network> { }
}
