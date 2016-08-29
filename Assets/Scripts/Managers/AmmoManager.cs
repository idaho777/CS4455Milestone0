using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoManager : MonoBehaviour {
    public static int ammo;
    public static int maxAmmo;

    Text text;
    
	void Start () {
        text = GetComponent <Text> ();
    }
	
	void Update ()
    {
        text.text = "Ammo: " + ammo + "/" + maxAmmo + " (Reload : r)";
    }
}
