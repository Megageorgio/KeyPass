using KeyPass.Data;
using KeyPass.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;

namespace KeyPass.Controllers
{
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Include(x => x.Department).Include(x => x.Passes).ThenInclude(x => x.PassType).First(user => user.Id == userId);
            if (user.Department == null)
            {
                return View(null);
            }
            var str = "ФИО: " + user.FullName + Environment.NewLine;
            str += "Отдел: " + user.Department.Name + Environment.NewLine;
            str += "Пропуска:";
            foreach (var pass in user.Passes)
            {
                str += Environment.NewLine + pass.PassType.Name + ": от " + pass.IssueDate.ToString("dd.MM.yyyy") + (pass.ExpirationDate == null ? "" : " до " + pass.ExpirationDate?.ToString("dd.MM.yyyy"));
            }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(str,
            QRCodeGenerator.ECCLevel.Q);
            
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(10);
            return View(qrCodeImage);
        }
    }
}