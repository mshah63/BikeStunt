using System.Collections;
using UnityEngine;

public class RotatingSpin : BaseSpin
{
    [Header("Rotating Spin Attribute")]
    [Tooltip("Center Circle")]
    public Transform Circle;
    [Tooltip("Tip Nob")]
    public Transform Tick;
    [Tooltip("Number of slices to be done")]
    public int NumberOfSlices = 5;
    [Header("Speed")]
    public float currentSpeed = 0;
    public float acceleration = 1;
    int x = 0;

    bool decelerate;

    int[] angles;

    /// <summary>
    /// Make a spin when user clicked on Spin Button
    /// </summary>
    public override void MakeASpin()
    {
        decelerate = false;
        angles = CalculateAngles(NumberOfSlices);
        base.MakeASpin();
        x = 0;
        // call analytics for reporting to Server
    }

    /// <summary>
    /// calculate Angles by given number of slices
    /// </summary>
    /// <param name="totalSlices">Total Slices</param>
    /// <param name="angle">output angles</param>
    int[] CalculateAngles(int totalSlices)
    {
        int[] angle = new int[totalSlices + 1];
        int currentAngle = 0;
        int additionAngle = 360 / totalSlices;
        angle[0] = currentAngle;
        for (int y = 1; y < angle.Length; y++)
        {
            currentAngle += additionAngle;
            angle[y] = currentAngle;
        }
        return angle;
    }

    /// <summary>
    /// Coroutine use to make an animated spin
    /// </summary>
    /// <returns></returns>
    public override IEnumerator DoSpin()
    {
        while (makeASpin)
        {
            time = Time.deltaTime;
            if (!decelerate)
                currentSpeed += acceleration * Time.deltaTime;
            else
                currentSpeed -= acceleration * Time.deltaTime;
            Circle.rotation *= Quaternion.Euler(Vector3.forward * Time.deltaTime * currentSpeed);
            yield return new WaitForSeconds(time);
            if (randTime / 2 <= cacheTime)
            {
                decelerate = true;
            }
        }
        yield return new WaitForSeconds(1.0f);
        float currentAngle = Circle.rotation.eulerAngles.z;
        for (int index = 1; index < angles.Length; index++)
        {
            if (currentAngle > angles[index - 1] && currentAngle < angles[index])
            {
                x = index - 1;
            }
        }
        OnSpinCompleted.Invoke(x);
    }
}
