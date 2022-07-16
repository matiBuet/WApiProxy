using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static System.Net.WebRequestMethods;

namespace WApiProxyPortalClientesVehiculos.Controllers
{

    public class apiJson
    {
        public string api { get; set; }
        public object json { get; set; }
    }

    public class ConexionController : ApiController
    {
        [AcceptVerbs(Http.Get)]
        public async Task<HttpResponseMessage> ProxyGet(string api)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var TipoMetodo = HttpContext.Current.Request;

            var misParametros = "";
            if (api != null && api != "")
            {
                var listaparametros = api.Split('~');
                for (int i = 0; i < listaparametros.Count(); i++)
                {
                    if (i == 0)
                        misParametros = listaparametros[i].ToString();
                    else
                        misParametros = misParametros + "&" + listaparametros[i].ToString();
                }
            }
            // validate cert by calling a function
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);


            string url = "https://192.168.10.60:444/" ;

            using (HttpClient http = new HttpClient())
            {
                this.Request.RequestUri = new Uri(url + misParametros);
                if (this.Request.Method == HttpMethod.Get)
                    this.Request.Content = null;

                return await http.SendAsync(this.Request);
            }
        }

        [AcceptVerbs(Http.Post)]
        public async Task<HttpResponseMessage> ProxyPost(apiJson api)
        {
            try
            {
                ////Trust all certificates
                //System.Net.ServicePointManager.ServerCertificateValidationCallback =
                //((sender, certificate, chain, sslPolicyErrors) => true);

                //// trust sender
                //System.Net.ServicePointManager.ServerCertificateValidationCallback
                //                = ((sender, cert, chain, errors) => cert.Subject.Contains("YourServerName"));

                // validate cert by calling a function
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

                var TipoMetodo = HttpContext.Current.Request;
                string url = "https://192.168.10.60:444/" + api.api;
                using (HttpClient http = new HttpClient())
                {
                    var data = new StringContent(api.json.ToString(), Encoding.UTF8, "application/json");
                    var response = await http.PostAsync(url, data);
                    return response;
                }
            }
            catch (Exception e)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                return  response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message + " - " + e.InnerException);
            }
        }

        [AcceptVerbs(Http.Get)]
        public HttpResponseMessage ProxyPrueba()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Prueba de conexion");
        }
               
  

        // callback used to validate the certificate in an SSL conversation
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            bool result = cert.Subject.Contains("192.168.10.60");
            return result;
        }
    }
}
