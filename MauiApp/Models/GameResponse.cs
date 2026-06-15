using System.Text.Json.Serialization;

namespace TicTacToe.Maui.Models;

/// <summary>Mirrors API.Models.Responses.GameResponse (camelCase JSON from ASP.NET Core).</summary>
public record GameResponse(
    [property: JsonPropertyName("id")]          Guid     Id,
    [property: JsonPropertyName("board")]        string[] Board,
    [property: JsonPropertyName("status")]       string   Status,
    [property: JsonPropertyName("botCell")]      int?     BotCell,
    [property: JsonPropertyName("createdAt")]    DateTime CreatedAt,
    [property: JsonPropertyName("finishedAt")]   DateTime? FinishedAt
);
