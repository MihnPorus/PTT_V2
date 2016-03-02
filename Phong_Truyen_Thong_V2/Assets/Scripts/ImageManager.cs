using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour {

    public ImageGroup[] imageGroups;

    Dictionary<string, Sprite[]> groupDirtinoary = new Dictionary<string, Sprite[]>();

    public static ImageManager intance;

    void Awake()
    {
        if (intance != null)
        {
            Destroy(gameObject);
        }
        intance = this;
        DontDestroyOnLoad(gameObject);
        foreach(ImageGroup imagegroup in imageGroups)
        {
            groupDirtinoary.Add(imagegroup.groupID,imagegroup.group);
        }
    }

    public Sprite[] GetSpritesFormName(string name)
    {
        if (groupDirtinoary.ContainsKey(name))
        {
            Sprite[] sprites = groupDirtinoary[name];
            return sprites;
        }
        return null;
    }

	[System.Serializable]
    public class ImageGroup
    {
        public string groupID;
        public Sprite[] group;
    }
}
