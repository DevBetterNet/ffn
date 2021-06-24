
using Dev.Plugin.App.Blog.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Plugin.App.Blog.Controllers
{
    public class CategoryController : PublicController
    {
        #region Fields
        private readonly IBlogService _blogService;
        #endregion

        #region Ctor
        public CategoryController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        #endregion

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetBlogPostsAsync()
        {
            var blog = await _blogService.GetAllPostAsync();
            return Ok(blog);
        }
    }
}
