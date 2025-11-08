using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.IConsumableCommand
{
    public class ConsumableCommandInvoker
    {
        public readonly List<IConsumableCommand> History = [];
        public Action OnHistoryChanged;

        public void ExecuteCommand(IConsumableCommand consumableCommand)
        {
            consumableCommand.Execute();
            History.Add(consumableCommand);
            OnHistoryChanged?.Invoke();
        }

        public void UndoCommand(IConsumableCommand consumableCommand)
        {
            consumableCommand.Undo();
            History.Remove(consumableCommand);
            OnHistoryChanged?.Invoke();
        }

        public void ClearHistory()
        {
            History.Clear();
            OnHistoryChanged?.Invoke();
        }
    }
}
