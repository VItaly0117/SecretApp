namespace SecretApp.Models;

public class SecretMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EncryptedData { get; set; } = string.Empty; // Зашифрований текст (Base64)
    public string Iv { get; set; } = string.Empty;            // Вектор ініціалізації для AES
    public string PasswordHash { get; set; } = string.Empty;  // Хеш для перевірки доступу
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}