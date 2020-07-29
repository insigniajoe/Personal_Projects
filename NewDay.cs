using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KirpysMods
{

    class NewDay : Mod
    {
        public ModConfig Config;
        public int DaysWORain = 0;
        List<String> morningPhrase = new List<String>();

        //string RainDay = this.Config.RainDay;
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            Initialize(helper);

        }
        /// <summary>
        /// Main part of Mod.
        /// Sets up list of morning phrases.  
        /// Did not want to change resource file.  
        /// </summary>
        /// <param name="helper"></param>
        private void Initialize(IModHelper helper)
        {

            //TODO: Add list of phrases in config file?
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.ConsoleCommands.Add("rain", "Toggles rain on demand! \n\nUseage: rain", this.ToggleRain);
            Console.WriteLine(this.Config.Greetings);
            morningPhrase.Add("LETS GET THIS BREAD ");
            morningPhrase.Add("MOISTURIZE ME ");
            morningPhrase.Add("Good morning, Sleeping Beauty! I thought you'd never wake up! ");
            morningPhrase.Add("Good morning, Sunshine! ");
            morningPhrase.Add("Wakey, wakey, eggs and bakey! ");
        }

        /// <summary>
        /// Sets Wednesdays to rain.
        /// Adds greeting from a list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            int MaxDays = this.Config.MaxDays;
            string RainDay = this.Config.RainDay;
            var date = SDate.Now();
            var RainPrep = date.AddDays(-1).DayOfWeek;

            bool UseMax = this.Config.UseMax;
            var CurrSeason = Game1.currentSeason;
            Random newGreeting = new Random();
            int greet2 = newGreeting.Next(0, morningPhrase.Count);
            Console.WriteLine(this.Config.Greetings);
            Console.WriteLine(morningPhrase[greet2]);




            var greeting = string.Format(morningPhrase[greet2] + Game1.player.name + "!");
            if (CurrSeason == "winter")
            {
                Game1.drawObjectDialogue(Game1.parseText(greeting.ToUpper()));

                return;
            }
            if (UseMax)
            {
                if (Game1.isRaining)
                {
                    DaysWORain = 0;
                    this.Monitor.Log($"It is currently raining.  Days without rain: {DaysWORain}", LogLevel.Debug);
                }
                else
                {
                    DaysWORain++;
                    this.Monitor.Log($"It is not raining.  Days without rain: {DaysWORain}", LogLevel.Debug);
                }
                if (DaysWORain >= MaxDays)
                {
                    Game1.weatherForTomorrow = 1;
                    Game1.chanceToRainTomorrow = .999;
                    this.Monitor.Log($"It has been {DaysWORain} days without rain, chance of rain tomorrow: {Game1.chanceToRainTomorrow} ", LogLevel.Debug);
                }
            }
            else
            {

                if (date.AddDays(1).DayOfWeek.ToString() == RainDay.ToString() && Game1.IsMasterGame)
                {
                    Game1.weatherForTomorrow = 1;
                    Game1.chanceToRainTomorrow = .999;
                    this.Monitor.Log($"Its {RainDay} tomorrow! ", LogLevel.Debug);
                }
                else if (!Game1.IsMasterGame)
                {
                    this.Monitor.Log($"User is not host.  Rain update cancelled", LogLevel.Debug);
                }
                else if (date.DayOfWeek.ToString() != RainDay)
                {

                    this.Monitor.Log($"Day is {date}.  Rain is scheduled for {RainDay}s", LogLevel.Debug);
                }
                if (Game1.isRaining)
                {
                    this.Monitor.Log($"It is raining, Chance of rain tomorrow: {Game1.chanceToRainTomorrow}.  Today's day: {date}, { date.DayOfWeek}", LogLevel.Debug);
                    greeting = "ITS RAINING!!!!";
                }
                else
                { this.Monitor.Log($"It is not raining, Chance of rain tomorrow: {Game1.chanceToRainTomorrow}.  Today's day: {date}, { date.DayOfWeek}", LogLevel.Debug); }
            }
            Game1.drawObjectDialogue(Game1.parseText(greeting.ToUpper()));

        }

        /// <summary>
        /// ...Toggles rain.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>

        private void ToggleRain(string command, string[] args)
        {
            if (Game1.currentSeason == "winter")
            {
                this.Monitor.Log($"It is winter, please wait until it is not winter to use this command!", LogLevel.Debug);
                return;
            }
            if (Game1.IsMasterGame)
            {
                if (!Game1.isRaining)
                {
                    Game1.isRaining = true;
                }
                else if (Game1.isRaining)
                {
                    Game1.isRaining = false;
                }
            }
            else
            {
                this.Monitor.Log($"YOU ARE NOT THE HOST. STOP TRYING TO MAKE IT RAIN!", LogLevel.Debug);
            }
        }

    }

}

