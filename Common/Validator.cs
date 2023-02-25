using System.Net;

namespace BlazorCommon
{
    internal class Validator
    {
        public Validator() { }


        /// <summary>
        /// Checks if the url exsists.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True : If the url exits</returns>
        internal static bool RemoteFileExists(string url)
        {
            try
            {               
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;                
                request.Method = "HEAD";                
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;                              
                bool result = (response.StatusCode == HttpStatusCode.OK);
                response.Close();
                return result;
            }
            catch
            {                
                return false;
            }
        }
    }
}
