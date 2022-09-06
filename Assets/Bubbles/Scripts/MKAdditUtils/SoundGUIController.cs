using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
	public class SoundGUIController : MonoBehaviour
	{
        [SerializeField]
        private PSlider volumeSlider;

        [SerializeField]
        private ToggleButton soundButton;
        [SerializeField]
        private ToggleButton musicButton;

        [SerializeField]
        private Image musicIcon;
        [SerializeField]
        private Image soundIcon;

        [SerializeField]
        private Sprite musicOnIconSprite;
        [SerializeField]
        private Sprite musicOffIconSprite;
        [SerializeField]
        private Sprite soundOnIconSprite;
        [SerializeField]
        private Sprite soundOffIconSprite;

        #region temp vars
        private SoundMaster MSound { get { return SoundMaster.Instance; } }
        #endregion temp vars

        #region regular
        private IEnumerator Start()
        {
            while (!MSound) yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            RefreshSound();
        }
        #endregion regular

        private void RefreshSound()
        {
            if (volumeSlider) volumeSlider.SetFillAmount(MSound.Volume);

            if (musicIcon) musicIcon.sprite =  !MSound.MusicOn? musicOffIconSprite : musicOnIconSprite;
            if (soundIcon) soundIcon.sprite = !MSound.SoundOn ? soundOffIconSprite : soundOnIconSprite;

            if (soundButton) soundButton.IsOn = MSound.SoundOn;
            if (musicButton) musicButton.IsOn = MSound.MusicOn;
        }

        public void MusikButtonClick()
        {
            MSound.SetMusic(!MSound.MusicOn);
            RefreshSound();
        }

        public void SoundButtonClick()
        {
            MSound.SetSound(!MSound.SoundOn);
            RefreshSound();
        }

        public void VolumeButton_Click(bool plus)
        {
            float currVolume = SoundMaster.Instance.Volume;
            currVolume = (plus) ? currVolume + 0.1f : currVolume - 0.1f;
            currVolume = Mathf.Clamp(currVolume, 0.0f, 1.0f);
            MSound.SetVolume(currVolume);
            if (volumeSlider) volumeSlider.SetFillAmount(MSound.Volume);
        }
    }
}
