﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using GameObject;
using SS13.IoC;
using SS13_Shared;
using SS13_Shared.GO;
using SS13_Shared.GO.Component.Light;
using SS13_Shared.GO.Component.Particles;
using ServerInterfaces.Chat;

namespace SGO
{
    public class ParticleSystemComponent : Component
    {
        private Dictionary<string, Boolean> emitters = new Dictionary<string, bool>();

        public ParticleSystemComponent()
        {
            Family = ComponentFamily.Particles;
        }

        public override ComponentReplyMessage RecieveMessage(object sender, ComponentMessageType type,
                                                             params object[] list)
        {
            ComponentReplyMessage reply = base.RecieveMessage(sender, type, list);

            if (sender == this)
                return reply;

            switch (type)
            {
                case ComponentMessageType.Activate:
                    HandleClickedInHand();
                    break;
            }

            return reply;
        }

        private void HandleClickedInHand() //TODO: This is really dumb. Change this !!!
        {
            foreach (KeyValuePair<string, bool> emitter in new Dictionary<string, bool>(emitters)) //to work around "collection modified" crap.
                emitters[emitter.Key] = !emitters[emitter.Key];
        }

        public override ComponentState GetComponentState()
        {
            return new ParticleSystemComponentState(emitters);
        }

        public override void HandleExtendedParameters(XElement extendedParameters)
        {
            foreach (XElement param in extendedParameters.DescendantNodes())
            {
                if(param.Name == "ParticleSystem")
                    if (param.Attribute("name") != null)
                    {
                        if (!emitters.ContainsKey(param.Attribute("name").Value))
                        {
                            if (param.Attribute("active") != null)
                            {
                                emitters.Add(param.Attribute("name").Value, bool.Parse(param.Attribute("active").Value));
                            }
                            else
                            {
                                emitters.Add(param.Attribute("name").Value, true);
                            }
                        }
                    }
            }
        }
    }
}