using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace Useage.Code.Model
{    
    [Table("cms_User")]
    public class UserInfo
    {
        [Key]
        public int AutoID { get; set; }
        public string UserName { get; set; }
        public string Gander { get; set; }
        public int Age { get; set; }
    }
}