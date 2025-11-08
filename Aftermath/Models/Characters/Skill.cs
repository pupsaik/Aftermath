using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AftermathModels.Characters
{
    public class Skill
    {
        public SkillType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CurrentLevel { get; set; }
        public int MaxLevel { get; set; }
    }

    public enum SkillType
    {
        Fishing,
        Healing,
        WoodChopping,
        Exploring
    }
}
