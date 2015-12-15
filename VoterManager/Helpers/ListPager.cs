/* ASP.NET MVC PAGER
 * Create by SkyLiver http://skyliver.ru 
 */
using System;
using System.Web;

namespace VoterManager.Helpers
{
    public class ListPager : IPager
    {
        #region Constants

        private const int DefaultMaxDisplayedPages = 10;
        private const int DefaultPageSize = 30;
        private const string DefaultPageQueryParameter = "page";

        #endregion

        #region Fields

        private readonly int _itemsCount;
        private readonly CustomQueryStringBuilder _queryBuilder;
        private int _maxDisplayedPages;
        private int _pageSize;
        private string _queryParameterName;

        #endregion

        #region Properties

        /// <summary>
        /// Максимальное число отображаемых страниц
        /// </summary>
        public int MaxDisplayedPages
        {
            get { return _maxDisplayedPages == 0 ? DefaultMaxDisplayedPages : _maxDisplayedPages; }
            set { _maxDisplayedPages = value; }
        }

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                PageCount = (int) (Math.Ceiling(_itemsCount/(double) _pageSize));

                if (CurrentPage > PageCount)
                    CurrentPage = PageCount;

                StartDisplayedPage = (CurrentPage - MaxDisplayedPages/2) < 1
                                         ? 1
                                         : CurrentPage - MaxDisplayedPages/2;
                EndDisplayedPage = (CurrentPage + MaxDisplayedPages/2) > PageCount
                                       ? PageCount
                                       : CurrentPage + MaxDisplayedPages/2;
            }
        }

        /// <summary>
        /// Параметр в запросе который содержит в себе значение текущей страницы
        /// </summary>
        public string QueryParameterName
        {
            get { return string.IsNullOrEmpty(_queryParameterName) ? DefaultPageQueryParameter : _queryParameterName; }
            set { _queryParameterName = value; }
        }

        #endregion

        #region Constructors

        public ListPager(int itemsCount)
            : this(itemsCount, DefaultPageSize)
        {
        }

        public ListPager(int itemsCount, int pageSize)
            : this(itemsCount, pageSize, DefaultMaxDisplayedPages)
        {
        }

        public ListPager(int itemsCount, int pageSize, int maxDisplayedPages)
            : this(itemsCount, pageSize, maxDisplayedPages, DefaultPageQueryParameter)
        {
        }


        public ListPager(int itemsCount, int pageSize, int maxDisplayedPages, string queryParameterName)
        {
            _itemsCount = itemsCount;

            QueryParameterName = queryParameterName;
            MaxDisplayedPages = maxDisplayedPages;

            if (HttpContext.Current == null)
                throw new Exception("No http context here!");
            _queryBuilder = new CustomQueryStringBuilder(HttpContext.Current.Request.QueryString, QueryParameterName);
            string currentPageString = HttpContext.Current.Request.QueryString[QueryParameterName] ?? "1";
            int page;
            if (!int.TryParse(currentPageString, out page))
                page = 1;
            CurrentPage = page;
            PageSize = pageSize;
        }

        #endregion

        #region IPager Members

        public int PageCount { get; private set; }
        public int CurrentPage { get; private set; }
        public int StartDisplayedPage { get; private set; }
        public int EndDisplayedPage { get; private set; }

        public string GetLinkForPage(int pageIndex)
        {
            return _queryBuilder.GetQueryStringForParameter(pageIndex.ToString());
        }

        #endregion
    }
}