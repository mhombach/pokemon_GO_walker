# pokemon_GO_walker
A windows-GUI for pokemon GO on an android-smartphone

## Small description:
pokemon_GO_walker is a little Tool, which allows to control the position of your location in pokemon GO(only Androis is supported).

It needs the App "MockGeoFix" running on the smartphone (https://play.google.com/store/apps/details?id=github.luv.mockgeofix&hl=en).
With this app it is possible to to use a combination of Telnet and "geo fix"-commands to set and control a fake GPS location on the smartphone.

After starting the app on the smartphone, you can use my Tool to connect to your phone. When connected, you can set positions AND (the main reason for this programm) you can "walk" in 4 directions (more coming).

I know the setup on your smartphone is not very easy, if you are not that into rooted devices. I will create better tutorials, if the demand is there. If you want a youtube-tutorial or have questions, just write me (720degreelotus@gmail.com) and i will do my best to help you :)

## Features:
- Control the distance of every step you makes
- "Auto-Wakler" for random movement (to incubate an egg)
- [soon] 2-Player Mode: 2 Players can control their pokemon GO on 1 instance of the programm (one uses WASD, the other the Numpad(Arrow-Keys)
- - [soon] Programm remembers your last location used
- [soon] you can create bookmarks with locations and you can "jump" to them

## Usage
- You need a rooted android-smartphone (for example use: https://root-apk.kingoapp.com/)
- With rooted smartphone you need to install Xposed framework
- With Xposed framework you need to install and activate "Hide Mock locations" (https://play.google.com/store/apps/details?id=ru.gavrikov.hidemocklocations)
- Install MockGeoFix App on your smartphone (https://play.google.com/store/apps/details?id=github.luv.mockgeofix&hl=en)
- "Allow Mock GPS" must be enabled on your smartphone (developer settings) or else MockGeoFix will tell you to enable it
- ... And sure you need pokemon GO installed :)
- start MockGeoFix App, click "run"
- Start executable (coming soon)
- Insert your IP and Port (shown/set in MockGeoFix App-Settings)
- Click "Connect". If exe doesn't crash, connection should be fine
- Insert decimal GPS coordinates (you can use http://www.gpskoordinaten.de for getting the coordinates but you need to replace the dots with commas at this point, will be fixed later)
- Click "set GPS". After the click, your handy should have the coordinates (you can check coordinates with this App: https://play.google.com/store/apps/details?id=com.eclipsim.gpsstatus2)
- You can now move around with the direction-buttons
- If you click in the Hotkey-field, for as long as the cursor is blinking within the field, you can use the keyboard-arrowkeys to move around
- Thas's It :)

## Changelog

### version 2016-07-31
- Started
