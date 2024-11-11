using Microsoft.AspNetCore.Mvc;

namespace Argo.MD.HealthCheck.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("/healthchecks-ui");
    }
}