using System.ComponentModel.DataAnnotations;

namespace AndreRosler.AspNetApi.DTOs;

public record TaskCreateRequest(
    [Required, MaxLength(200)] string Title,
    [MaxLength(2000)] string? Description,
    bool IsDone = false);

public record TaskUpdateRequest(
    [Required, MaxLength(200)] string Title,
    [MaxLength(2000)] string? Description,
    bool IsDone);

public record TaskResponse(int Id, string Title, string? Description, bool IsDone, DateTime CreatedAt);
