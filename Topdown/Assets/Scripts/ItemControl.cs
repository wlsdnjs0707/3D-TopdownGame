using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    private GunControl gunControl;

    public event System.Action SelectFinished;

    // Start is called before the first frame update
    void Start()
    {
        gunControl = GetComponent<GunControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipTestGun()
    {
        gunControl.TestGun();
        Finished();
    }

    void Finished()
    {
        if (SelectFinished != null)
        {
            SelectFinished();
        }
    }
}
