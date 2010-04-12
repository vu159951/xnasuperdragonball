using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDragonBall.Utils
{
	public delegate void TimerDelegate();
	public struct TimerInstance
	{
		public string sTimerName;
		public event TimerDelegate OnTimer;
		public bool bLooping;
		public bool bRemove;
		public float fTotalTime;
		public float fRemainingTime;
		public int iTriggerCount;

		public void Trigger() { OnTimer(); }
	}

	public class Timer
	{
		#region Fields

		private SortedList<string, TimerInstance> m_kTimers = new SortedList<string, TimerInstance>();
        private double previousUpdateTime = 0.0; 
		
		#endregion

		#region Methods

		/// <summary>
		/// Update is called every frame by the owner of this timer class.
		/// It's responsible for updating every currently registered timer.
		/// If any timer has expired, it is triggered, and based on looping or not 
		/// it may either be removed or restarted
		/// Additionally, iTriggerCount for a timer should be incremented every time it triggers.
		/// </summary>
		/// <param name="gameTime">Only ElasedGameTime is used to update all registered timers</param>
		public void Update(GameTime gameTime)
		{
         //   IList<TimerInstance> temp=m_kTimers.Values;
            for (int i = 0; i < m_kTimers.Values.Count; i++)
            {
                TimerInstance t = m_kTimers.Values[i];
                
                //Console.WriteLine(t.sTimerName+": "+t.fRemainingTime);
                t.fRemainingTime -= (float)gameTime.ElapsedGameTime.Ticks/System.TimeSpan.TicksPerMillisecond;
                //t.fRemainingTime -= 1000;
               // Console.WriteLine(t.sTimerName + ": " + t.fRemainingTime);
                //
                if (t.fRemainingTime <= 0)
                {
                    t.Trigger();
                    t.iTriggerCount++;
                   // Console.WriteLine(t.sTimerName + ": " + t.fRemainingTime);
                    if (t.bLooping)
                    {
                        t.fRemainingTime = t.fTotalTime + t.fRemainingTime;
                    }
                    else
                    {
                        t.bRemove = true;
                    }
                }
                m_kTimers[t.sTimerName] = t;

            }

            for (int i = 0; i < m_kTimers.Values.Count; i++)
            {
                TimerInstance t = m_kTimers.Values[i];
                if (t.bRemove == true)
                {
                    RemoveTimer(t.sTimerName);
                    i--;
                }
            }
            
            previousUpdateTime = gameTime.TotalGameTime.TotalMilliseconds;
            
		}

		/// <summary>
		/// AddTimer will add a new timer provided a timer of the same name does not already exist.
		/// </summary>
		/// <param name="sTimerName">Name of timer to be added</param>
		/// <param name="fTimerDuration">Duration timer should last, in seconds</param>
		/// <param name="Callback">Call back delegate which should be called when the timer expires</param>
		/// <param name="bLooping">Whether the timer should loop infinitely, or should fire once and remove itself</param>
		/// <returns>Returns true if the timer was successfully added, false if it wasn't</returns>
		public bool AddTimer(string sTimerName, float fTimerDuration, TimerDelegate Callback, bool bLooping)
		{
            if (!m_kTimers.ContainsKey(sTimerName))
            {
               // Console.WriteLine(sTimerName);
                TimerInstance t = new TimerInstance();
                t.sTimerName = sTimerName;
                t.fTotalTime = fTimerDuration*1000;
                t.OnTimer += Callback;
                t.bLooping = bLooping;
                t.bRemove = false;
                t.fRemainingTime = fTimerDuration*1000;
                t.iTriggerCount = 0;
                m_kTimers.Add(sTimerName, t);
                return true;
            }
			return false;
		}

		/// <summary>
		/// RemoveTimer removes the timer with the specified name
		/// You must support being able to remove one timer from another timer's callback
		/// (But don't worry about removing the same timer from your callback, 'cause that's confusing)
		/// </summary>
		/// <param name="sTimerName">Name of timer to remove</param>
		/// <returns>True if successfully removed, false if not found</returns>
		public bool RemoveTimer(string sTimerName)
		{
            if (m_kTimers.ContainsKey(sTimerName))
            {
                m_kTimers.Remove(sTimerName);
                return true;
            }
			return false;
		}

		/// <summary>
		/// GetTriggerCount gets the number of times the specified timer has been triggered
		/// </summary>
		/// <param name="sTimerName">Name of timer to get value for</param>
		/// <returns>iTriggerCount if found, otherwise -1</returns>
		public int GetTriggerCount(string sTimerName)
		{
            if (m_kTimers.ContainsKey(sTimerName))
            {
                
                int i= m_kTimers.Values[m_kTimers.IndexOfKey(sTimerName)].iTriggerCount;
                //m_kTimers.TryGetValue(sTimerName, out t);  //
                 return i;
                //return t.iTriggerCount;

            }
			return -1;
		}

		/// <summary>
		/// GetRemainingTime gets the remaining time on the specified timer
		/// </summary>
		/// <param name="sTimerName">Name of timer to get value for</param>
		/// <returns>fRemainingTime if found, otherwise -1.0f</returns>
		public float GetRemainingTime(string sTimerName)
		{
            if (m_kTimers.ContainsKey(sTimerName))
            {
                float i = m_kTimers.Values[m_kTimers.IndexOfKey(sTimerName)].fRemainingTime/1000.0f;
                //m_kTimers.TryGetValue(sTimerName, out t);  //
                return i;
                
            }
			return -1.0f;
		}
		#endregion
	}
}
