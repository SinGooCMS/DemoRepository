using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Useage.Code.Model;
using Dapper;
using Useage.Code.Utils;
using Useage.Code;
using System.Data;
using System.Data.SqlClient;

namespace Useage.Controllers
{
    public class DefaultController : Controller
    {
        DapperClient client = new DapperClient();
        // GET: Default
        public ActionResult Index()
        {
            /*
            string sql = "select distinct(线长工号)+',' from 线长人员设置 where rtrim(在职状态)='在职在岗' for xml path('')";
            string result = client.GetModel<string>(sql);
            result = "'" + result.TrimEnd(',').Replace(",", "','") + "'";
            ViewBag.Test = result;*/
            /*
            DataTable dt = new DataTable();
            dt.Columns.Add("自动", typeof(string));
            dt.Columns.Add("工号1", typeof(string));
            dt.Columns.Add("打卡时间1", typeof(DateTime));

            for (int i = 0; i < 100; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = "";
                dr[1] = "aa" + i;
                dr[2] = System.DateTime.Now;
                dt.Rows.Add(dr);
            }
            DBHelper.BulkInsert(dt, "任务_考勤");*/

            ViewBag.List = client.GetList<UserInfo>();
            return View();
        }

        [HttpGet]
        public string Get(int id) =>
            client.GetModel<UserInfo>(id).ToJson();

        [HttpGet]
        public string GetPager(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            int totalPage = 0;
            var lst = client.GetPageList<UserInfo>("*", " 1=1 ", "AutoID desc", pageIndex, pageSize, ref totalCount, ref totalPage);

            return $"{{\"result\":{lst.ToJson()},\"totalCount\":{totalCount},\"totalPage\":{totalPage}}}";
        }

        [HttpPost]
        public string Add()
        {
            int ret = client.Add<UserInfo>(new UserInfo()
            {
                UserName = Request.Form["name"].ToString(),
                Gander = Request.Form["gander"].ToString(),
                Age = int.Parse(Request.Form["age"])
            });

            return $"{{\"result\":{(ret > 0 ? "true" : "false")}}}";
        }

        [HttpPost]
        public string Update()
        {
            bool ret = false;
            var user = client.GetModel<UserInfo>(int.Parse(Request.Form["id"]));
            if (user != null)
            {
                user.UserName = Request.Form["name"].ToString();
                user.Gander = Request.Form["gander"].ToString();
                user.Age = int.Parse(Request.Form["age"]);

                ret = client.Update<UserInfo>(user);
            }

            return $"{{\"result\":{(ret ? "true" : "false")}}}";
        }

        [HttpPost]
        public string Del()
        {
            //bool ret = client.Delete<UserInfo>(" and AutoID=" + int.Parse(Request.Form["id"]));
            bool ret = client.Delete<UserInfo>(new UserInfo() { AutoID = int.Parse(Request.Form["id"]) });
            return $"{{\"result\":{(ret ? "true" : "false")}}}";
        }
    }
}