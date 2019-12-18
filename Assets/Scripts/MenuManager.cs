using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> buttons_0;
    public List<GameObject> buttons_1;
    public List<List<GameObject>> menu_buttons = new List<List<GameObject>>();
    private float button_x;

    // Start is called before the first frame update
    void Start()
    {
        menu_buttons.Add(buttons_0);
        menu_buttons.Add(buttons_1);

        button_x = buttons_0[0].transform.position.x;
    }

    // Ease out UI Items to the right of screen
    IEnumerator EaseOut(int _state_from, int _state_to)
    {
        float initial_pos = menu_buttons[_state_from][0].transform.position.x;
        float initial_time = Time.realtimeSinceStartup;

        // While final botton is 600 units to the right
        while (menu_buttons[_state_from][menu_buttons[_state_from].Count - 1].transform.position.x
            < initial_pos + 600.0f)
        {
            // For each button in set
            for (int i = 0; i < menu_buttons[_state_from].Count; i++)
            {
                // Stagger button movement
                if (i == 0 || menu_buttons[_state_from][i - 1].transform.position.x > initial_pos + 250.0f)
                {                  
                    menu_buttons[_state_from][i].transform.position += Vector3.right * 30.0f *
                        Easing.Exponential.Out(Time.realtimeSinceStartup - initial_time);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        // Unload exiting group
        foreach (GameObject btn in menu_buttons[_state_from])
        {
            btn.SetActive(false);
            btn.transform.position = new Vector3(button_x, btn.transform.position.y, btn.transform.position.z);
        }

        // Load entering group
        foreach (GameObject btn in menu_buttons[_state_to])
        {
            btn.SetActive(true);
        }
        yield return new WaitForSeconds(0.0f);
    }

    // MAIN MENU BUTTON EVENTS
    public void ProjectsPressed()
    {
        StartCoroutine(EaseOut(0, 1));
    }
    public void SettingsPressed()
    {

    }
    public void ExitPressed()
    {
        Application.Quit();
    }

    public void StartPressed()
    {
        SceneManager.LoadScene("Editor");
    }
    public void BackPressed()
    {
        StartCoroutine(EaseOut(1, 0));
    }
}
