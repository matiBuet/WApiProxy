using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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


            string url = "http://192.168.10.60:81/" ;

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
            var TipoMetodo = HttpContext.Current.Request;          

            string url = "http://192.168.10.60:81/" +api.api ;

            using (HttpClient http = new HttpClient())
            {
  
                var data = new StringContent(api.json.ToString(), Encoding.UTF8, "application/json");
                var response = await http.PostAsync(url, data);

                return response;

                //this.Request.RequestUri = new Uri(url);

                //return await http.SendAsync(this.Request);
            }
        }

    

    }
}
