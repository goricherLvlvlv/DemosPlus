using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
    public class Network : IModule
    {
        public void OnResolve()
        { 
        
        }

        public void OnDestroy()
        { 
        
        }

        public string GetResult(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = WebRequest.CreateHttp(uri);

            string result;
            try
            {
                var respon = request.GetResponse();
                var stream = respon.GetResponseStream();

                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            finally
            { 
                // do nothing
            }

            return result;
        }

    }

    public class QNetwork : Query<Network> { }
}
