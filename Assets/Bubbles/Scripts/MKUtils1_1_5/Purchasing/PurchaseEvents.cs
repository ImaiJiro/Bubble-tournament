using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    31.10.2020 
 */
namespace Mkey
{
    public class PurchaseEvents : MonoBehaviour
    {
        [SerializeField]
        private WarningMessController messagePrefab;
        [SerializeField]
        private bool showGoodMessage = true;
        [SerializeField]
        private bool showFailedMessage = true;

        private static GuiController mGui;
        private Purchaser MPurchaser => Purchaser.Instance;

        #region regular
        private void Start()
        {
            if (!mGui) mGui = FindObjectOfType<GuiController>();
            if (MPurchaser)
            {
               if(showGoodMessage) MPurchaser.GoodPurchaseEvent += GoodPurchaseMessage;
               if(showFailedMessage) MPurchaser.FailedPurchaseEvent += FailedPurchaseMessage;
            }
        }

        private void OnDestroy()
        {
            if (MPurchaser)
            {
                MPurchaser.GoodPurchaseEvent -= GoodPurchaseMessage;
                MPurchaser.FailedPurchaseEvent -= FailedPurchaseMessage;
            }
        }
        #endregion regular

        internal void GoodPurchaseMessage(string prodId, string prodName)
        {
            if (mGui && messagePrefab)
            {
                mGui.ShowMessageWithYesNoCloseButton(messagePrefab, "Succesfull!!!", prodName + " purchased successfull.", () => { }, null, null);
            }
            else if (mGui)
            {
                mGui.ShowMessageWithYesNoCloseButton("Succesfull!!!", prodName + " purchased successfull.", () => { }, null, null);
            }
        }

        internal void FailedPurchaseMessage(string prodId, string prodName)
        {
            if (mGui && messagePrefab)
            {
                mGui.ShowMessageWithYesNoCloseButton(messagePrefab, "Sorry.", prodName + " - purchase failed.", () => { }, null, null);
            }
            else if (mGui)
            {
                mGui.ShowMessageWithYesNoCloseButton("Sorry.", prodName + " - purchase failed.", () => { }, null, null);
            }
        }

        internal void GoodPurchaseMessage(string message)
        {
            if (mGui)
            {
                mGui.ShowMessage("Succesfull!!!", message, 3, null);
            }
        }

        internal void FailedPurchaseMessage(string message)
        {
            if (mGui)
            {
                mGui.ShowMessage("Sorry.", message, 3, null);
            }
        }
    }
}