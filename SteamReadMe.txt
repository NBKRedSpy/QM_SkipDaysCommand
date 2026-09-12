[h1]Quasimorph Fast Forward Time[/h1]


[b]WARNING[/b]: Using console commands will disable achievements.

A debugging utility to fast forward the game's time by X number of days or hours, as quickly as the game can process.
Can only be run while a game is loaded and is in space.

Usage:

[i]skip-days 30[/i]
[i]skip-days 8h[/i]

The mod will log the process to the Player.log.

[h1]Important[/h1]

Since the game will not allow time to pass when the dev console is open, this mod will close the dev console and log the process in the Player.log.
Once completed, the mod will re-open the console.

Opening the console will abort forwarding time.

[h1]Support[/h1]

If you enjoy my mods and want to buy me a coffee, check out my [url=https://ko-fi.com/nbkredspy71915]Ko-Fi[/url] page.
Thanks!

[h1]Source Code[/h1]

Source code is available on GitHub at https://github.com/NBKRedSpy/QM_SkipDaysCommand

[h1]Change Log[/h1]

[h2]2.0.0[/h2]
[list]
[*]Changed to use Unity TimeScale instead of partially emulated days.
[/list]

[h2]1.1.1[/h2]
[list]
[*]Fix: Station item creation was not being called as the game looks for the day to be Monday rather than checking if x amount of time has passed.
[/list]

[h2]1.1.0[/h2]
[list]
[*]Added hours option with the 'h' suffix
[/list]
