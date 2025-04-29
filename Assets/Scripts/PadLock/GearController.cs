using UnityEngine;

public class GearController : MonoBehaviour
{

    public bool isTurning;

    public void Turn()
    {
        if (isTurning) return;

        isTurning = true;

        float timer = 0;
        Vector3 startRot = transform.localRotation.eulerAngles;
        Vector3 targetRot = transform.localRotation.eulerAngles;
        targetRot.y += 36;

        while(timer < 1)
        {
            timer += Time.deltaTime;
            float t = 1 / timer;

            transform.localEulerAngles = Vector3.Lerp(startRot, targetRot, t);

        }

        isTurning = false;
    }
}
