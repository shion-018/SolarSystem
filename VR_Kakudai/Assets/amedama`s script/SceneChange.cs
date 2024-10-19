
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{


    [HideInInspector][SerializeField] private string sceneToLoad;

#if UNITY_EDITOR
    [SerializeField] SceneAsset ChangeScene;//ˆÚ“®‚·‚éScene‚ðŒˆ’è
#endif


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ChangeScene != null)
        {
            sceneToLoad = ChangeScene.name;
        }
    }
#endif

    public void SceneChange_func()
    {

        SceneManager.LoadScene(sceneToLoad);


    }   
}
