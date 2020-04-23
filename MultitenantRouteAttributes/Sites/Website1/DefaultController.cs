using Microsoft.AspNetCore.Mvc;

namespace MultitenantRouteAttributes.Sites.Website1
{
    [Area("Website1")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}