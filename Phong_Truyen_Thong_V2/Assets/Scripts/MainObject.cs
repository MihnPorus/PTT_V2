using UnityEngine;
using System.Collections;

public class MainObject : MonoBehaviour {

    public Objects[] objects;

    //int currentIndex;

    public static MainObject intance;

    void Start()
    {
        if (intance != null)
        {
            DestroyObject(gameObject);
        }
        else
        {
            intance = this;
        }
    }

    [System.Serializable]
	public class Objects
    {
        public bool isView = false;
        public GameObject viewHold;
        public string objID;
        public Transform objPointHold;
        public int currentIndex;

    }
}
