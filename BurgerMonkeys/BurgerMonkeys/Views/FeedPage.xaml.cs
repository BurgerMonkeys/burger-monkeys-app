using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BurgerMonkeys.Abstractions;
using BurgerMonkeys.Services;
using BurgerMonkeys.ViewModels;
using Xamarin.Forms;

namespace BurgerMonkeys.Views
{
    public partial class FeedPage
    {
        public FeedPage()
        {
            InitializeComponent();
            var postService = DependencyService.Get<IPostService>();
            var wpService = DependencyService.Get<IWpService>();
            BindingContext = new FeedViewModel(postService, wpService);
        }
    }
}
