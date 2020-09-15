using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using Bogus;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
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

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public async Task InitializeShouldReturnPosts(int posts)
        {
            var listPosts = new AutoFaker<Post>().Generate(posts);
            var listCovertedPost = new AutoFaker<Model.Post>().Generate(posts);

            A.CallTo(() => wpService.GetAll()).Returns(listPosts);
            A.CallTo(() => postService.Convert(listPosts)).Returns(listCovertedPost);

            var viewModel = new MainViewModel(postService, wpService);
            await viewModel.InitializeAsync();

            viewModel.Items.Should().BeEquivalentTo(listCovertedPost);
            viewModel.Items.Count.Should().Be(posts);
        }

        [Fact]
        public async Task InitializeItemsShouldBeEmpty()
        {
            var invalidListPosts = new AutoFaker<Post>().Generate(1);

            A.CallTo(() => wpService.GetAll()).Returns(invalidListPosts);
            A.CallTo(() => postService.Convert(invalidListPosts)).Returns(new List<Model.Post>());
            
            var viewModel = new MainViewModel(postService, wpService);
            await viewModel.InitializeAsync();

            viewModel.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task InitializeShouldNotCallServices()
        {
            var viewModel = new MainViewModel(postService, wpService);
            viewModel.Items.Add(new Faker<Model.Post>().Generate());

            A.CallTo(() => wpService.GetAll()).MustNotHaveHappened();
            A.CallTo(() => postService.Convert(A.CollectionOfDummy<Post>(1))).MustNotHaveHappened();
        }
    }
}
