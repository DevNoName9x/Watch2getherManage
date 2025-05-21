using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http;
using System.Text;
using Watch2getherManage.Models;
using Watch2getherManage.Services;

namespace Watch2getherManage.Pages.w2g
{
    public class IndexModel(StreamKeyService service, IHttpClientFactory httpClientFactory, IConfiguration config) : PageModel
    {
        [BindProperty]
        public string? ResponseMessage { get; set; }
        [BindProperty]
        public bool IsError { get; set; }
        [BindProperty]
        public List<string> ListStreamKeys { get; set; } = service.GetAllKeys().Select(x => x.StreamKey).ToList();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostCreateAsync(string videoUrl, string bgColor, double bgOpacity = 0)
        {
            string apiKey = config["ApiKey"] ?? "";
            if (string.IsNullOrEmpty(apiKey))
            {
                IsError = true;
                ResponseMessage = "Vui lòng nhập API Key!";
                return Page();
            }
            var payload = new
            {
                // Các trường chính bạn yêu cầu
                w2g_api_key = apiKey,
                share = string.IsNullOrEmpty(videoUrl) ? null : videoUrl,
                bg_color = string.IsNullOrEmpty(bgColor) ? "#22C1C3" : bgColor,
                bg_opacity = bgOpacity,
            };
            try
            {
                var client = httpClientFactory.CreateClient();
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.w2g.tv/rooms/create.json", content);
                if (!response.IsSuccessStatusCode)
                {
                    IsError = true;
                    ResponseMessage = $"Lỗi: {(int)response.StatusCode} - {response.ReasonPhrase}";
                    return Page();
                }
                var responseString = await response.Content.ReadAsStringAsync();
                RoomInfo? roomInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomInfo>(responseString);
                if(roomInfo == null)
                {
                    IsError = true;
                    ResponseMessage = "Lỗi: Không thể tạo phòng.";
                    return Page();
                }

                
                service.Insert(roomInfo.Streamkey?? "");
                IsError = false;
                ResponseMessage = $@"Phòng đã được tạo!";
            }
            catch (Exception ex)
            {
                IsError = true;
                ResponseMessage = $"Lỗi: {ex.Message}. Vui lòng kiểm tra API Key hoặc kết nối mạng.";
            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAddExistingRoomAsync(string StreamKey)
        {
            if (string.IsNullOrEmpty(StreamKey))
            {
                IsError = true;
                ResponseMessage = "Vui lòng nhập Stream Key!";
                return Page();
            }
            await service.InsertAsync(StreamKey);

            return RedirectToPage("Index");
        }
    }
}
