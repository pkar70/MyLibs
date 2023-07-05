This package contains helper methods for writing log of data.

After initialization

	SetRootFolder(sRootFolder)
	new Datalog(sRootFolder, sSubfolder)

sRootFolder must exists, sSubfolder would be created on first write.

you probably would use only these methods:

	AppendLogYearly(dataToAppend As String)		// append to file: root\YYYY.txt
	AppendLogMonthly(dataToAppend As String)	// append to file: root\YYYY\YYYY.MM.txt
	AppendLogDaily(dataToAppend As String)		// append to file: root\YYYY\MM\YYYY.MM.dd.txt
	AppendLogDailyWithTime(dataToAppend As String)	// append to file: root\YYYY\MM\YYYY.MM.dd.HH.mm.txt

But you can also use "more internal" methods:

	GetLogFileYearly(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
	GetLogFileMonthly(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
	GetLogFileDaily(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String
	GetLogFileDailyWithTime(Optional sBaseName As String = "", Optional sExtension As String = ".txt") As String

All above methods returns file pathnames with filename constructed from sBaseName concatenated with appropriate date part and extension, in proper subfolder inside Datalog's root.

And, if you want to iterate directories in log folders:

	GetLogFolderYear() As String	// root\YYYY
	GetLogFolderMonth() As String	// root\YYYY\MM

For .Net Standard 2.0 and above, you have additional constructors:

	New(specialFolder As Environment.SpecialFolder, Optional sSubfolder As String = "Datalog") // in this specialfolder, folder named as application would be created
	New(folderType As DatalogFolder, Optional sSubfolder As String = "Datalog")	// Local or Roam, works also in UWP
	New(Optional sSubfolder As String = "Datalog")	// same as New(DatalogFolder.Local, sSubfolder)

and also one utility method:

	static GetAppName As String

Version 2.x works with UWP for phones (15063, Lumia 532), if you use trick from:
https://gist.github.com/WamWooWam/e72e5137606f7c59ed657db6587cd5e8

You can also use Nuget pkar.uwp.datalog - almost same as this Nuget, but writes log to memory card (external storage).