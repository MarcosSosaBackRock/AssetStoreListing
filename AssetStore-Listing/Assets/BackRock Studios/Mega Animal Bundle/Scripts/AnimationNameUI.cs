using TMPro;
using UnityEngine;

namespace BackRock
{
    public class AnimationNameUI : MonoBehaviour
    {
        [Header("References")]
        public CharacterShowcaseManager showcaseManager;

        public TMP_Text characterLabel;
        public TMP_Text animationLabel;

        private void Start()
        {
            showcaseManager.OnCharacterChanged.AddListener(UpdateCharacterLabel);
            showcaseManager.OnAnimationChanged.AddListener(UpdateAnimationLabel);

            // Init
            UpdateCharacterLabel(showcaseManager.GetCurrentCharacter().gameObject.name);
            UpdateAnimationLabel(showcaseManager.GetCurrentCharacter().GetCurrentAnimationName());
        }

        void UpdateCharacterLabel(string characterName)
        {
            characterLabel.text = characterName;
        }

        void UpdateAnimationLabel(string animName)
        {
            animationLabel.text = animName;
        }
    }
}
