# Quasimorph Fast Forward Time

![thumbnail icon](media/thumbnail.png)

**WARNING**: Using console commands will disable achievements.

A debugging utility to fast forward the game's time by X number of days or hours, as quickly as the game can process.
Can only be run while a game is loaded and is in space.

Usage: 

`skip-days 30`
`skip-days 8h`

The mod will log the process to the Player.log.

# Important

Since the game will not allow time to pass when the dev console is open, this mod will close the dev console and log the process in the Player.log.
Once completed, the mod will re-open the console.

Opening the console will abort forwarding time.

# Support
If you enjoy my mods and want to buy me a coffee, check out my [Ko-Fi](https://ko-fi.com/nbkredspy71915) page.
Thanks!

# Source Code
Source code is available on GitHub at https://github.com/NBKRedSpy/QM_SkipDaysCommand

# Change Log
## 2.0.0
* Changed to use Unity TimeScale instead of partially emulated days.

## 1.1.1
* Fix: Station item creation was not being called as the game looks for the day to be Monday rather than checking if x amount of time has passed.

## 1.1.0

* Added hours option with the 'h' suffix

