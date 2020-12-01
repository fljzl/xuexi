using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cheng.Web.Mvc.Models;
using Cheng.Comon;
using Cheng.Comon.Web;
using Cheng.Web.Mvc.Language;
using Cheng.Web.Mvc.Models.ChuanZhi;

namespace Cheng.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICacheService _cacheService;

        public HomeController(ILogger<HomeController> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public IActionResult Index()
        {
            //_cacheService.SetCache(CacheKey.WEBCMSADV, "123", 10);
            //var code = _cacheService.GetCache<string>(CacheKey.WEBCMSADV);
            //Console.WriteLine(code);

            //var request = MvcContext.GetContext().Request;

            var gwdj = Transalate.GetVal("TYJS_000295");
            Console.WriteLine(gwdj);

            #region 视图传值
            ViewBag.data = new ChuanZhiModel
            {
                Id = 2,
                CreateTime = DateTime.Now.AddDays(1),
                Name = "1212"
            };
            #endregion
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
