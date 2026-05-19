using System.ComponentModel.DataAnnotations;

namespace API.Models.Requests;

/// <summary>Cell index (0–8) chosen by the player.</summary>
public record MakeMoveRequest([Range(0, 8)] int Cell);
