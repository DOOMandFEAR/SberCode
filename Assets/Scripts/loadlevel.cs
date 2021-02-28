using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("sph"))
        {
            SceneManager.LoadScene("temple");
        }
    }
    // Update is called once per frame
    void Update()
    {
        cube.transform.position=new Vector3(sphere.transform.position.x,sphere.transform.position.y-3f,sphere.transform.position.z);
    }
}
