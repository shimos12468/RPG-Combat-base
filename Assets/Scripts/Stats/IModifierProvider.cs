using RPG.stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.stats{

    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }

}
