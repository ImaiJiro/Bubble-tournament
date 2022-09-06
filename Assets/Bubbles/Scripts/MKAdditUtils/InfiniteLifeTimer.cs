using System;
using UnityEngine;

namespace Mkey
{
    public class InfiniteLifeTimer : MonoBehaviour
    {
        #region temp vars
        private GlobalTimer gTimer;
        private string lifeInfTimerName = "lifeinfinite";
        private bool debug = false;
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        public static InfiniteLifeTimer Instance;
        #endregion temp vars

        #region properties
        public float RestDays { get; private set; }
        public float RestHours { get; private set; }
        public float RestMinutes { get; private set; }
        public float RestSeconds { get; private set; }
        public bool IsWork { get; private set; }
        #endregion properties

        #region regular
        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            IsWork = false;
            MPlayer.StartInfiniteLifeEvent += StartInfiniteLifeHandler;
            MPlayer.EndInfiniteLifeEvent += EndInfiniteLifeHandler;
            if (MPlayer.HasInfiniteLife())
            {
                StartNewTimer();
            }
        }

        void Update()
        {
            if (IsWork)
                gTimer.Update();
        }

        private void OnDestroy()
        {
            if (MPlayer)
            {
                MPlayer.StartInfiniteLifeEvent -= StartInfiniteLifeHandler;
                MPlayer.EndInfiniteLifeEvent -= EndInfiniteLifeHandler;
            }
        }
        #endregion regular

        #region timerhandlers
        private void TickRestDaysHourMinSecHandler(int d, int h, int m, float s)
        {
            RestDays = d;
            RestHours = h;
            RestMinutes = m;
            RestSeconds = s;
        }

        private void TimePassedHandler(double initTime, double realTime)
        {
            IsWork = false;
            MPlayer.EndInfiniteLife();
        }
        #endregion timerhandlers

        #region player life handlers
        private void StartInfiniteLifeHandler()
        {
            StartNewTimer();
        }

        private void EndInfiniteLifeHandler()
        {
            IsWork = false;
        }
        #endregion player life handlers

        private void StartNewTimer()
        {
            if (debug) Debug.Log("start new");
            TimeSpan ts = MPlayer.GetInfLifeTimeRest();
            gTimer = new GlobalTimer(lifeInfTimerName, ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            gTimer.TickRestDaysHourMinSecEvent += TickRestDaysHourMinSecHandler;
            gTimer.TimePassedEvent += TimePassedHandler;
            IsWork = true;
        }

        private void StartExistingTimer()
        {
            gTimer = new GlobalTimer(lifeInfTimerName);
            gTimer.TickRestDaysHourMinSecEvent += TickRestDaysHourMinSecHandler;
            gTimer.TimePassedEvent += TimePassedHandler;
            IsWork = true;
        }

    }
}