using UnityEngine;

/*
    30.10.2020 - first
 */

namespace Mkey
{
	public class ShowSelfClosingMessage : MonoBehaviour
	{
        [SerializeField]
        private WarningMessController messagePrefab;
        [SerializeField]
        private string caption = "Succesfull!!!";
        [SerializeField]
        private string message = "Product received successfull.";
        [SerializeField]
        private float showTime = 3f;

        #region temp vars
        protected static GuiController mGui;
        #endregion temp vars


        public void ShowMesssage()
        {
            if (!mGui) mGui = FindObjectOfType<GuiController>();
            if (mGui && messagePrefab)
            {
                mGui.ShowMessageWithYesNoCloseButton(messagePrefab, caption, message, () => { }, null, null);
            }
            else if (mGui)
            {
                mGui.ShowMessageWithYesNoCloseButton(caption, message, () => { }, null, null);
            }
        }
    }
}
