using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BackRock
{
    public class CharacterAnimations : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator animator;

        [Header("Animation Clips")]
        public List<AnimationClip> animations = new List<AnimationClip>();

        [Header("Runtime Info (Read Only)")]
        [SerializeField] private List<string> animationNames = new List<string>();

        private int currentIndex = 0;

        [Header("Events")]
        public UnityEvent<string> OnAnimationChanged;

        private void Awake()
        {
            if (!animator)
                animator = GetComponent<Animator>();

            CacheAnimationNames();
        }

        private void Start()
        {
            if (animations.Count > 0)
                PlayCurrentAnimation();
        }

        // NEXT
        public void NextAnimation()
        {
            if (animations.Count == 0) return;

            currentIndex = (currentIndex + 1) % animations.Count;
            PlayCurrentAnimation();
        }

        //PREVIOUS
        public void PreviousAnimation()
        {
            if (animations.Count == 0) return;

            currentIndex--;
            if (currentIndex < 0)
                currentIndex = animations.Count - 1;

            PlayCurrentAnimation();
        }

        private void PlayCurrentAnimation()
        {
            AnimationClip clip = animations[currentIndex];
            animator.Play(clip.name, 0, 0f);

            // Notify UI
            OnAnimationChanged?.Invoke(animationNames[currentIndex]);
        }

        private void CacheAnimationNames()
        {
            animationNames.Clear();

            foreach (var clip in animations)
            {
                if (clip != null)
                    animationNames.Add(clip.name);
            }
        }

        // UI Helpers
        public string GetCurrentAnimationName()
        {
            if (animationNames.Count == 0) return string.Empty;
            return animationNames[currentIndex];
        }

        public int GetCurrentIndex() => currentIndex;
    }

}