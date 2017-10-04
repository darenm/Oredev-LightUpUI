using CreatorsNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorsNews.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private static Article[] _articles = Article.GenerateArticles();

        public Article[] Articles => _articles;
    }
}
