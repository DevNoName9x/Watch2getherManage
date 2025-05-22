using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Watch2getherManage.Pages.w2g
{
    public class DetailsModel(IHttpClientFactory httpClientFactory, IConfiguration config) : PageModel
    {
        [BindProperty]
        public required string StreamKey { get; set; }
        [BindProperty]
        public string? Message { get; set; }
        // Thuộc tính để bind dữ liệu từ form
        [BindProperty]
        public string? VideoUrl { get; set; }

        [BindProperty]
        public string? Title { get; set; }

        private readonly string? ApiKey = config["ApiKey"];
        public void OnGet(string id)
        {
            StreamKey = id;
        }
        public async Task<IActionResult> OnPost(string handler, string id)
        {
            StreamKey = id;
            // Kiểm tra handler để xử lý hành động
            if (handler == "PlayNow")
            {
                if (string.IsNullOrEmpty(VideoUrl))
                {
                    Message = "Vui lòng nhập URL video!";
                    return Page();
                }
                await PlayNowAsync();
                // Logic phát video (giả lập)
                Message = $"Đang phát video: {Title ?? "Video không có tiêu đề"} ({VideoUrl})";
                // Có thể chuyển hướng đến trang phát video
                // return Redirect(VideoUrl); // Nếu muốn chuyển hướng trực tiếp đến URL
            }
            else if (handler == "AddToPlaylist")
            {
                if (string.IsNullOrEmpty(VideoUrl))
                {
                    Message = "Vui lòng nhập URL video!";
                    return Page();
                }
                await AddToPlaylistAsync();
                Message = $"Đã thêm video \"{Title}\" vào playlist!";
            }
            else if (handler == "Delete")
            {
                Delete();
            }
            return Page();
        }

        private async Task PlayNowAsync()
        {
            var payload = new
            {
                w2g_api_key = ApiKey,
                item_url = VideoUrl
            };

            try
            {
                var client = httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://api.w2g.tv/rooms/{StreamKey}/sync_update", content);
                if (!response.IsSuccessStatusCode)
                {
                    Message = $"Lỗi: {(int)response.StatusCode} - {response.ReasonPhrase}";
                    return;
                }
                Message = $"Video đã được cập nhật thành công cho phòng {StreamKey}! <br/>";
            }
            catch (Exception ex)
            {
                Message = $"Lỗi: {ex.Message}. Có thể do Stream Key sai hoặc URL video không hợp lệ.";
            }
        }
        private async Task AddToPlaylistAsync()
        {
            var payload = new
            {
                w2g_api_key = ApiKey,
                add_items = new[]
        {
                    new
                    {
                        url = VideoUrl,
                        title = string.IsNullOrEmpty(Title) ? null : Title
                    }
                }
            };
            try
            {
                var client = httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://api.w2g.tv/rooms/{StreamKey}/playlists/current/playlist_items/sync_update", content);

                if (!response.IsSuccessStatusCode)
                {
                    Message = $"Lỗi: {(int)response.StatusCode} - {response.ReasonPhrase}";
                    return;
                }
                Message = $"Video '{Title ?? VideoUrl}' đã được thêm vào danh sách phát của phòng {StreamKey}!";
            }
            catch (Exception ex)
            {
                Message = $"Lỗi: {ex.Message}. Có thể do Stream Key sai hoặc URL video không hợp lệ.";
            }
        }
        private void Delete()
        {

        }
    }
}
