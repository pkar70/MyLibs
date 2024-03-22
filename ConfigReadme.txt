

classlib 1.4:
Nuget-Configsy
-> JSON

pkar.Uwp.Configs
* configExtensions <- WPF, WinUI, Uno
* configUWP	
-> pkar.Netconfigs 1.*

pkar.WPF.Configs
* extensions -> UWP
* methods <- WinUI
-> pkar.Netconfigs 2.*

pkar.winui3.Configs
* extensions  -> UWP
* methods -> WPF
-> pkar.Netconfigs 2.*

pkar.Uno.Configs
* extensions -> UWP
* methods 
-> pkar.Netconfigs 2.*


pkar.MAUI.Configs
* extensions 
* methods 
-> pkar.Netconfigs 2.*


