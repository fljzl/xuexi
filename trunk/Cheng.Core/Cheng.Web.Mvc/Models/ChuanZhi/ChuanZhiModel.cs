using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheng.Web.Mvc.Models.ChuanZhi
{
    public class ChuanZhiModel
    {
        public int Id { get; set; } = 1;

        public string Name { get; set; } = "123";

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
