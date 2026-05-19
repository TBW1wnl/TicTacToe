using API.Models;
using API.Models.Requests;
using API.Models.Responses;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public static class GameController
{
    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games").WithTags("Games");

        group.MapPost("/", (IGameService gameService) =>
        {
            var game = gameService.CreateGame();
            var response = ToResponse(game, botCell: null);
            return Results.Created($"/games/{game.Id}", response);
        })
        .WithName("CreateGame")
        .WithSummary("Start a new player-vs-bot game. Player is always X and moves first.");

        group.MapGet("/{id:guid}", (Guid id, IGameService gameService) =>
        {
            var game = gameService.GetGame(id);
            return game is null
                ? Results.NotFound()
                : Results.Ok(ToResponse(game, botCell: null));
        })
        .WithName("GetGame")
        .WithSummary("Get the current state of a game.");

        group.MapPost("/{id:guid}/move", (
            Guid id,
            [FromBody] MakeMoveRequest request,
            IGameService gameService) =>
        {
            if (request.Cell is < 0 or > 8)
                return Results.BadRequest("Cell must be between 0 and 8.");

            try
            {
                var (game, botCell) = gameService.MakeMove(id, request.Cell);
                return Results.Ok(ToResponse(game, botCell));
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(ex.Message);
            }
        })
        .WithName("MakeMove")
        .WithSummary("Play a cell (0–8). The bot replies immediately in the same response.");

        return app;
    }

    private static GameResponse ToResponse(Game game, int? botCell) =>
        new(
            Id: game.Id,
            Board: game.Board.Select(c => c.ToString()).ToArray(),
            Status: game.Status.ToString(),
            BotCell: botCell,
            CreatedAt: game.CreatedAt,
            FinishedAt: game.FinishedAt
        );
}
