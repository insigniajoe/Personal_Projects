using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Set max days without rain
//TODO: Config for setting "Rain day or max days without rain"
namespace KirpysMods
{
     
    class NewDay:Mod
    {
        
        public bool isRaining = false;
        List<String> morningPhrase = new List<String>();
        
        public override void Entry(IModHelper helper)
        {
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

            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.ConsoleCommands.Add("rain", "Toggles rain on demand! \n\nUseage: rain", this.ToggleRain);
            morningPhrase.Add("LETS GET THIS BREAD ");
            morningPhrase.Add("MOISTURIZE ME ");
            morningPhrase.Add("Good morning, Sleeping Beauty! I thought you'd never wake up! ");
            morningPhrase.Add("Good morning, Sunshine! ");
            morningPhrase.Add("Wakey, wakey, eggs and bakey! ");
        }

        /// <summary>
        /// Sets Wednesdays to rain.
        /// Adds greeting from a list.
        /// TODO: Add config option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

            //TODO: update if/else for config variable.
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            Random newGreeting = new Random();
            int greet2 = newGreeting.Next(0, morningPhrase.Count);
            Console.WriteLine(morningPhrase[greet2]);


            isRaining = Game1.isRaining;
            var date = SDate.Now();
            var Tues = "Tuesday";
            var Wed = "Wednesday";

            var greeting = string.Format(morningPhrase[greet2] + Game1.player.name + "!");
            if (date.DayOfWeek.ToString() == Tues &&  Game1.IsMasterGame)
            {
                Game1.weatherForTomorrow = 1;
                Game1.chanceToRainTomorrow = .999;
                
            }
            else if (!Game1.IsMasterGame)
            {
                this.Monitor.Log($"User is not host.  Rain update cancelled", LogLevel.Debug);
            }
            else if (date.DayOfWeek.ToString() != Wed)
            {

                this.Monitor.Log($"Day is {date}.  Rain is scheduled for Wednesdays", LogLevel.Debug);
            }
            if (Game1.isRaining)
            {
                this.Monitor.Log($"It is raining, Chance of rain tomorrow: {Game1.chanceToRainTomorrow}.  Today's day: {date}, { date.DayOfWeek}", LogLevel.Debug);
                greeting = "ITS RAINING!!!!";
            }
            else
            { this.Monitor.Log($"It is not raining, Chance of rain tomorrow: {Game1.chanceToRainTomorrow}.  Today's day: {date}, { date.DayOfWeek}", LogLevel.Debug); }

            Game1.drawObjectDialogue(Game1.parseText(greeting.ToUpper()));
            
        }

        /// <summary>
        /// ...Toggles rain.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        private void  ToggleRain(string command, string[] args)
        {
            if(Game1.IsMasterGame)
            {
                if (!isRaining)
                {
                    Game1.isRaining = true;
                    isRaining = true;
                }
                else if (isRaining)
                {
                    Game1.isRaining = false;
                    isRaining = false;
                }
            }
            else
            {
                this.Monitor.Log($"YOU ARE NOT THE HOST. STOP TRYING TO MAKE IT RAIN!", LogLevel.Debug);
            }
        }
        
    }

    }

