using CreatorsNews.Models;

namespace CreatorsNews.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private static readonly Article[] _articles = Article.GenerateArticles();

        #region Properties

        public Article[] Articles => _articles;

        #endregion
    }
}