using API.Models;

namespace API.Repositories;

public interface IGameRepository
{
    Game Create();
    Game? GetById(Guid id);
    void Save(Game game);
}
