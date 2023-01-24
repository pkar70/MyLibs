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

	