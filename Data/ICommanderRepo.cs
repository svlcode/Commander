using Commander.Models;

namespace Commander.Data
{
    public interface ICommanderRepo
    {
        Task<IEnumerable<Command>> GetAllCommandsAsync();
        Task<Command> GetCommandByIdAsync(int id);
        void CreateCommand(Command cmd);
        Task<bool> SaveChangesAsync();
        void Delete(Command cmd);
    }
}