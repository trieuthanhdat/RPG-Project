using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Lowpoly RPG/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] CharacterProgressionClass[] characterClasses = null;

        public float GetCharacterHealth(CharacterClass characterClass, int level)
        {
            foreach (CharacterProgressionClass characterProgressionClass in characterClasses)
            {
                if(characterProgressionClass.characterClass == characterClass )
                    return characterProgressionClass.health[level-1];
            }
            return 0;
        }

        [System.Serializable]
        class CharacterProgressionClass
        {
            public CharacterClass characterClass;
            public float[] health;
        }
    }
}
