using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
	public class DealSalePU : PopUpsController
	{
        [SerializeField]
        private Text dealTimeText;
        [SerializeField]
        private Button dealTimeButton;

        #region temp vars
        private DealSaleController DSC { get { return DealSaleController.Instance; } }
        #endregion temp vars

        #region regular
        private void Start()
        {
            if (DSC)
            {
                DSC.WorkingDealTickRestDaysHourMinSecEvent += WorkingDealTickRestDaysHourMinSecHandler;
                DSC.WorkingDealTimePassedEvent += WorkingDealTimePassedHandler;
                DSC.WorkingDealStartEvent += WorkingDealStartHandler;
                DSC.PausedDealStartEvent += PausedDealStartHandler;

                if (dealTimeButton) dealTimeButton.gameObject.SetActive(DSC.IsDealTime);
            }
        }
		
		private void OnDestroy()
        {
            if (DSC)
            {
                DSC.WorkingDealTickRestDaysHourMinSecEvent -= WorkingDealTickRestDaysHourMinSecHandler;
                DSC.WorkingDealTimePassedEvent -= WorkingDealTimePassedHandler;
                DSC.WorkingDealStartEvent -= WorkingDealStartHandler;
                DSC.PausedDealStartEvent -= PausedDealStartHandler;
            }
        }
        #endregion regular

        #region event handlers
        private void WorkingDealTickRestDaysHourMinSecHandler(int d, int h, int m, float s)
        {
            if (dealTimeText) dealTimeText.text = "Ends in " +String.Format("{0}d, {1}h, {2}m, {3}sec", d, h, m, s);
        }

        private void WorkingDealTimePassedHandler(double initTime, double realyTime)
        {
            if (dealTimeButton) dealTimeButton.gameObject.SetActive(false);
            if (dealTimeText) dealTimeText.text = "Ends in " + String.Format("{0}d, {1}h, {2}m, {3}sec", 0, 0, 0, 0);
        }

        private void WorkingDealStartHandler()
        {
            if (dealTimeButton) dealTimeButton.gameObject.SetActive(true);
        }

        private void PausedDealStartHandler()
        {
            if (dealTimeButton) dealTimeButton.gameObject.SetActive(false);
            if (dealTimeText) dealTimeText.text = "Ended";//String.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
        }
        #endregion event handlers
    }
}
