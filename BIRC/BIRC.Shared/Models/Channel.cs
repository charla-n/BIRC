using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIRC.Shared.Models
{
    public class Channel
    {
        public string Name { get; set; }
        [JsonIgnore]
        private string history;
        [JsonIgnore]
        public string History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
                MainPage.RunActionOnUiThread(() =>
                {
                    MainPage.currentDataContext.Changed("WebViewContent");
                });
            }
        }
    }
}
