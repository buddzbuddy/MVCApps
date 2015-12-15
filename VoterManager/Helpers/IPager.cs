namespace VoterManager.Helpers
{
    /// <summary>
    /// Модель для вывода постраничной прокрутки
    /// </summary>
    public interface IPager
    {
        /// <summary>
        /// Общее количество страниц
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// Начальная страница для отображения
        /// </summary>
        int StartDisplayedPage { get; }

        /// <summary>
        /// Конечная страница для отображения
        /// </summary>
        int EndDisplayedPage { get; }

        /// <summary>
        /// Получить адрес для конкретной страницы
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <returns>Адрес страницы</returns>
        string GetLinkForPage(int pageIndex);
    }
}