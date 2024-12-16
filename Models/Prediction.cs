namespace BryophytaClassifier.Models;

public class Prediction : Entity<int> {
    public required BryophytaCategory Category { get; set; }

    public int UserId { get; set; }

    public record RequestDto(string ImageBytesInBase64, int UserId, decimal Latitude, decimal Longitude);

    public enum BryophytaCategory {
        None,
        Hornwort,
        Liverwort,
        Moss
    }
    public int Points { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public required string ImageBytes { get; set; }
}
