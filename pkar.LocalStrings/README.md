
This Nuget helps me dealing with localizing software - it's role is to being one, common interface for all UI modes (UWP, WPF, Uno, MAUI...). You can use it directly, or use it via my UI nugets.


# Initializing
 Before use, you should initialize library in code with sequence similar to this:

	Localize.InitResMan(resPrefix = "res:", fallbackReplacePrefix = "") // localize strings prefixed with resPrefix; if no localized replacement is defined, return same string with prefix changed to fallback
	Localize.AddResMan("en", resourcemanagerEn, true)   // add first resource manager, for 'en' language, and this is default resource manager (if no resman for selected language exist). 'en' is as TwoLetterISOLanguageName
	Localize.AddResMan("pl", My.Resources.Resource_PL.ResourceManager) // add other resource managers
	...
	Localize.SelectCurrentLang  // select resource manager for CurrentCulture (without it, default resman would be used)

# Check if initialized

    Localize.GetResManCount() As Integer
    Localize.IsInitialized() As Boolean

# Retrieving one string

    Localize.GetResManString(sResID , sDefault = "") // returns localized string, or default

    Localize.TryGetResManString(inputString , sDefault = "")
    // if inputString is not prefixed with resPrefix from InitResMan, then return original string;
    // else, if inputString (with stripped resPrefix) is found in current ResMan, returns localized string;
    // else (if not found), returns default (if not empty), or inputString with resPrefix changed to fallbackPrefix

# Set texts in UI (in any object)

    Localize.SetPropertiesUsingObjectName(anyObject)
    // if object has Property "name", try to get every String property from ResMan, using GetResManString. resId is constructed same as in UWP: object name "." property name

    Localize.SetPrefixedProperties(anyObject)
    // try to localize all String properties of object, using  has Property "name", try to get every String property from ResMan, using TryGetResManString

## how to use it in your XAML code

### using object name

    XAML: <TextBox Name="uiTBox" ...>
    res:  uiTBox.Text="some localized text"
    .Net: Localize.SetPropertiesUsingObjectName(uiTBox)

### using text values

    XAML: <TextBox Text="res:pagetitle" ...>
    res:  pagetitle="some localized text"
    .Net: Localize.SetPrefixedProperties(uiTBox)


# other methods

    Localize.IsCurrentLang(lang = "en") // simple check which lang is used

    Localize.LangOverride(forceLanguage) // overriding current language; returns language previously used. Call without parameter to return to resman used before Override
    Localize.ResManOverride(forceResMan) // overriding ResMan; returns language previously used; Call without parameter to return to resman used before Override

