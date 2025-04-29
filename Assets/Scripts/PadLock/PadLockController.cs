using UnityEngine;

public class PadLockController : MonoBehaviour
{
    public bool Lock = true;
    public int Password;

    public Transform camPosition;

    public Transform FirstGear;
    public Transform SecondGear;
    public Transform ThirdGear;

    public int firstGearVal, secondGearVal, thirdGearVal;
    public int currentValue = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Lock)
        {
            firstGearVal = Mathf.RoundToInt((FirstGear.localRotation.eulerAngles.y % 360) / 36);
            secondGearVal = Mathf.RoundToInt((SecondGear.localRotation.eulerAngles.y % 360) / 36);
            thirdGearVal = Mathf.RoundToInt((ThirdGear.localRotation.eulerAngles.y % 360) / 36);

            currentValue = firstGearVal * 100 + secondGearVal * 10 + thirdGearVal;
            Debug.Log(currentValue);

            if(currentValue == Password)
            {
                Lock = false;
            }
        }



    }
}
