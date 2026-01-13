using Microsoft.AspNetCore.Mvc;
using SecretApp.Data;
using SecretApp.Models;

namespace SecretApp.Controllers;

public class MessagesController : Controller
{
    private readonly AppDbContext _context;
    public MessagesController(AppDbContext context) => _context = context;

    // СТОРІНКА 1: Головна
    public IActionResult Index() => View();

    // СТОРІНКА 2: Форма створення
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SecretMessage msg)
    {
        // Хешуємо пароль, щоб сервер міг перевірити право на видалення/читання
        msg.PasswordHash = BCrypt.Net.BCrypt.HashPassword(msg.PasswordHash);
        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();
        
        var url = $"{Request.Scheme}://{Request.Host}/Messages/Unlock/{msg.Id}";
        return Json(new { url });
    }

    // СТОРІНКА 3: Введення пароля
    [HttpGet]
    public IActionResult Unlock(Guid id) => View(id);

    // СТОРІНКА 4: Перегляд (API для отримання даних перед видаленням)
    [HttpPost]
    public async Task<IActionResult> GetAndDestroy(Guid id, [FromBody] string password)
    {
        var msg = await _context.Messages.FindAsync(id);
        if (msg == null || !BCrypt.Net.BCrypt.Verify(password, msg.PasswordHash))
            return BadRequest("Пароль невірний або повідомлення вже видалено.");

        var data = new { msg.EncryptedData, msg.Iv };

        // ВИДАЛЕННЯ: Повідомлення видаляється назавжди після першого запиту
        _context.Messages.Remove(msg);
        await _context.SaveChangesAsync();

        return Json(data);
    }
}