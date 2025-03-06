using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
    [SerializeField]
    private AudioSource collectSound;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();

        if (character && character.Lives < 5)
        {
            character.Lives++;
            collectSound.Play();
           StartCoroutine(WaitAndPrint());
        }
    }
    private IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(0.2F);
        Destroy(gameObject);
    }
}
