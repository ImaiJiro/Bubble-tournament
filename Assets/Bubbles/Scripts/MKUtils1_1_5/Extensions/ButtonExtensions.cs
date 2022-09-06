using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
  20.05.2020 - first
*/

namespace Mkey
{
    public static class ButtonExtensions
    {
        public static void Release(this Button b)
        {
            Image im = b.GetComponent<Image>();
            Sprite normal = b.spriteState.pressedSprite;
            Sprite pressed = im.sprite;
            im.sprite = normal;
            SpriteState bST = b.spriteState;
            bST.pressedSprite = pressed;
            b.spriteState = bST;
        }

        public static void SetPressed(this Button b)
        {
            Image im = b.GetComponent<Image>();
            Sprite normal = im.sprite;
            Sprite pressed = b.spriteState.pressedSprite;
            im.sprite = pressed;
            SpriteState bST = b.spriteState;
            bST.pressedSprite = normal;
            b.spriteState = bST;
        }
    }
}