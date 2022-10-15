using AutoMapper;
using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class CommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _dbContext;
        private readonly IMapper _mapper;
        public CommanderRepo(CommanderContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Command>> GetAllCommandsAsync()
        {
            return await _dbContext.Commands.ToListAsync();
        }

        public async Task<Command> GetCommandByIdAsync(int id)
        {
            return await _dbContext.Commands.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd is null) throw new ArgumentNullException();
            _dbContext.Commands.Add(cmd);
        }

        public void Delete(Command cmd)
        {
            if (cmd is null) throw new ArgumentNullException();
            _dbContext.Commands.Remove(cmd);
        }

    }
}