using Microsoft.AspNetCore.Mvc;

namespace MultitenantRouteAttributes.Sites._default
{
    [Area("_default")]
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