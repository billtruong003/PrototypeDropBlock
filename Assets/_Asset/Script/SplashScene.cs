using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using BillUtils.TimeUtilities;
public class SplashScene : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScenePlay());
    }

    private IEnumerator LoadScenePlay()
    {
        yield return TimeUtils.WaitTwoSec;
        SceneManager.LoadScene("PlayScene");
    }
}
