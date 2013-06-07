// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年03月01日 11:19
//  
//  修改于：2013年05月13日 18:20
//  文件名：FrameworkOnly/Dev.Comm.Web.Mvc/Pagination.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Comm.Web.Mvc.Pager
{
    using System;
    using System.Web.Routing;

    /// <summary>
    /// </summary>
    public class Pagination
    {
        //public Pagination(int currentPage, int totalPages, string action, string controller, object routeValues = null, string pageQueryString = "page")
        //{
        //    RouteValueDictionary routeDictionary = new RouteValueDictionary(routeValues);
        //    routeDictionary.Add("controller", controller);
        //    routeDictionary.Add("action", action);
        //    this.Init(currentPage, totalPages, routeDictionary, pageQueryString);
        //}

        #region Constructors and Destructors

        public Pagination(
            int currentPage,
            int count,
            int pageSize,
            string action,
            string controller,
            object routeValues = null,
            string pageQueryString = "page")
            : this(
                currentPage, count, pageSize, action, controller, new RouteValueDictionary(routeValues), pageQueryString
                )
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="currentPage"> </param>
        /// <param name="count"> </param>
        /// <param name="pageSize"> </param>
        /// <param name="action"> </param>
        /// <param name="controller"> </param>
        /// <param name="routeDictionary"> </param>
        /// <param name="pageQueryString"> </param>
        public Pagination(
            int currentPage,
            int count,
            int pageSize,
            string action,
            string controller,
            RouteValueDictionary routeDictionary = null,
            string pageQueryString = "page")
        {
            routeDictionary = routeDictionary ?? new RouteValueDictionary();

            routeDictionary.Add("controller", controller);
            routeDictionary.Add("action", action);
            var totalPages = (int) Math.Ceiling((decimal) count/pageSize);
            this.PageSize = pageSize;
            this.Init(currentPage, totalPages, routeDictionary, pageQueryString);
        }

        public Pagination(int currentPage, int count, int pageSize, object routeValues, string pageQueryString = "page")
        {
            var routeDictionary = new RouteValueDictionary(routeValues);
            this.PageSize = pageSize;
            var totalPages = (int) Math.Ceiling((decimal) count/pageSize);
            this.Init(currentPage, totalPages, routeDictionary, pageQueryString);
        }

        /// <summary>
        ///   AJAX方法调用的构造函数
        /// </summary>
        /// <param name="currentPage"> </param>
        /// <param name="count"> </param>
        /// <param name="pageSize"> </param>
        /// <param name="JavascriptFun"> </param>
        public Pagination(int currentPage, int count, int pageSize, string JavascriptFun)
            : this(currentPage, count, pageSize, "", "", null, "page")
        {
            this.JavascriptFun = JavascriptFun;
        }

        #endregion

        #region Public Properties

        public string Action { get; private set; }

        public string Controller { get; private set; }

        /// <summary>
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        ///   Javascript 方法体 ， 例如 ： @"ajaxpage({0})"
        ///   <![CDATA[
        ///  function ajaxpage(pageNo){
        ///      pageSize, 条件 等从页面中取得或写成固定值。。。
        ///      //  调用 Ajax 的方法 从服务器读取数据
        ///  }
        /// ]]>
        /// </summary>
        public string JavascriptFun { get; set; }

        public string PageQueryString { get; private set; }

        public int PageSize { get; set; }

        public RouteValueDictionary RouteValues { get; private set; }

        public bool Shorten { get; private set; }

        public int Start { get; private set; }

        public int Stop { get; private set; }

        public int TotalPages { get; private set; }

        #endregion

        #region Public Methods and Operators

        public RouteValueDictionary GetDictionary(int pageNumber)
        {
            this.RouteValues[this.PageQueryString] = pageNumber;
            return this.RouteValues;
        }

        #endregion

        #region Methods

        private void Init(int currentPage, int totalPages, RouteValueDictionary routeValues, string pageQueryString)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.Controller = (string) routeValues["controller"];
            this.Action = (string) routeValues["action"];
            this.PageQueryString = pageQueryString;
            this.Shorten = this.TotalPages >= 10;

            this.Start = 2;
            this.Stop = this.TotalPages - 1;

            if (this.Shorten)
            {
                if (this.CurrentPage - 3 > 0)
                {
                    this.Start = this.CurrentPage - 2;
                }
                if (this.Start == 3 && this.CurrentPage == 5)
                {
                    this.Start--;
                }

                if (this.TotalPages - this.CurrentPage > 4)
                {
                    this.Stop = this.CurrentPage + 2;
                }

                if (this.Start == 1)
                {
                    this.Start++;
                }
                if (this.Stop == this.TotalPages || this.Stop > this.TotalPages)
                {
                    this.Stop = this.TotalPages - 1;
                }
            }

            routeValues.Add(pageQueryString, 0);
            routeValues.Add("pagesize", this.PageSize);
            this.RouteValues = routeValues;
        }

        #endregion
    }
}