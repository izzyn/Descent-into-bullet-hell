using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hpAndLives : MonoBehaviour
{
    public string sceneName;
    public string unLoadScene;
    public float HP;
    public int Lives;
    public bool invincible;
    public displayHPandLives displayUpdater;
    float startingHP;
    int startingLives;
    int invincibilityStack;

    // Start is called before the first frame update
    void Start()
    {
        startingLives = Lives;
        startingHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            if(Lives > 0)
            {
                HP = startingHP;
                Lives--;
                displayUpdater.updateThing();
                StartCoroutine(beInvincible(2f));
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }

        }
    }
    public IEnumerator beInvincible(float time)
    {
        invincibilityStack++;
        invincible = true;
        yield return new WaitForSeconds(time);
        invincibilityStack--;
        if(invincibilityStack == 0)
        {
            invincible = false;
        }
    }
}
