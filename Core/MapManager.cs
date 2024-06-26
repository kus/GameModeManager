// Included libraries
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

// Declare namespace
namespace GameModeManager
{
    // Define Map class
    public class Map : IEquatable<Map>
    {
        public string Name { get; set; }
        public string WorkshopId { get; set; }

        public Map(string name)
        {
            Name = name;
            WorkshopId = "";
        }
        
        public Map(string name, string workshopId)
        {
            Name = name;
            WorkshopId = workshopId;
        }

        public bool Equals(Map? other) 
        {
            if (other == null) return false;  // Handle null 

            // Implement your equality logic, e.g.;
            return Name == other.Name && WorkshopId == other.WorkshopId;
        }

        public void Clear()
        {
            Name = "";
            WorkshopId = "";
        }
    }

    // Plugin class
    public partial class Plugin : BasePlugin
    {
        // Define current map and map list
        public static Map? CurrentMap;     
        public static List<Map> Maps = new List<Map>();
       
        // Construct reusable function to update map list
        private void UpdateMapList(MapGroup _group)
        {  
            // If using RTV Plugin
            if(Config.RTV.Enabled)
            {
                // Update map list for RTV Plugin
                try 
                {
                    using (StreamWriter writer = new StreamWriter(Config.RTV.MapListFile))
                    {
                        foreach (Map _map in _group.Maps)  
                        {
                            if (string.IsNullOrEmpty(_map.WorkshopId))
                            {
                                writer.WriteLine(_map.Name);
                            }
                            else
                            {
                                if(Config.RTV.DefaultMapFormat)
                                {
                                    writer.WriteLine($"ws:{_map.WorkshopId}");
                                }
                                else
                                {
                                    writer.WriteLine($"{_map.Name}:{_map.WorkshopId}");
                                }
                            }
                        } 
                    } 
                } 
                catch (IOException ex)
                {
                    Logger.LogError("Could not update map list.");
                    Logger.LogError($"{ex.Message}");
                }

                // Reload RTV Plugin
                Server.ExecuteCommand($"css_plugins reload {Config.RTV.Plugin}");
            }
            // Update map list for map menu
            try
            {
                UpdateMapMenu(_group);
            }
            catch(Exception ex)
            {
                Logger.LogError($"{ex.Message}");
            }
        }

        // Construct reusable function to change map
        private void ChangeMap(Map _nextMap)
        {
            // If map valid, change map based on map type
            if (Server.IsMapValid(_nextMap.Name))
            {
                Server.ExecuteCommand($"changelevel \"{_nextMap.Name}\"");
            }
            else if (_nextMap.WorkshopId != null)
            {
                Server.ExecuteCommand($"host_workshop_map \"{_nextMap.WorkshopId}\"");
            }
            else
            {
                Server.ExecuteCommand($"ds_workshop_changelevel \"{_nextMap.Name}\"");
            }

            // Set current map
            CurrentMap = _nextMap;
        }

        // Construct EventGameEnd Handler to automatically change map at game end
        int _counter = 0;
        private HookResult EventGameEnd(EventCsIntermission @event, GameEventInfo info)
        {
            Logger.LogInformation("Game has ended. Picking random map from current map group...");
            Server.PrintToChatAll(Localizer["plugin.prefix"] + " Game has ended. Changing map...");

            // Check if RTV is disabled in config and if so enable randomization
            if(!Config.RTV.Enabled)
            {
                if(CurrentMapGroup == null)
                {
                    CurrentMapGroup = _defaultMapGroup;
                }         

                // Check if game mode rotation is enabled
                if(Config.GameMode.Rotation && (float)_counter % Config.GameMode.Interval == 0)
                {  
                    // Get random game mode
                    Random _rnd = new Random();
                    int _randomIndex = _rnd.Next(0, MapGroups.Count); 
                    MapGroup _randomMode = MapGroups[_randomIndex];

                    // Change mode
                    Server.ExecuteCommand($"exec {_randomMode.Name}.cfg");
                }
                else
                {
                    // Get a random map
                    Random _rnd = new Random();
                    int _randomIndex = _rnd.Next(0, CurrentMapGroup.Maps.Count); 
                    Map _randomMap = CurrentMapGroup.Maps[_randomIndex];

                    // Change map
                    ChangeMap(_randomMap);
                }
            }
            _counter++;
            return HookResult.Continue;
        }
    }
}