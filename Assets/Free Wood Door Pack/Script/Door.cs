using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoorScript
{
	[RequireComponent(typeof(AudioSource))]


public class Door : MonoBehaviour, IInteractable 
	{
		public bool open;
		public float smooth = 1.0f;
		float DoorOpenAngle = -90.0f;
		float DoorCloseAngle = 0.0f;
		public AudioSource asource;
		public AudioClip openDoor,closeDoor;
        public string requiredKey = ""; // leave empty = no key needed

        public string GetInteractText()
        {
            return open ? "close door" : "open door";
        }

        // Use this for initialization
        void Start () 
		{
			asource = GetComponent<AudioSource> ();
		}

        // Update is called once per frame
        void Update()
        {
            if (open)
            {

                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);

            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);

            }
        }

        public void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                PlayerInventory inventory = player.GetComponent<PlayerInventory>();

                if (inventory != null)
                {
                    // Check if key is required
                    if (requiredKey != "" && !inventory.HasKey(requiredKey))
                    {
                        Debug.Log("Door is locked. Need key: " + requiredKey);

                        UIMessage ui = player.GetComponent<PlayerInteractor>().uiMessage;
                        if (ui != null)
                        {
                            ui.ShowNeedItem(requiredKey);
                        }

                        return;
                    }
                }
            }

            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }
    }
}