
This Nuget provides you useful types for various physic calculations. It is based on IEEE 1451.0 Physical Units (clause 4.11 in 2006 version, clause 4.13 in 2023 version) structure, which is based on 
Hamilton, Bruce, “A Compact Representation of Physical Units” Hewlett-Packard Company, Palo Alto, California, Hewlett-Packard Laboratories Technical Report HPL-96-61, 1995.

In physics, we have no numbers as numbers - every number has unit. It is not "5", but "5 meters", "5 seconds", "5 amperes", "5 billion years", etc. Each calculation is done in two steps: (1) calculate number, and (2) calculate units. Now, you can make your own value calculation using e.g. double type, and calculate units using this Nuget.

To use, first you should create new Units object. It can be done in several ways:

	UnitsForYourVariable = Meter	// and now UnitsForYourVariable is Meter
	UnitsForYourVariable = Meter(2)	// and now UnitsForYourVariable is m²
	UnitsForYourVariable = SquareMeter // same as above

For SI derived units, you have many options available:

	UnitsForYourVariable = Volt
	UnitsForYourVariable = Watt / Ampere
	UnitsForYourVariable = Meter(2) * Kilogram() * Second(-3) * Ampere(-1)
	UnitsForYourVariable = new Units(0, 0, 2, 1, -3, -1, 0, 0, 0)

You can also use general constructor:

	New(_radians , _steradians , _meters , _kilograms , _seconds , _amperes , _kelvins , _moles , _candels )

There are four operators defined: multiply (*), divide (-), equals (=) and differs (not equal, <> or !=).

You can also test if one unit "is inside" second unit, e.g. if Amper has meters:

	value.Contains(value2)	// m is inside m², but m² is not inside m; second is not inside volt (but Hz is)
	value.AbsContains(value2) // Hz is inside Volt, but also second is inside volt (although as 1/s³, not s)

