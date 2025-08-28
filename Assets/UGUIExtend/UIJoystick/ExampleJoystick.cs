using Hukiry.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleJoystick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        JoystickInput.ins.SetPlayer(this.transform);
    }

}
