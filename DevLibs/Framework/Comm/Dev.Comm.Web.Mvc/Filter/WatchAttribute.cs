// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年05月13日 18:15
//  
//  修改于：2013年05月13日 18:20
//  文件名：FrameworkOnly/Dev.Comm.Web.Mvc/WatchAttribute.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Dev.Comm.Web.Mvc.Filter
{

    //ToDo:其实这里跟踪使用 StopWatch 要比DateTime好，But...

    /// <summary>
    /// 用于存储跟踪信息的树结点，
    /// 其中 child 应该用 CollectBase会好些
    /// </summary>
    internal class WatchNode
    {
        private List<WatchNode> _child = new List<WatchNode>();
        public string Name { get; set; }
        public long OnActionExecuting { get; set; }
        public long OnActionExecuted { get; set; }
        public long OnResultExecuted { get; set; }
        public long OnResultExecuting { get; set; }

        public WatchNode Parent { get; set; }
        public List<WatchNode> Child
        {
            get { return this._child; }
            set { this._child = value; }
        }


        public long All
        {
            get { return (OnResultExecuted - OnActionExecuting) / 10000; }
        }

        public long Action
        {
            get { return (this.OnActionExecuted - this.OnActionExecuting) / 10000; }
        }

        public long Result
        {
            get { return (this.OnResultExecuted - this.OnResultExecuting) / 10000; }
        }

        public bool IsChild { get; set; }

        public string ParentName
        {
            get
            {
                if (Parent == null) return null;
                return Parent.Name;
            }
        }
    }

    /// <summary>
    /// 运行时数据
    /// </summary>
    public class NameTime
    {
        public string Name { get; set; }
        public long Time { get; set; }
        public string Do { get; set; }

        public ViewContext Parent
        {
            get;
            set;
        }

        public string ParentName
        {
            get
            {
                if (Parent == null)
                    return null;

                return Parent.RouteData.Values["action"].ToString();
            }
        }
    }
    /// <summary>
    /// 跟踪MVC Action Result 运行所用时间
    /// </summary>
    public class TraceRunAttribute : ActionFilterAttribute
    {
        private string _newLine = "<br/>\r\n";
        private const string __List__ = "______List__________";

        public TraceRunAttribute()
        {
        }

        public bool IsShow
        {
            get;
            set;
        }

        /// <summary>
        /// 新行
        /// </summary>

        public string NewLine
        {
            get { return this._newLine; }
            set { this._newLine = value; }
        }


        /// <summary>
        /// 标题模板
        /// </summary>
        public string TitleTemple { get; set; }
        /// <summary>
        /// 组模板
        /// </summary>
        public string ItemTemple { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string name = filterContext.ActionDescriptor.ActionName;
            var context = filterContext.HttpContext;

            var parent = filterContext.ParentActionViewContext;

            this.CheckItem(context, name, "OnActionExecuted", parent);

            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            string name = filterContext.ActionDescriptor.ActionName;
            var parent = filterContext.ParentActionViewContext;
            var context = filterContext.HttpContext;

            this.CheckItem(context, name, "OnActionExecuting", parent);


            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var parent = filterContext.ParentActionViewContext;

            string name = filterContext.RouteData.Values["action"].ToString();
            var context = filterContext.HttpContext;

            this.CheckItem(context, name, "OnResultExecuted", parent);

            if (!filterContext.IsChildAction && filterContext.Result is ViewResult)
            {
                WriteReport(context);
            }


            base.OnResultExecuted(filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            string name = filterContext.RouteData.Values["action"].ToString();
            var context = filterContext.HttpContext;
            var parent = filterContext.ParentActionViewContext;
            this.CheckItem(context, name, "OnResultExecuting", parent);

            base.OnResultExecuting(filterContext);
        }

        private void WriteReport(HttpContextBase context)
        {

            if (!IsShow)
                context.Response.Write("<!--");
            context.Response.Write("<b>调试信息</b>" + NewLine);

            var list = context.Items[__List__] as List<NameTime>;



            foreach (var nameTime in list)
            {
                context.Response.Write("" + nameTime.Name + "->" + nameTime.Do + "->" + nameTime.Time + "->" + (nameTime.Parent != null ? nameTime.Parent.RouteData.Values["action"].ToString() : "NONE") + NewLine);
            }
            context.Response.Write("<br/>-------------------------------------------------------------" + NewLine);

            var node = Parse(list);

            var str = PrintNode(node);

            context.Response.Write(str);

            if (!IsShow)
                context.Response.Write("-->");


            // CS html Trace

            var cshtmlstr = CshtmlTrance.Print(IsShow, NewLine);

            context.Response.Write(cshtmlstr);

        }


        private string PrintNode(WatchNode node)
        {

            var s = "name:" + node.Name + " = " + node.All + "  action->" + node.Action + " Result->" + node.Result + " parent->" + node.ParentName + NewLine;

            if (node.Child != null && node.Child.Count > 0)
            {
                s += "c___:" + node.Name + "=" + node.Child.Sum(x => x.All) + "  action->" +
                     node.Child.Sum(x => x.Action) + " Result->" + node.Child.Sum(x => x.Result) + NewLine;
            }

            foreach (var watchData in node.Child)
            {
                var temp = watchData;
                var c = 0;
                while (temp.Parent != null)
                {
                    temp = temp.Parent;
                    c++;
                }
                s += "".PadLeft(c * 2, '-') + PrintNode(watchData) ;
            }

            return s;

        }



        private WatchNode Parse(List<NameTime> list)
        {
            WatchNode TopNode = new WatchNode();
            var current = TopNode;
            foreach (var nameTime in list)
            {

                switch (nameTime.Do)
                {
                    case "OnActionExecuting":
                        if (nameTime.Parent == null)
                        {
                            TopNode.IsChild = false;
                            TopNode.OnActionExecuting = nameTime.Time;
                        }
                        else
                        {
                            var newnode = new WatchNode();
                            newnode.Parent = current;
                            current.Child.Add(newnode);
                            current = newnode;
                            current.IsChild = true;
                            current.OnActionExecuting = nameTime.Time;
                        }

                        current.Name = nameTime.Name;

                        break;
                    case "OnActionExecuted":
                        current.OnActionExecuted = nameTime.Time;

                        break;
                    case "OnResultExecuting":
                        current.OnResultExecuting = nameTime.Time;
                        break;
                    case "OnResultExecuted":
                        current.OnResultExecuted = nameTime.Time;

                        if (current.Parent != null && current.Parent.Name == nameTime.ParentName)
                        {
                            current = current.Parent;
                        }

                        break;

                }

            }

            return TopNode;
        }

        private void CheckItem(HttpContextBase httpcontext, string name, string Do, ViewContext parent)
        {

            if (!httpcontext.Items.Contains(__List__))
            {
                httpcontext.Items.Add(__List__, new List<NameTime>());
            }

            var list = httpcontext.Items[__List__] as List<NameTime>;

            list.Add(new NameTime
            {
                Name = name,
                Time = System.DateTime.Now.Ticks,
                Do = Do,
                Parent = parent
            });

        }



    }


    internal class CshtmlTraceModel
    {
        public string Name { get; set; }
        public long TimeTicks { get; set; }
    }

    /// <summary>
    /// CSHtml页面TRance
    /// </summary>
    public class CshtmlTrance
    {
        private const string CshtmlTrancekey = "_________CshtmlTrancekey_____________";
        public static void Add(string name, long timeTicket)
        {
            var httpcontext = HttpContext.Current;

            if (!httpcontext.Items.Contains(CshtmlTrancekey))
            {
                httpcontext.Items.Add(CshtmlTrancekey, new List<CshtmlTraceModel>());
            }

            var list = httpcontext.Items[CshtmlTrancekey] as List<CshtmlTraceModel>;

            list.Add(new CshtmlTraceModel
            {
                Name = name,
                TimeTicks = timeTicket
            });
        }


        public static string Print(bool isshow = false, string newLine = "<br/>\r\n")
        {
            var httpcontext = HttpContext.Current;

            var list = new List<CshtmlTraceModel>();

            if (httpcontext.Items.Contains(CshtmlTrancekey))
            {
                list = httpcontext.Items[CshtmlTrancekey] as List<CshtmlTraceModel>;
            }


            StringBuilder sb = new StringBuilder();




            if (!isshow)
                sb.Append("<!-- ").Append(newLine);

            sb.Append("cshtml 跟踪报告").Append(newLine);

            foreach (var cshtmlTraceModel in list)
            {
                sb.Append("" + cshtmlTraceModel.Name + "->" + cshtmlTraceModel.TimeTicks / 10000).Append(newLine);
            }

            if (!isshow)
                sb.Append("-->");

            return sb.ToString();
        }

    }

}