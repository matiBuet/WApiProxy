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
using System.Web.Http.Cors;
using static System.Net.WebRequestMethods;

namespace WApiProxy.Controllers
{
    public class ConnController : ApiController
    {

        [AcceptVerbs(Http.Get, Http.Head, Http.MkCol, Http.Post, Http.Put)]
        public async Task<HttpResponseMessage> Proxy(string metodo)
        {
            var TipoMetodo = HttpContext.Current.Request;

            var Patametros = "";
            var listaparametros = metodo.Split('~');
            for (int k = 0; k < listaparametros.Count(); k++)
            {
                if (k == 0)
                {
                    Patametros = listaparametros[k].ToString();
                }
                else
                {
                    Patametros = Patametros + "&" + listaparametros[k].ToString();
                }

            }
            string url = "http://192.168.10.60/WebApiVIP/api/VIP/";
            //string url = "http://192.168.10.60/WebApiVIPTest/api/VIP/";
            //string url = "http://localhost:5775/api/VIP/";

            using (HttpClient http = new HttpClient())
            {
                this.Request.RequestUri = new Uri(url + Patametros);

                if (this.Request.Method == HttpMethod.Get)
                {
                    this.Request.Content = null;
                }

                return await http.SendAsync(this.Request);
            }
        }

        [AcceptVerbs(Http.Get, Http.Head, Http.MkCol, Http.Post, Http.Put)]
        public async Task<HttpResponseMessage> login(usuario user)
        {

            string url = "http://192.168.10.60:81/wapiautenticacion/api/autenticacion/ValidarUsuarioExtranetNombramiento";

               var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient c = new HttpClient();

            var response = await c.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;

            var a = JsonConvert.DeserializeObject<dtoRespuestaUsuarioNombramientos>(result);
            return Request.CreateResponse(HttpStatusCode.OK, a);
        }

        [AcceptVerbs(Http.Get, Http.Head, Http.MkCol, Http.Post, Http.Put)]
        public async Task<HttpResponseMessage> ProxyGenerico(string aplicacionMetodo)
        {
            var TipoMetodo = HttpContext.Current.Request;

            var Patametros = "";
            var listaparametros = aplicacionMetodo.Split('~');
            for (int k = 0; k < listaparametros.Count(); k++)
            {
                if (k == 0)
                {
                    Patametros = listaparametros[k].ToString();
                }
                else
                {
                    Patametros = Patametros + "&" + listaparametros[k].ToString();
                }

            }
            string url = "http://192.168.10.60:81/";

            using (HttpClient http = new HttpClient())
            {
                this.Request.RequestUri = new Uri(url + Patametros);

                if (this.Request.Method == HttpMethod.Get)
                {
                    this.Request.Content = null;
                }

                return await http.SendAsync(this.Request);
            }
        }


    }

    public class usuario
    {
        public string user { get; set; }
        public string pass { get; set; }
    }

    public class dtoRespuestaUsuarioNombramientos
    {
        public int Codigo { get; set; }
        public string Usuario { get; set; }
        public string Cliente { get; set; }
        public string NOMBREAPP { get; set; }
        public string URL { get; set; }
        public string Empresa { get; set; }
        public bool Valido { get; set; }
    }
}
