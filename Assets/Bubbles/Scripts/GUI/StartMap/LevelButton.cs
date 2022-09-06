using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class LevelButton : MonoBehaviour
    {
        [Header("Stars")]
        public Image LeftStar;
        public Image MiddleStar;
        public Image RightStar;
        public Sprite fullStar;

        [Header ("Lock")]
        public Sprite ActiveButtonSprite;
        public Sprite LockedButtonSprite;
        public Image lockImage;

        [Header("Button")]
        public Button button;
        public Text numberText;

        public bool Interactable { get; private set; }

        internal void SetActive(bool active, int activeStarsCount, bool isPassed)
        {
            if (!fullStar)
            {
                LeftStar.gameObject.SetActive(activeStarsCount > 0 && isPassed);
                MiddleStar.gameObject.SetActive(activeStarsCount > 1 && isPassed);
                RightStar.gameObject.SetActive(activeStarsCount > 2 && isPassed);
            }
            else
            {
                LeftStar.gameObject.SetActive(isPassed);
                MiddleStar.gameObject.SetActive(isPassed);
                RightStar.gameObject.SetActive(isPassed);
                if (activeStarsCount > 0) LeftStar.sprite = fullStar;
                if (activeStarsCount > 1) MiddleStar.sprite = fullStar;
                if (activeStarsCount > 2) RightStar.sprite = fullStar;
            }
            Interactable = active || isPassed;
            if(button)   button.interactable = active || isPassed;

            if (lockImage)
            {
                lockImage.gameObject.SetActive(!isPassed);
                lockImage.sprite = (!active) ? LockedButtonSprite : ActiveButtonSprite;
            }
            if (active)
            {
                MapController.Instance.ActiveButton = this;
            }
        }
    }
}