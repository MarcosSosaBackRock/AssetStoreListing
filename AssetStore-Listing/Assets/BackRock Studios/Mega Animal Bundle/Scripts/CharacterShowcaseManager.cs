using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BackRock
{
    public class CharacterShowcaseManager : MonoBehaviour
    {
        [Header("Characters")]
        public List<CharacterAnimations> characters = new List<CharacterAnimations>();

        private int currentCharacterIndex = 0;

        [Header("Events")]
        public UnityEvent<string> OnCharacterChanged;
        public UnityEvent<string> OnAnimationChanged;


        [Header("Camera")]
        public OrbitCamera orbitCamera;


        private void Start()
        {
            ShowOnlyCurrentCharacter();
        }

        // ---------------- CHARACTER SWITCH ----------------

        public void NextCharacter()
        {
            if (characters.Count == 0) return;

            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            ShowOnlyCurrentCharacter();
        }

        public void PreviousCharacter()
        {
            if (characters.Count == 0) return;

            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
                currentCharacterIndex = characters.Count - 1;

            ShowOnlyCurrentCharacter();
        }

        private void ShowOnlyCurrentCharacter()
        {
            for (int i = 0; i < characters.Count; i++)
                characters[i].gameObject.SetActive(i == currentCharacterIndex);

            var current = GetCurrentCharacter();

            orbitCamera.target = current.transform;
            orbitCamera.SetCharacterView(current);

            OnCharacterChanged?.Invoke(current.gameObject.name);
            OnAnimationChanged?.Invoke(current.GetCurrentAnimationName());
        }


        // ---------------- ANIMATION CONTROLS ----------------

        public void NextAnimation()
        {
            var c = GetCurrentCharacter();
            c.NextAnimation();
            OnAnimationChanged?.Invoke(c.GetCurrentAnimationName());
        }

        public void PreviousAnimation()
        {
            var c = GetCurrentCharacter();
            c.PreviousAnimation();
            OnAnimationChanged?.Invoke(c.GetCurrentAnimationName());
        }

        // ---------------- HELPERS ----------------

        public CharacterAnimations GetCurrentCharacter()
        {
            return characters[currentCharacterIndex];
        }
    }
}
