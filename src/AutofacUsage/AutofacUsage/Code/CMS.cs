using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Code
{
    public interface ICMS
    {
        string WriteSomething();
    }

    public class CMS : ICMS
    {
        public string WriteSomething()
        {
            return "123";
        }
    }
}