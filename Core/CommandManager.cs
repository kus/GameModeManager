// Included libraries
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Attributes.Registration;

// Declare namespace
namespace GameModeManager
{
    public partial class Plugin : BasePlugin
    {
        // Construct server map group command handler
        [ConsoleCommand("css_mapgroup", "Sets the mapgroup for the MapListUpdater plugin.")]
        [CommandHelper(minArgs: 1, usage: "mg_active", whoCanExecute: CommandUsage.SERVER_ONLY)]
        public void OnMapGroupCommand(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null) 
            {
                // Get map group
                MapGroup? _mapGroup = MapGroups.FirstOrDefault(g => g.Name == $"{command.ArgByIndex(1)}");

                if (_mapGroup == null || _mapGroup.Name == null || _mapGroup.Maps == null)
                {
                    Logger.LogWarning("New map group could not be found. Setting default map group.");
                    _mapGroup = _defaultMapGroup;
                }
                Logger.LogInformation($"Current map group is {CurrentMapGroup.Name}.");
                Logger.LogInformation($"New map group is {_mapGroup.Name}.");

                // Update map list and map menu
                try
                {
                    UpdateMapList(_mapGroup);
                }
                catch(Exception ex)
                {
                    Logger.LogError($"{ex.Message}");
                }
            }
        }

        // Construct admin map menu command handler
        [RequiresPermissions("@css/changemap")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [ConsoleCommand("css_maps", "Provides a list of maps for the current game mode.")]
        public void OnMapsCommand(CCSPlayerController? player, CommandInfo command)
        {
            if(player != null && _plugin != null && MapMenu != null)
            {
                MapMenu.Title = Localizer["maps.hud.menu-title"];
                OpenMenu(MapMenu, Config.GameMode.Style, player);
            }
        }

        // Construct change map command handler
        [RequiresPermissions("@css/changemap")]
        [CommandHelper(minArgs: 1, usage: "[map name] optional: [id]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [ConsoleCommand("css_map", "Changes the map to the map specified in the command argument.")]
        public void OnMapCommand(CCSPlayerController? player, CommandInfo command)
        {
            if(player != null && _plugin != null)
            {
                Map _newMap = new Map($"{command.ArgByIndex(1)}",$"{command.ArgByIndex(2)}");
                Map? _foundMap = Maps.FirstOrDefault(g => g.Name == $"{command.ArgByIndex(1)}");

                if (_foundMap != null)
                {
                    _newMap = _foundMap; 
                }
                // Write to chat
                Server.PrintToChatAll(Localizer["changemap.message", player.PlayerName, _newMap.Name]);
                // Change map
                AddTimer(5.0f, () => ChangeMap(_newMap));
            }
        }

        // Construct change mode command handler
        [RequiresPermissions("@css/changemap")]
        [CommandHelper(minArgs: 1, usage: "[mode]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [ConsoleCommand("css_mode", "Changes the game mode to the mode specified in the command argument.")]
        public void OnModeCommand(CCSPlayerController? player, CommandInfo command)
        {
            if(player != null && _plugin != null)
            {
                // Write to chat
                Server.PrintToChatAll(Localizer["changemode.message", player.PlayerName, command.ArgByIndex(1)]);

                // Change game mode
                string _option = $"{command.ArgByIndex(1)}".ToLower();
                AddTimer(5.0f, () => Server.ExecuteCommand($"exec {_option}.cfg"));
            }
        }

        // Construct admin mode menu command handler
        [RequiresPermissions("@css/changemap")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [ConsoleCommand("css_modes", "Provides a list of game modes.")]
        public void OnModesCommand(CCSPlayerController? player, CommandInfo command)
        {
            if(player != null && _plugin != null && ModeMenu != null)
            {
                ModeMenu.Title = Localizer["mode.hud.menu-title"];
                OpenMenu(ModeMenu, Config.GameMode.Style, player);
            }
        }

        // Construct change setting command handler
        [RequiresPermissions("@css/cvar")]
        [CommandHelper(minArgs: 2, usage: "[enable/disable] [setting name]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [ConsoleCommand("css_setting", "Changes the game setting specified.")]
        public void OnSettingCommand(CCSPlayerController? player, CommandInfo command)
        {
            if(player != null && _plugin != null)
            {
                // Get args
                string _status = $"{command.ArgByIndex(1).ToLower()}";
                string _settingName = $"{command.ArgByIndex(2)}";

                // Find game setting
                Setting? _option = Settings.FirstOrDefault(s => s.Name == _settingName);

                if(_option != null) 
                {
                    if (_status == "enable")
                    {
                        // Write to chat
                        Server.PrintToChatAll(Localizer["enable.changesetting.message", player.PlayerName, command.ArgByIndex(2)]);

                        // Change game setting
                        Server.ExecuteCommand($"exec settings/{_option.ConfigDisable}");
                    }
                    else if (_status == "disable")
                    {
                        // Write to chat
                        Server.PrintToChatAll(Localizer["disable.changesetting.message", player.PlayerName, command.ArgByIndex(2)]);

                        // Change game setting
                        Server.ExecuteCommand($"exec settings/{_option.ConfigDisable}");
                    }
                    else
                    {
                        
                        command.ReplyToCommand($"Unexpected argument: {command.ArgByIndex(1)}");
                    }  
                }
                else
                {
                    command.ReplyToCommand($"Can't find setting: {command.ArgByIndex(2)}");
                }
            }
        }
   
        // Construct admin setting menu command handler
        [RequiresPermissions("@css/cvar")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [ConsoleCommand("css_settings", "Provides a list of game settings.")]
        public void OnSettingsCommand(CCSPlayerController? player, CommandInfo command)
        {
            if(player != null && _plugin != null && SettingsMenu != null)
            {
                // Open menu
                SettingsMenu.Title = Localizer["settings.hud.menu-title"];
                OpenMenu(SettingsMenu, Config.Settings.Style, player);
            }
        }
    }
}