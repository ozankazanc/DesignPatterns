using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Commands
{
    public class FileCreateInvoker
    {
        private ITableActionCommand _command;
        private List<ITableActionCommand> _commands = new List<ITableActionCommand>();
        public void SetCommand(ITableActionCommand command)
        {
            _command = command;
        }
        public void AddCommand(ITableActionCommand tableActionCommand)
        {
            _commands.Add(tableActionCommand);
        }

        public IActionResult CreateFile()
        {
            return _command.Execute();
        }

        public List<IActionResult> CreateFiles()
        {
            return _commands.Select(x => x.Execute()).ToList();
        }
    }
}
