namespace OracleTest.Models.Response
{
    /// <summary>
    /// 傳出參數 - 分頁資料
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataRP<T>
    {
        /// <summary>
        /// 分頁資訊
        /// </summary>
        public PageInfoRP PageInfo { get; set; }

        /// <summary>
        /// 資料物件
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 傳出參數 - 分頁資訊
    /// </summary>
    public class PageInfoRP
    {
        /// <summary>
        /// 分頁頁碼
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 資料數量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分頁總數
        /// </summary>
        public int PageCnt { get; set; }

        /// <summary>
        /// 資料總數
        /// </summary>
        public int TotalCnt { get; set; }
    }
}
