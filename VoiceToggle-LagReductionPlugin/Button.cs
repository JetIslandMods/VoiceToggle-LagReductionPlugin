using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

namespace VoiceToggle_LagReductionPlugin
{
    public class Button
    {

        GameObject buttonObj;
        GameObject buttonPrimitive;
        MeshRenderer buttonPrimitiveRenderer;
        TextMeshPro buttonTextMesh;
        float buttonTimer = 0f;

        public bool isButtonOn = false;
        public bool buttonPressed = false;

        public bool isButtonHidden = false;

        public bool debugButton = false;

        private bool _isToggleButton = false;
        private Vector3 _position;

        public Button(string text, Vector3 position, Vector3 size, float fontSize, bool isToggleButton)
        {
            buttonObj = new GameObject(text + "_button");
            buttonObj.transform.localPosition = position;

            buttonPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);

            buttonPrimitiveRenderer = buttonPrimitive.GetComponent<MeshRenderer>();
            buttonPrimitiveRenderer.material.color = Color.blue;
            Shader holo = Shader.Find("Zololgo/Sci-Fi Hologram");
            buttonPrimitiveRenderer.material.shader = holo;
            BoxCollider buttonPrimitiveCollider = buttonPrimitive.GetComponent<BoxCollider>();
            buttonPrimitiveCollider.enabled = false;

            buttonTextMesh = buttonObj.AddComponent<TextMeshPro>();
            buttonTextMesh.text = text;
            buttonTextMesh.font = PlayerBody.localPlayer.pauseMenu.statsTextHookshotLength.font;
            buttonTextMesh.color = Color.white;
            buttonTextMesh.fontSize = fontSize;
            buttonTextMesh.alignment = TextAlignmentOptions.Center;
            buttonTextMesh.ForceMeshUpdate();
            buttonTextMesh.transform.Rotate(new Vector3(0f, 180f, 0f));

            buttonTextMesh.transform.localPosition = position;
            buttonPrimitive.transform.localPosition = position;
            buttonPrimitive.transform.localScale = size; //buttonTextMesh.textBounds.size + new Vector3(0.01f, 0.006f, 0.015f);

            buttonTextMesh.transform.SetParent(buttonObj.transform);
            buttonPrimitive.transform.SetParent(buttonObj.transform);

            //buttonPrimitive.transform.localPosition = new Vector3(0f, 0f, 0f); // position;
            //buttonTextMesh.transform.localPosition = new Vector3(0f, 0f, 0f); // position;
            buttonPrimitive.transform.localPosition += new Vector3(0, 0, 0.009f);

            buttonObj.transform.Rotate(30f, 0f, 0f);

            _position = position;
            _isToggleButton = isToggleButton;
        }

        public void setButtonToggleState(bool state)
        {
            if(state)
                buttonPrimitiveRenderer.material.color = Color.magenta;
            else
                buttonPrimitiveRenderer.material.color = Color.blue;
        }

        public void Update(PlayerBody me)
        {
            if (me.pauseMenu.menuParent && me.pauseMenu.menuParent.gameObject.activeInHierarchy && me.pauseMenu.hologramMenuController.currentMenu == HologramButtonScript.Menu.MainMenu && !isButtonHidden)
            {
                buttonObj.SetActive(true);
                buttonObj.transform.SetParent(me.pauseMenu.menuParent);
                if(!debugButton)
                    buttonObj.transform.localPosition = _position;
                Transform leftFinger = me.hands.leftPointerFingerTip;
                Transform rightFinger = me.hands.rightPointerFingerTip;

                buttonTimer += Time.deltaTime;
                if (buttonPrimitiveRenderer.bounds.Contains(leftFinger.transform.position) || buttonPrimitiveRenderer.bounds.Contains(rightFinger.transform.position))
                {
                    if (debugButton)
                    {
                        if(me.input.rightTriggerPulled)
                        {
                            buttonObj.transform.position = rightFinger.transform.position;
                            Console.WriteLine("POS: " + buttonObj.transform.localPosition.ToString("G4"));
                            Console.WriteLine("SCL: " + buttonPrimitive.transform.localScale.ToString("G4"));
                        }
                    }

                    if (!buttonPressed && buttonTimer > 0.6f)
                    {
                        AudioSource.PlayClipAtPoint(me.pauseMenu.hologramMenuController.MenuSettings.pressSound, me.pauseMenu.menuParent.transform.position);
                        buttonPressed = true;
                        buttonTimer = 0f;
                        if(_isToggleButton)
                            isButtonOn = !isButtonOn;
                        if(isButtonOn)
                        {
                            buttonPrimitiveRenderer.material.color = Color.magenta;
                        }
                        else
                        {
                            buttonPrimitiveRenderer.material.color = Color.blue;
                        }
                    }
                    else
                    {
                        buttonPressed = false;
                    }
                }
                else
                {
                    buttonPressed = false;
                }
            }
            else
            {
                buttonObj.SetActive(false);
            }
        }

    }
}
