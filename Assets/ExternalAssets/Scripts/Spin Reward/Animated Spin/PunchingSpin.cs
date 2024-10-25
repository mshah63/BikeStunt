using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PunchingSpin : BaseSpin
{
    [Header("Punch Spin Attribute")]
    public Transform[] Hexas;
    int x = 0;

    /// <summary>
    /// Make a spin when user clicked on Spin Button
    /// </summary>
    public override void MakeASpin()
    {
        base.MakeASpin();
        x = 0;
        // call analytics for reporting to Server
    }

    /// <summary>
    /// Coroutine use to make an animated spin
    /// </summary>
    /// <returns></returns>
    public override IEnumerator DoSpin()
    {
        while (makeASpin)
        {
            Hexas[x].DOPunchScale(Vector3.one * 0.5f, 0.15f);
            yield return new WaitForSeconds(time);
            x += 1;
            x = x % Hexas.Length;
        }

        OnSpinCompleted.Invoke(x);
    }
}
