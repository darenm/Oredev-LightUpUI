using CreatorsNews.Models;

namespace CreatorsNews.ViewModels
{
    public class ArticlePageViewModel : ViewModelBase
    {
        private Article _article;

        public Article Article
        {
            get => _article;
            set
            {
                _article = value;
                RaisePropertyNameChanged();
            }
        }

        public void LoadArticle(int index)
        {
            Article = Article.GenerateArticles()[index];
        }
    }
}