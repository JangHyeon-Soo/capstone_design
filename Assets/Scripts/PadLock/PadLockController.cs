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

    // Update is called once per frame
    void Update()
    {
        if(Lock)
        {
            firstGearVal = Mathf.RoundToInt((FirstGear.localRotation.eulerAngles.y % 360) / 36);
            secondGearVal = Mathf.RoundToInt((SecondGear.localRotation.eulerAngles.y % 360) / 36);
            thirdGearVal = Mathf.RoundToInt((ThirdGear.localRotation.eulerAngles.y % 360) / 36);

            currentValue = firstGearVal * 100 + secondGearVal * 10 + thirdGearVal;
            

            if(currentValue == Password)
            {
                Lock = false;
            }
        }

        else
        {
            gameObject.AddComponent<Rigidbody>();

            transform.localEulerAngles = new Vector3(Random.Range(25, 60), Random.Range(20, 60), Random.Range(20, 60));
            gameObject.layer = 2;

            this.enabled = false;


        }



    }
}
