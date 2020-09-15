using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using FakeItEasy;
using FluentAssertions;
using WordPressPCL.Models;
using Xunit;

namespace BurgerMonkeys.Test.Tests
{
    public class MainViewModelTest
    {
        IWpService wpService;
        IPostService postService;

        public MainViewModelTest()
        {
            wpService = A.Fake<IWpService>();
            postService = A.Fake<IPostService>();
        }

        [Fact]
        public async Task InitializeShouldReturnAPostTest()
        {
            var listPosts = new AutoFaker<Post>().Generate(1);
            var listCovertedPost = new AutoFaker<Model.Post>().Generate(1);

            A.CallTo(() => wpService.GetAll()).Returns(listPosts);
            A.CallTo(() => postService.Convert(listPosts)).Returns(listCovertedPost);

            var viewModel = new MainViewModel(postService, wpService);
            await viewModel.InitializeAsync();

            viewModel.Items.Should().BeEquivalentTo(listCovertedPost);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public async Task InitializeShouldReturnPosts(int posts)
        {
            var listPosts = new AutoFaker<Post>().Generate(posts);
            var listCovertedPost = new AutoFaker<Model.Post>().Generate(posts);

            A.CallTo(() => wpService.GetAll()).Returns(listPosts);
            A.CallTo(() => postService.Convert(listPosts)).Returns(listCovertedPost);

            var viewModel = new MainViewModel(postService, wpService);
            await viewModel.InitializeAsync();

            viewModel.Items.Should().BeEquivalentTo(listCovertedPost);
            viewModel.Items.Count().Should().Be(20);
        }
    }
}
