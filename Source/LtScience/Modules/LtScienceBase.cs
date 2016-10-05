﻿using LtScience.InternalObjects;
using System;

namespace LtScience.Modules
{
    public class LtScienceBase : ModuleScienceExperiment
    {
        private int _resource;

        protected static bool CheckBoring(Vessel vessel, bool msg = false)
        {
            if ((vessel.orbit.referenceBody.name == "Kerbin") && (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.PRELAUNCH || vessel.situation == Vessel.Situations.SPLASHED || vessel.altitude <= vessel.orbit.referenceBody.atmosphereDepth))
            {
                if (msg)
                    Util.DisplayScreenMsg("Too boring here. Go to space!");

                return true;
            }

            return false;
        }

        // Simple system to prevent some experiments from being run in places they can't be ran.
        protected static bool CanRunExperiment(Vessel vessel, SkyLabExperimentData node, ref string msg)
        {
            msg = "Vessel must be (one of): ";

            if (node.Landed)
            {
                if (vessel.situation == Vessel.Situations.LANDED)
                    return true;

                msg += "landed, ";
            }

            if (node.Splashed)
            {
                if (vessel.situation == Vessel.Situations.SPLASHED)
                    return true;

                msg += "splashed down, ";
            }

            if (node.Flying)
            {
                if (vessel.altitude <= vessel.orbit.referenceBody.atmosphereDepth)
                    return true;

                msg += "flying, ";
            }

            if (node.Space)
            {
                if (vessel.altitude > vessel.orbit.referenceBody.atmosphereDepth)
                    return true;

                msg += "in space, ";
            }

            msg = msg.Remove(msg.Length - 2);
            return false;
        }

        private PartResource GetResource(string res)
        {
            PartResourceList resourceList = part.Resources;

            return resourceList.Get(res);
        }

        public int GetResourceAmount(string res)
        {
            PartResource resource = GetResource(res);

            if (resource == null)
                return 0;

            return (int)Math.Floor(resource.amount);
        }

        // Checks if a craft has an amount of resources without actually taking them.
        public bool LtsHasResources(string res, int amount)
        {
            _resource = (int)part.RequestResource(res, amount);
            part.RequestResource(res, -(_resource));
            return (_resource == amount);
        }

        // Some ease function I guess... whatever
        protected bool LtsUseResources(string res, int amount)
        {
            _resource = (int)part.RequestResource(res, amount);

            if (_resource == amount)
                return true;

            part.RequestResource(res, -(_resource));
            return false;
        }
    }
}
