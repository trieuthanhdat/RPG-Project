using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Lowpoly RPG/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] CharacterProgressionClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;
        public float GetStats(Stats stat, CharacterClass characterClass, int level)
        {
            //using dictionary to look up the level of the character
            BuildDictionaryTable();
            float[] levels = lookupTable[characterClass][stat];

            if(levels.Length <= level) return 0;

            return levels[level-1];
            
            //Another way
            // foreach (CharacterProgressionClass characterProgressionClass in characterClasses)
            // {
            //     if(characterProgressionClass.characterClass != characterClass )
            //         continue;
                
            //     foreach(CharacterProgressionStat progressionStat in characterProgressionClass.stats)
            //     {
            //         if(progressionStat.stat != stat) continue;
            //         if(progressionStat.levels.Length < level) continue;

            //         return progressionStat.levels[level-1];
            //     }
            // }
            // return 0;
        }
        public int GetLevels(CharacterClass characterClass, Stats stats)
        {
            BuildDictionaryTable();
            float[] levels = lookupTable[characterClass][stats];

            return levels.Length;
        }
        private void BuildDictionaryTable()
        {
            if(lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach(CharacterProgressionClass progressionClass in characterClasses)
            {
                var statTable = new Dictionary<Stats, float[]>();
                foreach(CharacterProgressionStat progressionStat in progressionClass.stats)
                {
                    statTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statTable;
            }

        }

        [System.Serializable]
        class CharacterProgressionClass
        {
            public CharacterClass characterClass;
            public CharacterProgressionStat[] stats;
        }
        
        [System.Serializable]
        class CharacterProgressionStat
        {
            public Stats stat;
            public float[] levels;
        }
    }
}
