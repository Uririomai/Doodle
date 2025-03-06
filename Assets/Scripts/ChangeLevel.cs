using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public GameObject FadeIn;
    public int numScene;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();

        if (character)
            Finish();
    }

    public void SceneChange()
    {
        SceneManager.LoadScene(numScene);
    }

    public void Finish()
    {
        FadeIn.SetActive(true);
    }
}
