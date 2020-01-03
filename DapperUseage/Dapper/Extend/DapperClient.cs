using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Dapper
{
    /// <summary>
    /// Dapper客户端 Created by JsonLee 2019-12-20
    /// </summary>
    public class DapperClient
    {
        private string _connstr = ConnStore.ConnStr;
        public DapperClient(string connStr)
        {
            _connstr = connStr;
        }
        public DapperClient() { }

        #region 一般查询

        /// <summary>
        /// 查询并返回Id为主键的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetModel<T>(int id) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.Get<T>(id);
            }
        }

        public T GetModel<T>(string sql) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.QueryFirstOrDefault<T>(sql);
            }
        }

        public IList<T> GetList<T>(string where="",string orderBy="") where T:class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                string tableName = SqlMapperExtensions.GetTableName(typeof(T));
                string sql = $" select * from {tableName} ";
                if (!string.IsNullOrEmpty(where))
                    sql += $" where {where} ";
                if (!string.IsNullOrEmpty(orderBy))
                    sql += $" order by {orderBy} ";

                return conn.Query<T>(sql).ToList();
            }
        }

        #endregion

        #region 分页

        /// <summary>
        /// 查询分页记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        public IList<T> GetPageList<T>(string filters, string where, string orderby,
            int pageIndex, int pageSize, ref int totalCount, ref int totalPage) where T : class
        {
            string tableName = SqlMapperExtensions.GetTableName(typeof(T));
            if (string.IsNullOrEmpty(where))
                where = " 1=1 ";

            using (var conn = new SqlConnection(_connstr))
            {
                int skip = 1;
                if (pageIndex > 0)
                {
                    skip = (pageIndex - 1) * pageSize + 1;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT COUNT(1) FROM {0} where {1};", tableName, where); //总记录数
                sb.AppendFormat(@"SELECT  {0}
                                FROM(SELECT ROW_NUMBER() OVER(ORDER BY {3}) AS RowNum,{0}
                                          FROM  {1}
                                          WHERE {2}
                                        ) AS result
                                WHERE  RowNum >= {4}   AND RowNum <= {5}
                                ORDER BY {3}", filters, tableName, where, orderby, skip, pageIndex * pageSize);
                using (var reader = conn.QueryMultiple(sb.ToString()))
                {
                    totalCount = reader.ReadFirst<int>();
                    totalPage = totalCount % pageSize == 0 ? totalCount / pageSize : (totalCount / pageSize) + 1;
                    return reader.Read<T>().ToList();
                }
            }
        }

        public int GetCount<T>(string condition) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.GetCount<T>(condition);
            }
        }

        #endregion

        #region 增加

        public int Add<T>(T model) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.Insert<T>(model);
            }
        }

        #endregion

        #region 修改

        public bool Update<T>(T model) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.Update<T>(model);
            }
        }

        #endregion

        #region 删除

        public bool Delete<T>(string condition) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.Delete<T>(condition);
            }
        }

        public bool Delete<T>(T model) where T : class
        {
            using (var conn = new SqlConnection(_connstr))
            {
                return conn.Delete<T>(model);
            }
        }

        #endregion
    }
}
