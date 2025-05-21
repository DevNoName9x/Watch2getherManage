using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Watch2getherManage.Pages.w2g
{
    public class DetailsModel : PageModel
    {
        [BindProperty]
        public required string StreamKey { get; set; } 
        public void OnGet(string id)
        {
            StreamKey = id;
        }
    }
}
