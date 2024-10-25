using System.Collections;
using UnityEngine;

public abstract class BaseSpin : MonoBehaviour
{
    [Range(0.1f, 8f)]
    public float minTime = 1, maxTime = 2;
    public float time = 0.5f;

    protected float randTime;
    protected float cacheTime;

    protected bool makeASpin;

    /// <summary>
    /// delegate calls when spin completed
    /// </summary>
    /// <param name="index">the stopping number</param>
    public delegate void OnSpinComplete(int index);
    public OnSpinComplete OnSpinCompleted;

    /// <summary>
    /// Make a spin when user clicked on Spin Button
    /// </summary>
    public virtual void MakeASpin()
    {
        randTime = UnityEngine.Random.Range(minTime, maxTime);
        makeASpin = true;
        cacheTime = 0;
        StartCoroutine(DoSpin());
    }

    /// <summary>
    /// calls on every frame
    /// </summary>
    private void Update()
    {
        if (makeASpin)
        {
            cacheTime += Time.deltaTime;
            if (cacheTime >= randTime)
            {
                makeASpin = false;
                cacheTime = 0;
            }
        }
    }

    /// <summary>
    /// Coroutine use to make an animated spin
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator DoSpin();
}
