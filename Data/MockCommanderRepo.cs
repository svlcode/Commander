using Commander.Models;

namespace Commander.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        public IEnumerable<Command> GetAppCommands()
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

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command
            {
                Id = id,
                HowTo = "Test stuff",
                Line = "command here",
                Platform = "Android"
            };
        }
    }
}