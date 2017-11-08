using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace CreatorsNews.Models
{
    public class MenuItem
    {
        #region Properties

        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }

        #endregion

        public static List<MenuItem> GetMainItems()
        {
            var items = new List<MenuItem>();
            items.Add(new MenuItem {Icon = Symbol.Home, Name = "Home", PageType = typeof(Views.HomePage)});
            items.Add(new MenuItem {Icon = Symbol.XboxOneConsole, Name = "Games", PageType = typeof(Views.ArticlesPage)});
            items.Add(new MenuItem {Icon = Symbol.Document, Name = "News", PageType = typeof(Views.NewsPage)});
            return items;
        }

        public static List<MenuItem> GetOptionsItems()
        {
            var items = new List<MenuItem>();
            items.Add(new MenuItem {Icon = Symbol.Setting, Name = "Settings", PageType = typeof(Views.SettingsPage)});
            return items;
        }
    }
}