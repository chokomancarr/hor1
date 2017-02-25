using UnityEngine;
using System.Collections.Generic;

public class InKeys : MonoBehaviour {
    public static bool isJoystick;

    static Dictionary<string, InKeyPair> keys;

	void Awake () {
        keys = new Dictionary<string, InKeyPair>();
        isJoystick = false;
        keys.Add("E", new InKeyPair("E", "▢", KeyCode.E, KeyCode.Joystick1Button0));
        keys.Add("C", new InKeyPair("C", "✕", KeyCode.C, KeyCode.Joystick1Button1));
        keys.Add("F", new InKeyPair("F", "◯", KeyCode.F, KeyCode.Joystick1Button2));
        keys.Add("Shift", new InKeyPair("⇑Shift", "L2", KeyCode.LeftShift, KeyCode.Joystick1Button6));
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.J))
            isJoystick = !isJoystick;
    }

    public static string Nm(string s)
    {
        return keys[s].nm;
    }
    public static KeyCode Key (string s)
    {
        return keys[s].btn;
    }

    class InKeyPair {
        public InKeyPair(string n1, string n2, KeyCode b1, KeyCode b2)
        {
            nmKbd = n1;
            nmJys = n2;
            btnKbd = b1;
            btnJys = b2;
        }

        public string nm
        {
            get { return isJoystick ? nmJys : nmKbd; }
        }
        public KeyCode btn
        {
            get { return isJoystick ? btnJys : btnKbd; }
        }

        public string nmKbd, nmJys;
        public KeyCode btnKbd, btnJys;

    }
}
