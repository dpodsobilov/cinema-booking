using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class TestController : ControllerBase
{
    [HttpGet]
    public List<Ticket> Get()
    {
        List<Ticket> Tickets = new List<Ticket>();
        using(ApplicationContext db = new ApplicationContext())
        {
            //Tickets = db.Tickets.ToList();
            Tickets.AddRange(db.Tickets.ToList());
        }
        return Tickets;
    }
}