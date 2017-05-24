using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouldProjectAzureV2
{
    public class UserSettings
    {
        private String userId;
        private int accelerometerOnOff;
        private int proximityOnOff;
        private int lightOnOff;
        private int samlingRate;

        public UserSettings(String userId, int accelerometerOnOff, int proximityOnOff, int lightOnOff, int samplingRate)
        {
            this.userId = userId;
            this.accelerometerOnOff = accelerometerOnOff;
            this.proximityOnOff = proximityOnOff;
            this.lightOnOff = lightOnOff;
            this.samlingRate = samplingRate;
        }
        public String getUserId()
        {
            return userId;
        }
        public int getAcceleroMeterOnoff()
        {
            return accelerometerOnOff;
        }
        public int getProximityOnoff()
        {
            return proximityOnOff;
        }
        public int getLightOnOff()
        {
            return lightOnOff;
        }
        public int getSamplingRate()
        {
            return samlingRate;
        }
    }
}