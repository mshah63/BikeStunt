using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimTrigger : MonoBehaviour
{
    public GameObject WinPrefabe ;
    public GameObject PlayerWithBike, BikerMan;
    public Vector3 lastRespawnPointpos;
    public Quaternion lastspawnrot;
    public static WinAnimTrigger instance;
    [HideInInspector]
    public bool _isCollideWithMidPoint;
    // public GameObject L1, L2, L3, L4, L5, L6, L7, L8, L9,L10;
    //public GameObject Anim1, Anim2, Anim3, Anim4, Anim5, Anim6, Anim7, Anim8, Anim9, Anim10;  
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        PlayerWithBike = GameObject.FindGameObjectWithTag("Player");
        BikerMan = GameObject.FindGameObjectWithTag("Bikeman");
        
       WinPrefabe = GameObject.FindGameObjectWithTag("anim");
        if (WinPrefabe.activeInHierarchy)
        {
            WinPrefabe.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.name== "finishPoint")
        {
            if (WinPrefabe)
                WinPrefabe.SetActive(true);
            print("animation");
        }
        if (other.gameObject.name == "Stop Point")
        {
            PlayerWithBike.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            BikerMan.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            print("osho alaka");

        }
        if (other.gameObject.CompareTag("respawnpoints"))
        {
            BikeControl.instance._iscollideWithHalfPoint = true;
            Debug.Log("collide with respawn point");
            lastRespawnPointpos = other.transform.position;
            lastspawnrot = other.transform.rotation;
            _isCollideWithMidPoint = true;
        }
       
    }
}
