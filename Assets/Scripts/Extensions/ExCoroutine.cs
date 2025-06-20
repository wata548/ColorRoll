using System;
using System.Collections;
using UnityEngine;

namespace Extensions {
    public static class ExCoroutine {
        public static IEnumerator WaitStart(float sec, Action process) {
            yield return new WaitForSeconds(sec);
            process?.Invoke();
        }
    }
}