using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrationScript : MonoBehaviour
{

    string startMessage = "Well, it looks like you've died. Before you can enter the afterlife, you must complete a challenge. There are three things you must collect, though not in a normal sense.";
    string startMessage2 = "Unlike how things were in the land of the living, you must seek out things to kill you again.";
    string startMessage3 = "This test is prevent souls from becoming ghosts. Instead of being stuck in the living world, haunting innocents, you're here until you accept your death.";

    string spikeDeath = "Congratulations! you found the spikes. to help overcome some other obstacles, I'll give you the power to fall slower than normal";

     string worldBorderDeath = "You found the border! Although you aren't able to escape that way, I commend you. I'll give you an extra jump while you're in the air";

     string creatureDeath = "Congratulations on overcoming your fear of being eaten. I'll give you the speed you didn't have in life";

     string closeToEnd = "Hmmm. The Exit's right over there, I wonder how to get to it.";

    string allAbilities = "You should have everything you need to make it there.";

     string missingAbilities = "You still need to do one of the following:";

     string missingSpeed = "Find a creature to kill you (check near the beginning)";
     string missingJump = "Find the edge of the world (you may need to find the spikes and creature first)";
     string missingFloat = "Find spikes to jump on (try and find a hidden area under a platform)";

    private string message;

    private PlayerController player;
    private TextMeshProUGUI narrationText;
    private GameObject narrationPanel;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        narrationPanel = GameObject.FindGameObjectWithTag("Narration");
        narrationText = TextMeshProUGUI.FindObjectOfType<TextMeshProUGUI>();

        narrationPanel.SetActive(false);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            message = "";

            if(this.name == "Narration1")
            {
                message += closeToEnd;
            }

            if (player.power_Speed && player.power_Float && player.power_DJump)
            {
                message += allAbilities;
            }
            else
            {
                if (!player.power_Speed)
                {
                    message += "\n" +missingSpeed;
                }

                if (!player.power_Float)
                {
                    message += "\n" + missingFloat;
                }

                if (!player.power_DJump)
                {
                    message += "\n" + missingJump;
                }
            }

            narrationText.text = message;
            narrationPanel.SetActive(true);
        }
    }
}
