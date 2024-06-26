![Copyright Nickj609l](https://img.shields.io/badge/Copyright-Nickj609-red) ![GitHub License](https://img.shields.io/github/license/nickj609/GameModeManager) ![Issues](https://img.shields.io/github/issues/nickj609/GameModeManager) ![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/nickj609/GameModeManager/total) ![GitHub commits since latest release](https://img.shields.io/github/commits-since/nickj609/GameModeManager/latest)

# GameModeManager
A Counter-Strike 2 server plugin inspired by the [CS2 Modded Dedicated Server by Kus](https://github.com/kus/cs2-modded-server) and the [CS2 Rock The Vote plugin by Abnerfs](https://github.com/abnerfs/cs2-rockthevote).

## Description
GameModeManager streamlines your Counter-Strike 2 server administration with these features:

- **Admin Game Mode Menu:** Effortlessly switch between game modes.
- **Admin Map List Menu:** Effortlessly switch between maps within the current game mode.
- **Default Map Cycles:** Automatically changes the map to a random map within the current map group.
- **RTV Compatibility:** Works seamlessly with your chosen RTV plugin, ensuring smooth rock-the-vote functionality.

## Credits
This plugin utilizes the [GameLoop.Vdf library](https://github.com/shravan2x/Gameloop.Vdf/) (licensed under the [MIT License](https://github.com/shravan2x/Gameloop.Vdf/blob/master/LICENSE)) for parsing the `gamemodes_server.txt` file, which is in [Valve Data Format](https://developer.valvesoftware.com/wiki/VDF).

## Requirements
- [Counter-Strike 2](https://www.counter-strike.net/cs2)
- [Metamod:Source](https://github.com/alliedmodders/metamod-source/) (v1282+)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) (v.197+)

> [!NOTE]
> If you are using the [CS2 Modded Dedicated Server by Kus](https://github.com/kus/cs2-modded-server) you should remove the [Ultimate Map Chooser](https://github.com/kus/cs2-modded-server/tree/master/game/csgo/addons/counterstrikesharp/plugins/CS2-Ultimate-Mapchooser) plugin until the pull request is approved/finalized.

## Commands

Server Only
- `css_mapgroup <mg_name>` - Sets the map group and updates the map list.

Client Only
- `!map <map name> <id>` - Changes the map to the map specified.
  
  > The map id is optional and only required if not already specified in an existing map group.

- `!mode (css_mode)` - Changes the game mode to the mode specified.
  
  > Only the mode name is required. For example, for mg_surf you would do !mode surf. 
  
- `!modes (css_modes)` - Displays an admin menu with game mode options.

  ![Screenshot 2024-03-21 161458](https://github.com/nickj609/GameModeManager/assets/32173425/db33fe48-21f3-455c-9987-5406fca99c4f)

- `!maps (css_maps)` - Displays an admin menu with maps for the current game mode/map group.

  ![Screenshot 2024-03-21 161416](https://github.com/nickj609/GameModeManager/assets/32173425/f9c193b0-2ad3-4fa1-8a83-eaac812d2f21)

## RTV Plugin Compatibility

> [!IMPORTANT]
> You will need to manually enable RTV Plugin Compatibility within the configuration file after the first load.

This plugin is compatible with any RTV plugin as long as it uses a maplist.txt file.

![Screenshot 2024-03-21 161846](https://github.com/nickj609/GameModeManager/assets/32173425/1e291efb-fe7f-4f0d-bb2c-e21d042bd153)

## Installation
1. Install Metamod:Source and Counter Strike Sharp.
2. Copy DLLs to `csgo/addons/counterstrikesharp/plugins/GameModeManager`.
3. Make sure your `gamemodes_server.txt` file is in VDF format and contains a list of map groups.
4. If needed, update your custom configuration files for each game mode to include `css_mapgroup <map group>`.
5. After the first run, update the configuration file `GameModeManager.json` as detailed below.

## Configuration
> [!IMPORTANT]
> On the first load, a configuration file will be created in `csgo/addons/counterstrikesharp/configs/plugins/GameModeManager/GameModeManager.json`.

GameModeManager offers flexible configuration options. See below for details and customization instructions.

### RTV Settings
| Setting             | Description                                                                                                                               |
| ------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- | 
| Enabled             | When enabled, the RTV plugin specified will be reloaded after updating the maplist.txt file.                                              | 
| Plugin              | This is the default path to the RTV Plugin you are using. This can also just be the module name.                                          | 
| MapListFile         | This is the default path for the maplist.txt file that will be updated when the map group or game mode changes.                           | 
| DefaultMapFormat    | When enabled, the format for adding maps to the maplist.txt file will be `ws:{workshopid}` instead of `{mapname}:{workshopid}`.           |

### Map Group Settings
| Setting             | Description                                                                                                                               |
| ------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| Default             | Default map group on server start (i.e. mg_active).                                                                                        | 
| File                | Default path for the gamemodes_server.txt file used to specify game modes and map groups.                                                 |     

### Game Mode Settings
| Setting             | Description                                                                                                                               |
| ------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- | 
| ListEnabled         | When enabled, the game mode list specified will be used. Otherwise, the list will be generated based on the map groups discovered.        |
| List                | A list of game modes for your server.                                                                                                     |     

> [!NOTE]
> - All game mode configuration files must be located in the `csgo/cfg` directory and include `css_mapgroup` to set the current map group.
> - If `ListEnabled` is set to `false`, the Game Mode List will be created based on the discovered map groups. For example, `mg_surf` would display as `surf` and the `surf.cfg` would be executed. 

### Default Values

```
// This configuration was automatically generated by CounterStrikeSharp for plugin 'GameModeManager', at 2024/04/01 04:22:02
{
  "RTV": {
    "Enabled": false,
    "Plugin": "/home/steam/cs2/game/csgo/addons/counterstrikesharp/plugins/RockTheVote/RockTheVote.dll",
    "MapListFile": "/home/steam/cs2/game/csgo/addons/counterstrikesharp/plugins/RockTheVote/maplist.txt",
    "DefaultMapFormat": false
  },
  "MapGroup": {
    "Default": "mg_active",
    "File": "/home/steam/cs2/game/csgo/gamemodes_server.txt"
  },
  "GameMode": {
    "ListEnabled": true,
    "List": [
      "comp",
      "1v1",
      "aim",
      "awp",
      "scoutzknivez",
      "wingman",
      "gungame",
      "surf",
      "dm",
      "dm-multicfg",
      "course",
      "hns",
      "kz",
      "minigames"
    ]
  },
  "ConfigVersion": 1
}
```

### Languages
This plugin will display all in-game menus and messaging within the language of the player. Below is an example language configuration you can customize to your liking.

/lang/en.json
```
{
  "plugin.prefix": "[{GREEN}Server{DEFAULT}]",
  "changemap.message": "Admin {LIGHTRED}{0}{DEFAULT} has changed the map to {LIGHTRED}{1}{DEFAULT}.",
  "changemode.message": "Admin {LIGHTRED}{0}{DEFAULT} has changed the game mode to {LIGHTRED}{1}{DEFAULT}.",
  "mode.hud.menu-title": "Game Mode List",
  "maps.hud.menu-title": "Map List"
}
```

## Logging
>[!WARNING]
> Due to the heavy use of the above configuration file to specify paths to files and plugins, you may run into several issues upon the initial deployment. All logs associated with this plugin can be found in the below location.
> 
> `csgo/addons/counterstrikesharp/logs`

### Example
```
2024-04-01 03:26:13.249 +00:00 [INFO] plugin:GameModeManager Loading map groups...
2024-04-01 03:26:13.264 +00:00 [INFO] plugin:GameModeManager Creating game modes...
2024-04-01 03:27:20.767 +00:00 [INFO] plugin:GameModeManager Current map group is mg_active.
2024-04-01 03:27:20.768 +00:00 [INFO] plugin:GameModeManager New map group is mg_aim.
```

### Common Error Messages
| Error Message                                                  | Description                                                                                                              |
| ---------------------------------------------------------------| ------------------------------------------------------------------------------------------------------------------------ | 
| `Cannot Find`                                                  | Unable to locate the file specified from `GameModeManager.json` config.                                                  | 
| `Incomplete VDF data`                                          | Your `gamemodes_server.txt` file is not formatted properly in [VDF Format](https://developer.valvesoftware.com/wiki/VDF).| 
| `The mapgroup property doesn't exist`                          | The "mapgroup" property cannot be found in your `gamemodes_server.txt` file.                                             | 
