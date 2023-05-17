# Stats Companion
A companion app for tracking [Final Fantasy VI: Worlds Collide](https://ff6worldscollide.com/) run statistics. It's designed to be as easy to use as possible, in other words, it "just works": just open the app and it will silently track your run data, so you'll finally know how long you've been sitting in menus juggling espers around!

### Special thanks:
- My friend and mentor, Ale. None of this would be possible without you <3.
- StatsCollide creator Seto Kiaba, for being the inspiration to Stats Companion.
- The Worlds Collide community and developers, for being the most amazing group of people. Special thanks to DoctorDT for putting up with me asking dumb questions about how things work, and my very early stage guinea pigs: Overswarm, HunnyDoo, RetrophileTV and gaahr.

## Requirements
- **[.NET Desktop Runtime 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)**
- **[SNI](https://github.com/alttpo/sni)** (Super Nintendo Interface) - if you're auto-tracking your run you probably already have this. It's not required but it's **STRONGLY RECOMMENDED** that you update SNI to the latest version since it now features automatic reconnection.
- A device/emulator compatible with SNI. Both [**Snes9x-rr 1.60**](https://github.com/gocha/snes9x-rr/releases) and **[RetroArch](https://www.retroarch.com/) version 1.9.2 or higher** with bsnes-mercury core are working properly at the moment. Sd2snes is also supported.

## Features

 - **Run timer**: accurate timer that will automatically clock your run, while also tracking important events like: time you unlocked Kefka Tower, time you stepped on Kefka Tower switches, and more.
 - **Statistics tracking**: time spent on menus and shops, time spent driving the airship, time spent in battle, amount of resets, starting characters and commands, final battle preparation, use of crucial items like Super Balls or Exp. Eggs, bosses killed, amount of characters and espers found and dragons killed.
 - Coliseum, Tzen Thief and Auction House specific data.
 - **List of checks done**, as well as checks that were **peeked/reset/abandoned**.
 - Full list of the final **4-character Final Kefka lineup data**, including stats, equipment, command, spell and skill list.
 - **Timestamped list of maps visited** throughout the run.

## How to use

Open Stats Companion to get started. The app will wait for you to hit "new game" on a clean ROM without any previous saves in order to begin tracking. Time will automatically stop once Kefka starts his final battle animation (when he disintegrates after the flash).
After killing Kefka, it will export a JSON file containing all the data inside a "runs" folder. Pretty straightforward!
