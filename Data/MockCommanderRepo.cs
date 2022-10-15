using Commander.Models;

namespace Commander.Data
{
    public class MockCommanderRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Command>> GetAllCommands()
        {
            var commands = new List<Command>
            {
              new Command
              {
                Id = 0,
                HowTo = "Test stuff",
                Line = "command here",
                Platform = "Android"
              },
              new Command
              {
                Id = 2,
                HowTo = "Boil an egg",
                Line = "bla bla here",
                Platform = "LAA"
              }
            };

            return Task.FromResult(commands.AsEnumerable());
        }

        public Task<Command> GetCommandById(int id)
        {
            return Task.FromResult(new Command
            {
                Id = id,
                HowTo = "Test stuff",
                Line = "command here",
                Platform = "Android"
            });
        }

        public Task<bool> SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}