using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangerController : MonoBehaviour
{
    public Animator animator;

    private int sceneToLoad;

    public void FadeToScene(int indexLevel)
    {
        sceneToLoad = indexLevel;
        animator.SetTrigger("FadeOut");
    }

    public void FaceToNextScene() =>
        FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
