using TMPro;
using UnityEngine;

namespace BackRock { 
public class AnimationNameUI : MonoBehaviour
{
    public CharacterAnimations characterAnimations;
    public TMP_Text label;

    private void Start()
    {
        characterAnimations.OnAnimationChanged.AddListener(UpdateLabel);
        UpdateLabel(characterAnimations.GetCurrentAnimationName());
    }

    void UpdateLabel(string animName)
    {
        label.text = animName;
    }
}
}
