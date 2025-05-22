using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using Watch2getherManage.Models;

namespace Watch2getherManage.Pages
{
    public class IndexModel(IHttpClientFactory httpClientFactory, IConfiguration config) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [BindProperty]
        public string? ResponseMessage { get; set; }

        public void OnGet()
        {
            // Không cần xử lý gì khi tải trang
        }

        public async Task<IActionResult> OnPostCreateRoomAsync(string videoUrl, string bgColor, int bgOpacity = 0)
        {
            string apiKey = config["ApiKey"] ?? "";
            if (string.IsNullOrEmpty(apiKey))
            {
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
                persistent = true,
                persistent_name = "Phòng Xịn",

                // Các trường bổ sung, nhưng dùng giá trị KHÁC với JSON gốc
                id = 999999999L,                                // khác 289618596
                streamkey = "changed_stream_key",               // khác "97rg68z1odv82zhvhs"
                //created_at = DateTime.UtcNow,                   // khác "2025-05-21T01:53:37.000Z"
                deleted = true,                                 // khác false
                moderated = true,                               // khác false
                location = "EU",                                // khác "SEA"
                stream_created = true,                          // khác false
                background = "custom_background.jpg",           // khác null
                moderated_background = true,                    // khác false
                moderated_playlist = true,                      // khác false
                moderated_item = true,                          // khác false
                theme_bg = "dark",                              // khác null
                playlist_id = 123456789L,                       // khác 282378512
                members_only = true,                            // khác false
                moderated_suggestions = true,                   // khác false
                moderated_chat = true,                          // khác false
                moderated_user = true,                          // khác false
                moderated_cam = true,                           // khác false
                moderated_videourl = true                       // khác false
            };

            try
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.w2g.tv/rooms/create.json", content);

                // Kiểm tra tiêu đề CORS (dành cho thông tin debug)
                //var corsHeader = response.Headers.GetValues("Access-Control-Allow-Origin")?.FirstOrDefault() ?? "Không có";

                if (!response.IsSuccessStatusCode)
                {
                    ResponseMessage = $"Lỗi: {(int)response.StatusCode} - {response.ReasonPhrase}";
                    return Page();
                }

                //var responseData = await response.Content.ReadFromJsonAsync<RoomInfo>();
                var responseString = await response.Content.ReadAsStringAsync();
                RoomInfo? roomInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomInfo>(responseString);
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(roomInfo, Newtonsoft.Json.Formatting.Indented);
                ResponseMessage = $@"Phòng đã được tạo!<br/><pre> {jsonString}</pre>";

                //$@"Stream Key: {responseData.Streamkey}<br>
                //    Link phòng: <a href='https://w2g.tv/rooms/{responseData.Streamkey}' target='_blank'>https://w2g.tv/rooms/{responseData.Streamkey}</a><br>
                //    ID: {responseData.Id}<br>
                //    Created At: {responseData.CreatedAt}<br>
                //    Persistent: {responseData.Persistent}<br>";
            }
            catch (Exception ex)
            {
                ResponseMessage = $"Lỗi: {ex.Message}. Vui lòng kiểm tra API Key hoặc kết nối mạng.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateRoomAsync(string streamKey, string updateVideoUrl)
        {
            string apiKey = config["ApiKey"] ?? "";
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(streamKey) || string.IsNullOrEmpty(updateVideoUrl))
            {
                ResponseMessage = "Vui lòng nhập API Key, Stream Key và URL video!";
                return Page();
            }
            return Page();
        }
        // Thêm video vào playlist
        public async Task<IActionResult> OnPostAddToPlaylistAsync(string streamKey, string playlistVideoUrl, string videoTitle)
        {
            string apiKey = config["ApiKey"] ?? "";
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(streamKey) || string.IsNullOrEmpty(playlistVideoUrl))
            {
                ResponseMessage = "Vui lòng nhập API Key, Stream Key và URL video!";
                return Page();
            }
            return Page();
        }
    }
}
