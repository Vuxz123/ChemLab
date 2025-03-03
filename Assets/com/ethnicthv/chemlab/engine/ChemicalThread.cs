using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace com.ethnicthv.chemlab.engine
{
    public class ChemicalThread : MonoBehaviour
    {
        private bool _running;
        private Coroutine _coroutine;
        
        public void StartTick()
        {
            _running = true;
            _coroutine = StartCoroutine(TickCoroutine());
        }

        public void Stop()
        {
            _running = false;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        
        private IEnumerator TickCoroutine()
        {
            while (_running)
            {
                try {
                    PerformChemicalUpdates();
                } catch (Exception e) {
                    Debug.LogError("Error in chemical thread: " + e.Message + "\n" + e.StackTrace);
                    throw;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        private static void PerformChemicalUpdates()
        {
            ChemicalTickerHandler.TickAll();
        }
    }
}