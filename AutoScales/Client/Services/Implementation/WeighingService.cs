using AutoScales.Client.Constants;
using AutoScales.Client.Features;
using AutoScales.Client.Services.Interfaces;
using AutoScales.Shared.Dtos;
using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace AutoScales.Client.Services.Implementation
{
    public class WeighingService : IWeighingService
    {
        private readonly HttpClient _httpClient;
        public WeighingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TransportView> GetRandomTransportAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<TransportView>(new Uri(ClientConstants.WeighingUrl + "random"));
            if (response == null)
                return new TransportView();
            return response;
        }

        public async Task<bool> SaveWeighingAsync(JournalView journalView)
        {
            var response = await _httpClient.PostAsJsonAsync<JournalView>(new Uri(ClientConstants.WeighingUrl + "save"), journalView);
            if(response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<PagingResponse<ForDayModel>> GetPagedForDayAsync(PageParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString()
            };
            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(ClientConstants.WeighingUrl + "forday", queryStringParam));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pagingResponse = new PagingResponse<ForDayModel>
                {
                    Items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ForDayModel>>(content),
                    MetaData = Newtonsoft.Json.JsonConvert.DeserializeObject<Metadata>(response.Headers.GetValues("X-Pagination").First())
                };
                return pagingResponse;
            }
            return new PagingResponse<ForDayModel>();
        }

        public async Task<PagingResponse<TransportDto>> GetTransportsForAdminAsync(PageParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString()
            };
            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(ClientConstants.WeighingUrl + "admin-journal", queryStringParam));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pagingResponse = new PagingResponse<TransportDto>
                {
                    Items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TransportDto>>(content),
                    MetaData = Newtonsoft.Json.JsonConvert.DeserializeObject<Metadata>(response.Headers.GetValues("X-Pagination").First())
                };
                return pagingResponse;
            }
            return new PagingResponse<TransportDto>();
        }

        public async Task<bool> CreateTransportAsync(TransportDto transportDto)
        {
            var response = await _httpClient.PostAsJsonAsync<TransportDto>(new Uri(ClientConstants.WeighingUrl + "create"), transportDto);
            if(response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateTransportAsync(TransportDto transportDto)
        {
            var response = await _httpClient.PutAsJsonAsync<TransportDto>(new Uri(ClientConstants.WeighingUrl + "update"), transportDto);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> DeleteSoftAsync(TransportDto transportDto)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(ClientConstants.WeighingUrl),
                Method = HttpMethod.Delete,
                Content = JsonContent.Create(transportDto)
            };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
