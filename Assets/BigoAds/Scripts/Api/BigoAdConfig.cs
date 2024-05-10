using System;
using System.Collections.Generic;
using BigoAds.Scripts.Api.Constant;
using System.Collections.Generic;

namespace BigoAds.Scripts.Api
{
    public class BigoAdConfig
    {
        public const string EXTRA_KEY_HOST_RULES = "host_rules";

        /// <summary>
        /// the unique identifier of the App
        /// </summary>
        internal string AppId { get; }

        /// <summary>
        /// Custom set the debugLog to print debug Log.
        /// debugLog NO: close debug log, YES: open debug log.
        /// </summary>
        internal bool DebugLog { get; }

        /// <summary>
        /// Channels for publishing media applications
        /// </summary>
        internal string Channel { get; }

        internal int Age { get; }

        internal int Gender { get; }

        internal long ActivatedTime { get; }

        internal Dictionary<string, string> ExtraDictionary { get; }

        private BigoAdConfig(BigoAdConfig.Builder builder)
        {
            AppId = builder.AppId;
            DebugLog = builder.DebugLog;
            Channel = builder.Channel;
            Age = builder.Age;
            Gender = (int)builder.Gender;
            ActivatedTime = builder.ActivatedTime;
            ExtraDictionary = builder.ExtraDictionary;
        }

        public class Builder
        {
            internal string AppId;

            internal bool DebugLog;

            internal string Channel;

            internal int Age;

            internal BGAdGender Gender;

            internal long ActivatedTime;

            internal Dictionary<string, string> ExtraDictionary = new Dictionary<string, string>();

            public Builder SetAppId(string appid)
            {
                this.AppId = appid;
                return this;
            }

            public Builder SetDebugLog(bool debugLog)
            {
                this.DebugLog = debugLog;
                return this;
            }

            public Builder SetChannel(string channel)
            {
                this.Channel = channel;
                return this;
            }

            public Builder SetAge(int age)
            {
                this.Age = age;
                return this;
            }

            public Builder SetGender(BGAdGender gender)
            {
                this.Gender = gender;
                return this;
            }

            public Builder SetActivatedTime(long activatedTime)
            {
                this.ActivatedTime = activatedTime;
                return this;
            }

            ///Only works on Android
            public Builder SetExtra(string key, string extra)
            {
                if (key != null && extra != null)
                {
                    this.ExtraDictionary.Add(key, extra);
                }
                return this;
            }

            public BigoAdConfig Build()
            {
                return new BigoAdConfig(this);
            }
        }
    }
}