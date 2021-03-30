using Newtonsoft.Json;
using SmartAccess.Droid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAccess.Droid.Services
{
    public class ApiService
    {
        private string _AccesToken;
        #region Usuario

        public async Task<Response<User>> GetUsuarioByEmailAsync(
         string urlBase,
         string servicePrefix,
         string controller,
         string email,
         string password)
        {
            try
            {

                var request = new { UserName = email, Password = password };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                //var client = new HttpClient
                //{
                //    BaseAddress = new Uri(urlBase)
                //};



                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<User>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var usuario = JsonConvert.DeserializeObject<User>(result);
                return new Response<User>
                {
                    IsSuccess = true,
                    Result = usuario
                };
            }
            catch (Exception ex)
            {
                return new Response<User>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<object>> OlvidePassword(
        string urlBase,
        string servicePrefix,
        string controller,
        object request
        )
        {
            try
            {

                // var request = new LoginRequest { Nombre1 = nuevapass., Contrasena = password };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                //var client = new HttpClient
                //{
                //    BaseAddress = new Uri(urlBase)
                //};



                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var usuario = JsonConvert.DeserializeObject<Object>(result);
                return new Response<Object>
                {
                    IsSuccess = true,
                    Result = usuario
                };
            }
            catch (Exception ex)
            {
                return new Response<Object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<object>> SolicitudRegistro(string urlBase, string servicePrefix, string controller, User userRegistro)
        {
            try
            {
                var requestString = JsonConvert.SerializeObject(userRegistro);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var x = JsonConvert.DeserializeObject<object>(result);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = x
                };
            }
            catch (Exception ex)
            {

                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<Response<List<Chat>>> GetChats(
         string urlBase,
         string servicePrefix,
         string controller)
        {
            try
            {

                //var request = new IdRequest { IdUsuario = id };
                //var requestString = JsonConvert.SerializeObject(request);
                //var content = new StringContent("", Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };



                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<List<Chat>>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var chats = JsonConvert.DeserializeObject<List<Chat>>(result);
                return new Response<List<Chat>>
                {
                    IsSuccess = true,
                    Result = chats
                };
            }
            catch (Exception ex)
            {
                return new Response<List<Chat>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<List<OpenRequest>>> GetOpenRequest(
        string urlBase,
        string servicePrefix,
        string controller)
        {
            try
            {

                //var request = new IdRequest { IdUsuario = id };
                //var requestString = JsonConvert.SerializeObject(request);
                //var content = new StringContent("", Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };



                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<List<OpenRequest>>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<OpenRequest>>(result);
                return new Response<List<OpenRequest>>
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response<List<OpenRequest>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<Chat>> NewMessage(
        string urlBase,
        string servicePrefix,
        string controller,
        object request)
        {
            try
            {

                //var request = new IdRequest { IdUsuario = id };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                //var client = new HttpClient
                //{
                //    BaseAddress = new Uri(urlBase)
                //};



                var url = $"{urlBase}{servicePrefix}{controller}?mobile=true";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<Chat>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var chat = JsonConvert.DeserializeObject<Chat>(result);
                return new Response<Chat>
                {
                    IsSuccess = true,
                    Result = chat
                };
            }
            catch (Exception ex)
            {
                return new Response<Chat>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<Response<SesionLock>> GetSesionByEmailAsync(
        string urlBase,
        string controller)
        {
            try
            {

                var request = new
                {
                    installId = "79fd0eb6-381d-4adf-95a0-47721289d1d9",
                    password = "Da310894",
                    identifier = "email:tejadadiiana@gmail.com"
                };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Add("x-kease-api-key", "79fd0eb6-381d-4adf-95a0-47721289d1d9");


                var url = $"{urlBase}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<SesionLock>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var sesion = JsonConvert.DeserializeObject<SesionLock>(result);



                IEnumerable<string> values;
                var token = string.Empty;
                if (response.Headers.TryGetValues("x-august-access-token", out values))
                {
                    token = values.FirstOrDefault();
                    Preferences.Set("AccesToken", token);
                }
                return new Response<SesionLock>
                {
                    IsSuccess = true,
                    Result = sesion
                };
            }
            catch (Exception ex)
            {
                return new Response<SesionLock>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<Response<StatusLock>> GetStatusLock(
        string urlBase,
        string servicePrefix,
        string controller)
        {
            try
            {
                _AccesToken = "";
                _AccesToken = Preferences.Get("AccesToken", "");


                var content = new StringContent("", Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Add("x-kease-api-key", "79fd0eb6-381d-4adf-95a0-47721289d1d9");
                client.DefaultRequestHeaders.Add("x-august-access-token", _AccesToken);


                var url = $"{urlBase}{servicePrefix}{controller}";
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();              

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<StatusLock>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }


                var status = JsonConvert.DeserializeObject<StatusLock>(result);
                return new Response<StatusLock>
                {
                    IsSuccess = true,
                    Result = status
                };
            }
            catch (Exception ex)
            {
                return new Response<StatusLock>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<OpenRequest>> SetStatus(
       string urlBase,
       string servicePrefix,
       string controller,
       int id,
       OpenRequest value)
        {
            try
            {

                var requestString = JsonConvert.SerializeObject(value);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                var client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(urlBase)
                };                


                var url = $"{urlBase}{servicePrefix}{controller}{id}";
                var response = await client.PutAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<OpenRequest>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }


                var status = JsonConvert.DeserializeObject<OpenRequest>(result);
                return new Response<OpenRequest>
                {
                    IsSuccess = true,
                    Result = status
                };
            }
            catch (Exception ex)
            {
                return new Response<OpenRequest>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

    }
}
