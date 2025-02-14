using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreserveObject : MonoBehaviour
{
    public static List<string> singletons = new List<string>();

    private void Awake()
    {
        string gameObjectIdentifier = gameObject.name;
        if (!singletons.Contains(gameObjectIdentifier))
        {
            singletons.Add(gameObjectIdentifier);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;

    }

    private void ChangedActiveScene(Scene current, Scene next)
    {

        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
