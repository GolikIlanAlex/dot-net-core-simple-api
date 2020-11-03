using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IParser
    {
        IActionResult Parse(ChargeResponse response);
    }
}