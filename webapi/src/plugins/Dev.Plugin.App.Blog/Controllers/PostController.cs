using Dev.Core.Filter;
using Dev.Plugin.App.Blog.Domain;
using Dev.Plugin.App.Blog.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dev.Plugin.App.Blog.Controllers;

public class PostController : PublicController
{

    #region Fields
    private readonly IBlogService _blogService;
    #endregion

    #region Ctor
    public PostController(IBlogService blogService)
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

    [HttpPost]
    [JwtAuthorize]
    [Route("CreatePost")]
    public async Task<IActionResult> CreatePostAsync(Post post)
    {
        await _blogService.CreatePostAsync(post);
        return Ok();
    }

    [HttpPut]
    [Route("UpdatePost")]
    public async Task<IActionResult> UpdatePostAsync(Post post)
    {
        await _blogService.UpdatePostAsync(post);
        return Ok();
    }
}
