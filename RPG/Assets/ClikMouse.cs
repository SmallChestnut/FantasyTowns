using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;


public class CubeData
{
    public List<Vector3> cubes = new List<Vector3>();
    public List<Quaternion> rota = new List<Quaternion>(); 
}



public class ClikMouse : MonoBehaviour
{
    public GameObject cube;
    private CubeData cubes = new CubeData();
    private List<GameObject> temps = new List<GameObject>();
    void Start()
    {
        string path = Application.dataPath + "\\save.xml";
        XmlSerializer xml = new XmlSerializer(cubes.GetType());
        if (!File.Exists(path))
        {
            return;
        }
        using (FileStream file = File.Open(path, FileMode.Open))
            cubes = xml.Deserialize(file) as CubeData;
        for(int i = 0; i < cubes.cubes.Count; i++)
        {
            GameObject temp = Instantiate(cube);
            temps.Add(temp);
            temp.transform.position = cubes.cubes[i];
            temp.transform.rotation = cubes.rota[i];
        }
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject temp = Instantiate(cube);
            temp.transform.position = new Vector3(0, 0, 0);
            temps.Add(temp);
        }
    }

    public void Save()
    {
        cubes.cubes.Clear();
        foreach(var x in temps)
        {
            cubes.cubes.Add(x.transform.position);
            cubes.rota.Add(x.transform.rotation);
        }
        XmlSerializer xml = new XmlSerializer(cubes.GetType());
        using (FileStream fs = File.Open(Application.dataPath + "\\save.xml", FileMode.Create)) 
        {
            xml.Serialize(fs, cubes);
            Debug.Log(Application.dataPath + "\\save.xml");
        }
        
    }
}
