using splitwisescaler.CommandPattern;

namespace splitwisescaler
{
    public class CommandInvoker
    {
        private Stack<ICommand> _commandHistory;

        public CommandInvoker()
        {
            _commandHistory = new Stack<ICommand>();
        }

        public void ExecuteCommand(ICommand command)
        {
            if (command.CanExecute())
            {
                command.Execute();
                _commandHistory.Push(command);
            }
            else
            {
                Console.WriteLine("Cannot execute command!");
            }
        }

        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                var command = _commandHistory.Pop();
                command.Undo();
            }
            else
            {
                Console.WriteLine("No commands to undo!");
            }
        }
    }
}
