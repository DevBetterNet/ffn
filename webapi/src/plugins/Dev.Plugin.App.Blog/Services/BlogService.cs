using Dev.Data;
using Dev.Plugin.App.Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dev.Plugin.App.Blog.Services
{
    public interface IBlogService
    {
        Task<List<Post>> GetAllPostAsync();
        Task CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
    }
    public class BlogService : IBlogService
    {

        #region Fields
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<PostTag> _postTagRepository;

        #endregion

        #region Ctor
        public BlogService(IRepository<Post> postRepository,
                           IRepository<PostTag> postTagRepository)
        {
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
        }
        #endregion
        public async Task<List<Post>> GetAllPostAsync()
        {
            var posts = await _postRepository.GetAllAsync(query => query.OrderByDescending(o => o.CreatedOnUtc), getCacheKey: default);

            return posts.ToList();
        }

        public async Task CreatePostAsync(Post post)
        {
            await _postRepository.InsertAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.UpdateAsync(post);
        }        
    }
}
