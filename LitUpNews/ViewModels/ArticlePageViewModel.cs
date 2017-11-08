using CreatorsNews.Models;

namespace CreatorsNews.ViewModels
{
    public class ArticlePageViewModel : ViewModelBase
    {
        #region Fields

        private Article _article;

        #endregion

        #region Properties

        public Article Article
        {
            get => _article;
            set
            {
                _article = value;
                RaisePropertyNameChanged();
            }
        }

        #endregion

        public void LoadArticle(int index)
        {
            Article = Article.GenerateArticles()[index];
        }
    }
}