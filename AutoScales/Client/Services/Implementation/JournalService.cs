using AutoScales.Client.Constants;
using AutoScales.Client.Features;
using AutoScales.Client.Services.Interfaces;
using AutoScales.Shared.Dtos;
using AutoScales.Shared.Models;
using AutoScales.Shared.Models.PageModels;
using AutoScales.Shared.Views;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AutoScales.Client.Services.Implementation
{
    public class JournalService : IJournalService
    {
        private readonly HttpClient _httpClient;
        public JournalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagingResponse<JournalView>> GetPagedJournalAsync(PageParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString()
            };
            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(ClientConstants.JournalUrl, queryStringParam));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pagingResponse = new PagingResponse<JournalView>
                {
                    Items = JsonConvert.DeserializeObject<List<JournalView>>(content),
                    MetaData = JsonConvert.DeserializeObject<Metadata>(response.Headers.GetValues("X-Pagination").First())
                };
                return pagingResponse;
            }
            return new PagingResponse<JournalView>();
        }

        public async Task<PagingResponse<JournalView>> GetPagedJournalBySearchAsync(SearchModel searchModel, PageParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString()
            };
            var requestUri = QueryHelpers.AddQueryString(ClientConstants.JournalUrl, queryStringParam);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(requestUri),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(searchModel)
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var pagingResponse = new PagingResponse<JournalView>();
            if(response.IsSuccessStatusCode && content != null)
            {
                pagingResponse = new PagingResponse<JournalView>
                {
                    Items = JsonConvert.DeserializeObject<List<JournalView>>(content),
                    MetaData = JsonConvert.DeserializeObject<Metadata>(response.Headers.GetValues("X-Pagination").First())
                };
            }
            return pagingResponse;
        }
    }
}
