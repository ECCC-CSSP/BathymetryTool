namespace MainViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public string startDir;

    [ObservableProperty]
    public string fileXYZFullName;

    [ObservableProperty]
    public bool fileXYZSelected;

    [ObservableProperty]
    public string fileKMLToCreateFromXYZFullName;

    [ObservableProperty]
    public bool fileKMLCreatedExist;

    [ObservableProperty]
    public string fileKMLFullName;

    [ObservableProperty]
    public bool fileKMLSelected;

    [ObservableProperty]
    public string fileXYZToCreateFromKMLFullName;

    [ObservableProperty]
    public bool fileXYZCreatedExist;

    [ObservableProperty]
    public string fileXYZWithinPolygonFullName;

    [ObservableProperty]
    public string fileXYZWithinPolygonName;

    [ObservableProperty]
    public string googleEarthPolygonText;

    [ObservableProperty]
    public string statusCreateKML;

    [ObservableProperty]
    public string statusCreateXYZ;

    [ObservableProperty]
    public string statusCreateXYZWithinPolygon;

    [ObservableProperty]
    public string status;

    [ObservableProperty]
    public string log;

    [RelayCommand]
    async Task ChangeStartDir()
    {
        FolderPickerResult result = await PickFolder(StartDir, CancellationToken.None);
        if (result.IsSuccessful)
        {
            StartDir = result.Folder.Path + "\\";

            FileXYZWithinPolygonFullName = StartDir + "\\" + FileXYZWithinPolygonName;
        }
        else
        {
            // don't change the StartDir
        }
    }

    [RelayCommand]
    async Task CreateKMLFileFromXYZFile()
    {
        Status = "";
        StatusCreateKML = "";

        FileInfo fiXYZ = new (FileXYZFullName);

        await CreateKMLFileFromXYZFileAsync(fiXYZ);

        FileInfo fiKML = new (FileKMLToCreateFromXYZFullName);
        if (fiKML.Exists)
        {
            FileKMLCreatedExist = true;
        }
        else
        {
            FileKMLCreatedExist = false;
        }

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateKML = Status;
        }
        else
        {
            StatusCreateKML = $"Created File: --- {FileKMLToCreateFromXYZFullName}";
        }
    }

    [RelayCommand]
    async Task CreateOneXYZFileFromManyXYZFileWithinAPolygon()
    {
        Status = "";
        StatusCreateXYZWithinPolygon = "";

        DirectoryInfo di = new DirectoryInfo(StartDir);

        if (!di.Exists)
        {
            StatusCreateXYZWithinPolygon = $"Directory does not exist [{di.FullName}]";
        }
        else
        {
            await CreateOneXYZFileFromManyXYZFileWithinAPolygonAsync(di, FileXYZWithinPolygonName, GoogleEarthPolygonText);

            if (!string.IsNullOrEmpty(Status))
            {
                StatusCreateXYZWithinPolygon = Status;
            }
            else 
            { 
                FileInfo fiXYZ = new (di.FullName + "\\" + FileXYZWithinPolygonName);
                FileXYZWithinPolygonFullName = fiXYZ.FullName;

                if (fiXYZ.Exists)
                {
                    StatusCreateXYZWithinPolygon = $"Created File: --- {fiXYZ.FullName}";
                }
                else
                {
                    StatusCreateXYZWithinPolygon = $"Could not create File: --- {fiXYZ.FullName}";
                }

            }

        }
    }

    [RelayCommand]
    async Task CreateXYZFileFromKMLFile()
    {
        Status = "";
        StatusCreateXYZ = "";

        FileInfo fiKML = new (FileKMLFullName);

        await CreateXYZFileFromKMLFileAsync(fiKML);

        FileInfo fiXYZ = new (FileXYZToCreateFromKMLFullName);
        if (fiXYZ.Exists)
        {
            FileXYZCreatedExist = true;
        }
        else
        {
            FileXYZCreatedExist = false;
        }

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateXYZ = Status;
        }
        else
        {
            StatusCreateXYZ = $"Created File: --- {FileXYZToCreateFromKMLFullName}";
        }
    }

    [RelayCommand]
    async Task OpenFileExplorerAtKMLFileSelected()
    {
        Status = "";
        StatusCreateXYZ = "";

        FileInfo fi = new (FileKMLFullName);

        if (fi.Directory != null)
        {
            Process.Start("explorer.exe", fi.Directory.FullName);

            StatusCreateXYZ = $"Opened File Explorer at directory: --- {fi.Directory.FullName}";

            await Task.CompletedTask;
        }
    }

    [RelayCommand]
    async Task OpenFileExplorerAtXYZFileSelected()
    {
        Status = "";
        StatusCreateKML = "";

        FileInfo fi = new (FileXYZFullName);

        if (fi.Directory != null)
        {
            Process.Start("explorer.exe", fi.Directory.FullName);

            StatusCreateKML = $"Opened File Explorer at directory: --- {fi.Directory.FullName}";
        }

        await Task.CompletedTask;
    }

    [RelayCommand]
    async Task OpenFileExplorerAtStartDir()
    {
        Status = "";
        StatusCreateKML = "";

        DirectoryInfo di = new (StartDir);

        if (!di.Exists)
        {
            Status = $"Directory does not exist [{di.FullName}]";
            Log = Status;
            StatusCreateKML = Status;

            return;
        }

        Process.Start("explorer.exe", di.FullName);

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateKML = Status;
        }
        else
        {
            StatusCreateKML = $"Opened File Explorer at directory: --- {di.FullName}";
        }

        await Task.CompletedTask;
    }

    [RelayCommand]
    async Task OpenKMLFileCreated()
    {
        Status = "";
        StatusCreateKML = "";

        FileInfo fiKML = new (FileKMLToCreateFromXYZFullName);

        if (!fiKML.Exists)
        {
            Status = $"File does not exist [{fiKML.FullName}]";
            Log = Status;
            StatusCreateKML = Status;

            return;
        }

        await Launcher.Default.OpenAsync(new OpenFileRequest("Open Google Earth", new ReadOnlyFile(fiKML.FullName)));

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateKML = Status;
        }
        else
        {
            StatusCreateKML = $"Opened File in Google Earth: --- {FileKMLToCreateFromXYZFullName}";
        }
    }

    [RelayCommand]
    async Task OpenKMLFileSelected()
    {
        Status = "";
        StatusCreateXYZ = "";

        FileInfo fiKML = new (FileKMLFullName);

        if (!fiKML.Exists)
        {
            Status = $"File does not exist [{fiKML.FullName}]";
            Log = Status;
            StatusCreateXYZ = Status;

            return;
        }

        await Launcher.Default.OpenAsync(new OpenFileRequest("Open File in Google Earth", new ReadOnlyFile(fiKML.FullName)));

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateXYZ = Status;
        }
        else
        {
            StatusCreateXYZ = $"Opened File in NotePad: --- {FileKMLFullName}";
        }
    }

    [RelayCommand]
    async Task OpenXYZFileCreated()
    {
        Status = "";
        StatusCreateXYZ = "";

        FileInfo fiXYZ = new (FileXYZToCreateFromKMLFullName);

        if (!fiXYZ.Exists)
        {
            Status = $"File does not exist [{fiXYZ.FullName}]";
            Log = Status;
            StatusCreateXYZ = Status;

            return;
        }

        await Launcher.Default.OpenAsync(new OpenFileRequest("Open in Notepad", new ReadOnlyFile(fiXYZ.FullName)));

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateXYZ = Status;
        }
        else
        {
            StatusCreateXYZ = $"Opened File in Google Earth: --- {FileXYZToCreateFromKMLFullName}";
        }
    }

    [RelayCommand]
    async Task OpenXYZFileSelected()
    {
        Status = "";
        StatusCreateKML = "";

        FileInfo fiXYZ = new FileInfo(FileXYZFullName);

        if (!fiXYZ.Exists)
        {
            Status = $"File does not exist [{fiXYZ.FullName}]";
            Log = Status;
            StatusCreateKML = Status;

            return;
        }

        await Launcher.Default.OpenAsync(new OpenFileRequest("Open Notepad", new ReadOnlyFile(fiXYZ.FullName)));

        if (!string.IsNullOrEmpty(Status))
        {
            StatusCreateKML = Status;
        }
        else
        {
            StatusCreateKML = $"Opened File in NotePad: --- {FileXYZFullName}";
        }
    }

    [RelayCommand]
    async Task PickXYZFileToCreateKMLFile()
    {
        Status = "";
        StatusCreateKML = "";

        PickOptions options = await GetCustomFileType(".xyz");

        FileResult? fileResult = await PickFile(options);
        if (fileResult != null)
        {
            FileXYZFullName = fileResult?.FullPath ?? "";

            FileKMLToCreateFromXYZFullName = Path.ChangeExtension(FileXYZFullName, ".kml");

            FileInfo fi = new FileInfo(FileXYZFullName);
            if (fi.Exists)
            {
                FileXYZSelected = true;
            }
            else
            {
                FileXYZSelected = false;
            }

            fi = new FileInfo(FileKMLToCreateFromXYZFullName);
            if (fi.Exists)
            {
                FileKMLCreatedExist = true;
            }
            else
            {
                FileKMLCreatedExist = false;
            }

            StatusCreateKML = $"Selected File: --- {FileXYZFullName}";
        }
    }

    [RelayCommand]
    async Task PickKMLFileToCreateXYZFile()
    {
        Status = "";
        StatusCreateXYZ = "";

        PickOptions options = await GetCustomFileType(".kml");

        FileResult? fileResult = await PickFile(options);
        if (fileResult != null)
        {
            FileKMLFullName = fileResult?.FullPath ?? "";

            FileXYZToCreateFromKMLFullName = Path.ChangeExtension(FileKMLFullName, ".xyz");

            FileInfo fi = new FileInfo(FileKMLFullName);
            if (fi.Exists)
            {
                FileKMLSelected = true;
            }
            else
            {
                FileKMLSelected = false;
            }

            fi = new FileInfo(FileXYZToCreateFromKMLFullName);
            if (fi.Exists)
            {
                FileXYZCreatedExist = true;
            }
            else
            {
                FileXYZCreatedExist = false;
            }

            StatusCreateXYZ = $"Selected File: --- {FileKMLFullName}";
        }
    }

    private string GetGooglePolygonExample ()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine($@"<kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">");
        sb.AppendLine($@"<Document>");
        sb.AppendLine($@"	<name>KmlFile</name>");
        sb.AppendLine($@"	<Placemark>");
        sb.AppendLine($@"		<name>Untitled Polygon</name>");
        sb.AppendLine($@"		<styleUrl>#m_ylw-pushpin</styleUrl>");
        sb.AppendLine($@"		<Polygon>");
        sb.AppendLine($@"			<tessellate>1</tessellate>");
        sb.AppendLine($@"			<outerBoundaryIs>");
        sb.AppendLine($@"				<LinearRing>");
        sb.AppendLine($@"					<coordinates>");
        sb.AppendLine($@"						-66.74300860188258,44.64907422874228,0 -66.73061105620137,44.52568655868959,0 -66.70029466035791,44.51585599763866,0 -66.64410234522505,44.53981997472919,0 -66.66437206951781,44.59463399512927,0 -66.74300860188258,44.64907422874228,0 ");
        sb.AppendLine($@"					</coordinates>");
        sb.AppendLine($@"				</LinearRing>");
        sb.AppendLine($@"			</outerBoundaryIs>");
        sb.AppendLine($@"		</Polygon>");
        sb.AppendLine($@"	</Placemark>");
        sb.AppendLine($@"</Document>");
        sb.AppendLine($@"</kml>");

        return sb.ToString();
    }

    public MainViewModel()
    {
        StartDir = "";
        FileXYZFullName = "";
        FileXYZSelected = false;
        FileKMLToCreateFromXYZFullName = "";
        FileKMLCreatedExist = false;
        FileKMLFullName = "";
        FileKMLSelected = false;
        FileXYZToCreateFromKMLFullName = "";
        FileXYZCreatedExist = false;
        FileXYZWithinPolygonFullName = "...\\Testing.xyz";
        FileXYZWithinPolygonName = "Testing.xyz";
        GoogleEarthPolygonText = GetGooglePolygonExample();
        StatusCreateKML = "";
        StatusCreateXYZ = "";
        StatusCreateXYZWithinPolygon = "";
        Status = "";
        Log = "";
    }
}
