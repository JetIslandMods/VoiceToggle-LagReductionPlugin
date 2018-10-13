using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IllusionPlugin;
using UnityEngine;

namespace VoiceToggle_LagReductionPlugin
{
    public class Plugin : IPlugin
    {

        int currentLevelId = 0;

        Button voiceToggleButton = null;

        public void createVoiceButton()
        {
            try
            {
                if (voiceToggleButton != null)
                    return;
                else
                {
                    voiceToggleButton = new Button("Voice Mute", new Vector3(0.03711f, -0.09566f, 0.01281f), new Vector3(0.1805f, 0.03535f, 0.015f), 0.2f, true);
                }
            }
            catch(Exception e)
            {

            }

        }

        public string Name
        {
            get { return "Voice Mute / Lag Reduction"; }
        }
        public string Version
        {
            get { return "0.1"; }
        }

        public void OnApplicationQuit()
        {
        }

        public void OnApplicationStart()
        {
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLevelWasInitialized(int level)
        {
            currentLevelId = level;
            if(level == 1)
            {
            }
            else
            {
                if(voiceToggleButton != null)
                {
                    voiceToggleButton = null;
                }
            }
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnUpdate()
        {
            if(currentLevelId == 1)
            {
                try
                {
                    if (StartGameScript.playingOnline)
                    {
                        PlayerBody me = PlayerBody.localPlayer;
                        if (voiceToggleButton == null)
                            createVoiceButton();

                        if (me != null)
                            voiceToggleButton.Update(me);

                        if (voiceToggleButton.buttonPressed)
                        {
                            if (voiceToggleButton.isButtonOn)
                                PhotonVoiceNetwork.Disconnect();
                            else
                                PhotonVoiceNetwork.Connect();

                            me.DisplayMessageInFrontOfPlayer("Voice  is  " + (voiceToggleButton.isButtonOn ? "off" : "on"));
                        }
                    }
                }
                catch(Exception e)
                {

                }
            }
        }
    }
}
