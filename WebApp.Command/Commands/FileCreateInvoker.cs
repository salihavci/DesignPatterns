using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Command.Commands
{
    public class FileCreateInvoker
    {
        private ITableActionCommand _actionCommand;
        private List<ITableActionCommand> _tableActionCommand = new List<ITableActionCommand>();
        public void SetCommand(ITableActionCommand actionCommand)
        {
            _actionCommand = actionCommand;
        }

        public void AddCommand(ITableActionCommand actionCommand)
        {
            _tableActionCommand.Add(actionCommand);
        }

        public void RemoveCommand(ITableActionCommand actionCommand)
        {
            _tableActionCommand.Remove(actionCommand);
        }

        public IActionResult CreateFile()
        {
            return _actionCommand.Execute();
        }
        
        public List<IActionResult> CreateFiles()
        {
            return _tableActionCommand.Select(x => x.Execute()).ToList();
        }

    }
}
