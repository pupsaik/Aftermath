using AftermathModels.Loot;
using Aftermath.Views.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aftermath.ViewModels
{
    public class ExpeditionSelectOccupiedVM : BaseViewModel
    {
        public string CharacterImagePath { get; }

        public string ToolName { get; }

        public ExpeditionSelectOccupiedVM(Character character, ITool tool)
        {
            CharacterImagePath = character.Name;
            ToolName = tool != null ? tool.Name : null;
        }
    }
}
