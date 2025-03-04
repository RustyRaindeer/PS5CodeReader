using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.VisualStudio.Threading;
using System.Globalization;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace PS5CodeReader
{
    public partial class Form1 : Form
    {
        private readonly string registryKeyName = @"SOFTWARE\RustyRaindeer\PS5CodeReader";
        private readonly string FileNameCache = @"cache.json";
        private readonly string StrAuto = @"Auto";
        private PS5ErrorCodeList? errorCodeList;
        private CancellationTokenSource? cancellationTokenSource;
        private UInt32 firstErrorTimestamp;
        private Int32 errorDatabaseIndex = -1;

        /*
         * Possible Commands
         * version
         * bringup
         * shutdown
         * firmud
         * bsn
         * halt
         * cp ready
         * cp busy
         * cp reset
         * bestat
         * powersw
         * resetsw
         * bootbeep stat
         * bootbeep on
         * bootbeep off
         * reset syscon
         * xdrdiag info
         * xdrdiag start
         * xdrdiag result
         * xiodiag
         * fandiag
         * errlog 
         * readline
         * tmpforcp <zone id>
         * cp beepreote
         * cp beep2kn1n3
         * cp beep2kn2n3
         * csum
         * osbo
         * scopen
         * scclose
         * ejectsw
         */

        public void saveSelectedErrorDatabaseIndexToRegistry(Int32 value)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyName, true);
            if (key != null)
            {
                key.SetValue("ErrorDatabaseIndex", value, RegistryValueKind.DWord);
                key.Close();
            }
        }
        public void initRegistry()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyName);

            //if it does exist, retrieve the stored values  
            if (key != null)
            {
                errorDatabaseIndex = int.Parse(key.GetValue("ErrorDatabaseIndex").ToString());
                key.Close();
            }
            else
            {
                key = Registry.CurrentUser.CreateSubKey(registryKeyName);

                key.SetValue("ErrorDatabaseIndex", errorDatabaseIndex);
                key.Close();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            initRegistry();
            LoadOperationTypes();
            LoadPorts();
            ComboBoxOperationType.SelectedValueChanged += ComboBoxOperationType_SelectedValueChanged;
            ComboBoxDatabase.SelectedIndex = errorDatabaseIndex; // ComboBoxDatabase.Items.IndexOf("RustyRaindeer/PS5CodeReader/master/ErrorCodes.json");
            await GetErrorCodesListAsync();
            if (errorCodeList == default)
            {
                LogBox.AppendLine("[-] No Errors List Loaded, Close Application and Try Again!");
            }
            else
            {
                LogBox.AppendLine("[+] Please connect your Playstation 5 to UART do not power up the console.", ReadOnlyRichTextBox.ColorInformation);
            }
            ComboBoxDatabase.SelectedValueChanged += ComboBoxDatabase_SelectedValueChanged;
        }

        private void ComboBoxOperationType_SelectedValueChanged(object? sender, EventArgs e)
        {
            PanelRawCommand.Visible = ComboBoxOperationType.SelectedValue is OperationType type && type == OperationType.RunRawCommand;
            PanelInterpretError.Visible = ComboBoxOperationType.SelectedValue is OperationType type2 && type2 == OperationType.InterpretError;
        }

        private void ComboBoxDatabase_SelectedValueChanged(object? sender, EventArgs e)
        {
            GetErrorCodesListAsync();
        }

        private void ComboBoxDevices_DropDown(object sender, EventArgs e)
        {
            LoadPorts();
        }


        #region Data Source Information

        private void LoadPorts()
        {
            ComboBoxDevices.DataSource = SerialPort.SelectSerial();
        }

        private void LoadOperationTypes()
        {
            ComboBoxOperationType.EnumForComboBox<OperationType>();
            ComboBoxOperationType.DisplayMember = "Description";
            ComboBoxOperationType.ValueMember = "Value";
        }

        #endregion

        #region Error Codes List From Server

        private async Task GetErrorCodesListAsync()
        {
            LogBox.AppendLine("[+] Loading Errors List", ReadOnlyRichTextBox.ColorInformation);
            errorCodeList = default;
            try
            {
                LogBox.Append($"Attempting to load from server {ComboBoxDatabase.SelectedItem} ...");
                errorCodeList = await GetErrorCodesGitHubAsync($"{ComboBoxDatabase.SelectedItem}");
                if (errorCodeList != default)
                {
                    LogBox.Okay();

                    Boolean valueChanged = (ComboBoxDatabase.SelectedIndex != errorDatabaseIndex);
                    errorDatabaseIndex = ComboBoxDatabase.SelectedIndex;

                    if (valueChanged)
                    {
                        saveSelectedErrorDatabaseIndexToRegistry(errorDatabaseIndex);
                    }

                    //Store errorCode list as a chache.
                    await CacheErrorListLocalAsync(valueChanged);
                }
                else
                {
                    if (ComboBoxDatabase.SelectedIndex != errorDatabaseIndex)
                    {
                        LogBox.AppendLine("[-] FAILED, Reverting back to previous database selection", ReadOnlyRichTextBox.ColorError);
                        ComboBoxDatabase.SelectedIndex = errorDatabaseIndex;
                    }
                    LogBox.Fail();
                }
            }
            catch
            {
                if (ComboBoxDatabase.SelectedIndex != errorDatabaseIndex)
                {
                    LogBox.AppendLine("[-] FAILED, Reverting back to previous database selection", ReadOnlyRichTextBox.ColorError);
                    ComboBoxDatabase.SelectedIndex = errorDatabaseIndex;
                }
                LogBox.Fail();
                //todo: Error Handling
                //Attempt to get error codes from server failed. 
                //Lets get errorCodes from a cached local file. 
                errorCodeList = await GetErrorCodesCacheAsync();
            }
            if (errorCodeList != default && errorCodeList.PlayStation5 != null && errorCodeList.PlayStation5.ErrorCodes.Any())
            {
                LogBox.AppendLine($"[+] Loaded {errorCodeList.PlayStation5.ErrorCodes.Count} Errors Succesfully.", ReadOnlyRichTextBox.ColorSuccess);
            }
            else
            {
                LogBox.AppendLine("[-] Failed to load Errors List.", ReadOnlyRichTextBox.ColorError);
            }
        }

        /// <summary>
        /// Get List of Error Codes for the PS5 from Git hub server.
        /// </summary>
        /// <returns>Error Code List</returns>
        private static async Task<PS5ErrorCodeList?> GetErrorCodesGitHubAsync(string fileRef)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://raw.githubusercontent.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; PS5CodeReader/2.1; +https://github.com/amoamare)");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(fileRef);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PS5ErrorCodeList>();
        }


        /// <summary>
        /// Gets a list of Error Codes from Cached File on System
        /// </summary>
        /// <returns></returns>
        private async Task<PS5ErrorCodeList?> GetErrorCodesCacheAsync()
        {
            LogBox.Append("Loading Errors List From Cached File...");
            if (!File.Exists(FileNameCache))
            {
                LogBox.Fail();
                LogBox.AppendText("No Cached File Saved!");
                return default;
            }
            using var stream = File.OpenRead(FileNameCache);
            var cached = await JsonSerializer.DeserializeAsync<PS5ErrorCodeList>(stream);
            if (cached == default || cached.PlayStation5 != default && !cached.PlayStation5.ErrorCodes.Any())
            {
                LogBox.Fail();
                return default;
            }
            LogBox.Okay();
            return cached;
        }


        /// <summary>
        /// Save error codes to a cached file on system
        /// </summary>
        /// <param name="fileName">cached.json</param>
        /// <returns></returns>
        private async Task SaveCacheFileAsync(string fileName)
        {
            using var stream = File.Create(fileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(stream, errorCodeList, options: options);
            await stream.DisposeAsync();
            return; // only need to store it as a cahce first time creating it
        }


        /// <summary>
        /// Cachce error list on local disk
        /// </summary>
        /// <returns></returns>
        private async Task CacheErrorListLocalAsync(Boolean databaseSwap)
        {
            if (errorCodeList == default)
            {
                //Can't save what we don't have right?
                return;
            }
            if (!File.Exists(FileNameCache) && errorCodeList != default)
            {
                LogBox.Append("Creating new errors list cache file...");
                await SaveCacheFileAsync(FileNameCache);
                LogBox.Okay();
                return; // only need to store it as a cahce first time creating it
            }
            else
            {
                LogBox.Append("Comparing cached version from server version...");
                //Lets open and serialize the revision to compare if we need to update the cached file. 
                using var stream = File.OpenRead(FileNameCache);
                var cached = await JsonSerializer.DeserializeAsync<PS5ErrorCodeList>(stream);
                if (cached == default || errorCodeList == default)
                {
                    LogBox.Fail();
                    //todo: Update error handling
                    return;
                }
                LogBox.Okay();
                if (cached.Revision < errorCodeList.Revision || databaseSwap)
                {
                    LogBox.AppendLine($"Cached Version: {cached.Revision}.");
                    LogBox.AppendLine($"Server Version: {errorCodeList.Revision}.");
                    LogBox.Append("Updating cached version with server...");
                    //Our downloaded error codes have updated. Lets update the cached version.
                    await stream.DisposeAsync();
                    try
                    {
                        File.Delete(FileNameCache);
                    }
                    catch
                    {
                        try
                        {
                            File.Move(FileNameCache, $"{FileNameCache}.old");
                        }
                        catch
                        {
                            //todo: update error handling if we can not delete or move the file.
                            LogBox.Fail();
                            return;
                        }
                    }
                    if (!File.Exists(FileNameCache))
                    {
                        //safe to create new file.
                        await SaveCacheFileAsync(FileNameCache);
                        LogBox.Okay();
                    }
                }
            }
        }

        #endregion

        private bool InterfaceState
        {
            set
            {
                ButtonRunOperation.Text = value ? @"Run Operation" : @"Cancel";
                ButtonRunOperation.Tag = !value;
                ComboBoxDevices.Enabled = value;
                ComboBoxOperationType.Enabled = value;
                ComboBoxDatabase.Enabled = value;
                TextBoxRawCommand.Enabled = !value;
                TextBoxInterpretError.Enabled = !value;
            }
        }

        private async Task<Device?> AutoDetectDeviceAsync()
        {
            Device? device = ComboBoxDevices.SelectedItem as Device;
            if (device == default || errorCodeList == null) return default;
            var autoDetect = device.Port.StartsWith(StrAuto, StringComparison.InvariantCultureIgnoreCase);
            if (!autoDetect) return device;
            var devices = ComboBoxDevices.Items.OfType<Device>().ToList();
            devices.Remove(device); // remove the auto detect device. 
            if (devices.Count == 1) return devices.FirstOrDefault(); //if only 1 device is detected we can just skipp detecting it.
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            foreach (var autoDevice in devices)
            {
                LogBox.AppendLine($"[*] Auto Detecting Playstation 5 on {autoDevice}", ReadOnlyRichTextBox.ColorInformation);
                LogBox.AppendLine("\t- Disconnect power cord from PS5\r\n\t- Wait 5 seconds.\r\n\t- Connect Power to PS5 due not power on!", ReadOnlyRichTextBox.ColorError);
                using var serial = new SerialPort(autoDevice.Port);
                LogBox.Append($"Opening Device on {autoDevice.FriendlyName}...");
                serial.Open();
                LogBox.Okay();
                LogBox.AppendLine("[*] Listening for Playstation 5.", ReadOnlyRichTextBox.ColorInformation);
                List<string> Lines = new();
                do
                {
                    try
                    {
                        var line = await serial.ReadLineAsync(cancellationTokenSource.Token);
                        Lines.Add(line);
                    }
                    catch (OperationCanceledException)
                    {
                        cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                        await serial.SendBreakAsync(cancellationToken: cancellationTokenSource.Token);
                        var line = await serial.ReadLineAsync(cancellationTokenSource.Token);
                        Lines.Add(line);
                    }
                } while (serial.BytesToRead != 0);

                var flag = Lines.Any(x => x.StartsWith(@"$$ [MANU] UART CMD READY:36") || x.StartsWith(@"NG E0000003:4D") || x.StartsWith("OK 00000000:3A"));
                if (flag)
                {
                    LogBox.AppendLine($@"[+] Detected a Playstation 5 on {autoDevice.FriendlyName}", ReadOnlyRichTextBox.ColorSuccess);
                    ComboBoxDevices.SelectedItem = autoDevice;
                    return autoDevice;
                }
            }
            return default;
        }

        private async void ButtonRunOperations_Click(object sender, EventArgs e)
        {
            if (ComboBoxOperationType.SelectedValue is not OperationType type) return;
            try
            {
                if (ButtonRunOperation.Tag is not null && ButtonRunOperation.Tag is bool cancel && cancel
                    && cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel(false);
                    return;
                }

                LogBox.Clear();
                InterfaceState = false;
                await RunOperationsAsync(type);
            }
            catch (OperationCanceledException)
            {
                LogBox.AppendLine("[!] Operation Cancelled");
            }
            catch (Exception ex)
            {
                //todo: add error handling
                Debug.WriteLine(ex);
            }
            finally
            {
                InterfaceState = true;
            }
        }

        private async Task RunOperationsAsync(OperationType type)
        {
            switch (type)
            {
                default: return;
                case OperationType.ReadErrorCodes:
                    LogBox.AppendLine("[*] Operation: Read UART Codes.", ReadOnlyRichTextBox.ColorError);
                    await ReadCodesAsync();
                    break;
                case OperationType.ReadAllErrorCodes:
                    LogBox.AppendLine("[*] Operation: Read UART Codes (full log).", ReadOnlyRichTextBox.ColorError);
                    await ReadCodesAsync(64);
                    break;
                case OperationType.ClearErrorCodes:
                    LogBox.AppendLine("[*] Operation: Clear UART Codes.", ReadOnlyRichTextBox.ColorError);
                    await ClearLogsAsync();
                    break;
                case OperationType.MonitorMode:
                    LogBox.AppendLine("[*] Operation: Running Monitor Mode.", ReadOnlyRichTextBox.ColorError);
                    await RunMonitorModeAsync();
                    break;
                case OperationType.RunCommandList:
                    LogBox.AppendLine("[*] Operation: Run Command List.", ReadOnlyRichTextBox.ColorError);
                    await RunCommmandListAsync();
                    break;
                case OperationType.RunRawCommand:
                    LogBox.AppendLine("[*] Operation: Run Raw Command.", ReadOnlyRichTextBox.ColorError);
                    await RunRawCommandAsync();
                    break;
                case OperationType.InterpretError:
                    LogBox.AppendLine("[*] Operation: Interpret Error Code.", ReadOnlyRichTextBox.ColorError);
                    await InterpretErrorAsync();
                    break;

            }
        }

        #region Run Operation Types

        /// <summary>
        /// Read past 10 error logs
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private async Task ReadCodesAsync(int count = 10)
        {
            var device = await AutoDetectDeviceAsync();
            if (device == default)
            {
                LogBox.AppendLine("[-] No Playstation 5 Detected!", ReadOnlyRichTextBox.ColorError);
                return;
            }
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var serial = new SerialPort(device.Port);
            serial.Open();
            firstErrorTimestamp = 0xffffffff;

            for (var i = 0; i < count; i++)
            {
                LogBox.Append($"slot#{i}: ");
                var command = $"errlog {i:X}";
                var checksum = SerialPort.CalculateChecksum(command);

                await serial.WriteLineAsync(command);
                do
                {
                    var line = await serial.ReadLineAsync(cancellationTokenSource.Token);

                    if (!string.Equals($"{command}:{checksum:X2}", line, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //ignore the echo'd command capture everything else. 
                        ShowLineDetail(line);
                    }
                } while (serial.BytesToRead != 0);
            }

            LogBox.SelectionStart = 0;
            LogBox.ScrollToCaret();
        }

        private void ShowLineDetail(string l)
        {
            if (ShowErrorLine.Checked)
            {
                LogBox.AppendLine(l);
            }
            var split = l.Split(' ');
            if (!split.Any()) return;
            switch (split[0])
            {
                case "NG":
                    LogBox.AppendLine("Failed to read data");
                    break;
                case "OK":

                    var errorCode = split[2];

                    if (!ShowErrorLine.Checked)
                    {
                        LogBox.Append($"{errorCode} ");
                    }
                    if (errorCode == "FFFFFFFF")
                    {
                        LogBox.AppendLine("     (empty slot)", ReadOnlyRichTextBox.ColorInformation);
                    }
                    else
                    {
                        String tmStr = "(time unknown)";
                        UInt32 tm = UInt32.Parse(split[3], NumberStyles.HexNumber);
                        if (tm.Equals(0xffffffff))
                        {
                            // This line has no time stamp ?!. Reset reference.
                            firstErrorTimestamp = tm;
                        }
                        else
                        {
                            if (firstErrorTimestamp.Equals(0xffffffff) || tm > firstErrorTimestamp)
                            {
                                if (firstErrorTimestamp.Equals(0xffffffff))
                                {
                                    tmStr = "(latest/newest error)";
                                }
                                else
                                {
                                    tmStr = "(time stamp reset)";
                                }

                                firstErrorTimestamp = tm;
                            }
                            else
                            {
                                tm = firstErrorTimestamp - tm;
                                UInt32 sec = tm % 60;
                                UInt32 min = (tm / 60) % 60;
                                UInt32 hour = (tm / (60 * 60)) % 24;
                                UInt32 day = tm / (60 * 60 * 24);

                                tmStr = $"- {day}d{hour}h{min}m{sec}s";
                            }
                        }

                        LogBox.Append($" {tmStr}: ", ReadOnlyRichTextBox.ColorDetail);

                        var pwrState = UInt32.Parse(split[4], NumberStyles.HexNumber);
                        string pwrStateStr = "";
                        var osState = (pwrState >> 16) & 0xFF;
                        if (osState == 0x00) pwrStateStr = "SystemReady|";
                        else if (osState == 0x01) pwrStateStr = "MainOnStandby|";
                        else if (osState >= 0x02 && osState <= 0x0f) pwrStateStr = "Reserved|";
                        else if (osState >= 0x10 && osState <= 0x1f) pwrStateStr = "PSP|";
                        else if (osState >= 0x20 && osState <= 0x3f) pwrStateStr = "BIOS|";
                        else if (osState == 0x40) pwrStateStr = "EAPReady|";
                        else if (osState >= 0x41 && osState <= 0x4f) pwrStateStr = "EAP|";
                        else if (osState >= 0x50 && osState <= 0xbf) pwrStateStr = "Kernel|";
                        else if (osState >= 0xc0 && osState <= 0xfe) pwrStateStr = "InitProcess|";
                        else if (osState == 0xff) pwrStateStr = "HostOsOff|";
                        var sysState = (pwrState & 0xffff);
                        if (sysState == 0x0000) pwrStateStr += "ACIN_L";
                        else if (sysState == 0x0001) pwrStateStr += "Standby";
                        else if (sysState == 0x0002) pwrStateStr += "PG2_ON";
                        else if (sysState == 0x0003) pwrStateStr += "EFC_ON";
                        else if (sysState == 0x0004) pwrStateStr += "EAP_ON";
                        else if (sysState == 0x0005) pwrStateStr += "SoC_ON";
                        else if (sysState == 0x0006) pwrStateStr += "ErrorDetected";
                        else if (sysState == 0x0007) pwrStateStr += "FatalError";
                        else if (sysState == 0x0008) pwrStateStr += "NeverBoot";
                        else if (sysState == 0x0009) pwrStateStr += "ForcedOff";
                        else if (sysState == 0x000a) pwrStateStr += "BT_FMW_DL";
                        else pwrStateStr += $"?({sysState:X}h)?";
                        LogBox.Append(" PwrState:");
                        LogBox.Append($"{pwrStateStr} ", ReadOnlyRichTextBox.ColorDetail);

                        var upCause = UInt32.Parse(split[5], NumberStyles.HexNumber);
                        string upCstr = "??";
                        if ((upCause) == 0) upCstr = "N/A";
                        if ((upCause & 0x00000001) != 0) upCstr = "PSUPwrOn";
                        if ((upCause & 0x00000100) != 0) upCstr = "PwrButton";
                        if ((upCause & 0x00000200) != 0) upCstr = "DiscLoaded";
                        if ((upCause & 0x00000400) != 0) upCstr = "EjectButton";
                        if ((upCause & 0x00010000) != 0) upCstr = "SoC_order";
                        if ((upCause & 0x00020000) != 0) upCstr = "EAP_order";
                        if ((upCause & 0x00040000) != 0) upCstr = "HDMI-CEC";
                        if ((upCause & 0x00080000) != 0) upCstr = "BT-Dualsense";
                        if ((upCause & 0x04000000) != 0) upCstr = "UARTCommand";
                        LogBox.Append(" UpCause:");
                        LogBox.Append($"{upCstr} ", ReadOnlyRichTextBox.ColorDetail);

                        var devPM = UInt16.Parse(split[7], NumberStyles.HexNumber);
                        string devPMstr = "";
                        if ((devPM & 0x10) != 0) devPMstr += "H"; else devPMstr += "_";
                        if ((devPM & 0x08) != 0) devPMstr += "B"; else devPMstr += "_";
                        if ((devPM & 0x04) != 0) devPMstr += "C"; else devPMstr += "_";
                        if ((devPM & 0x02) != 0) devPMstr += "U"; else devPMstr += "_";
                        if ((devPM & 0x01) != 0) devPMstr += "W"; else devPMstr += "_";
                        LogBox.Append(" DevPM:");
                        LogBox.Append($"{devPMstr} ", ReadOnlyRichTextBox.ColorDetail);

                        var cpuTemp = split[8];
                        if (cpuTemp == "0000" || cpuTemp == "FFFF")
                        {
                            LogBox.Append(" TSoC:");
                            LogBox.Append("*N/A* ", ReadOnlyRichTextBox.ColorDetail);

                        }
                        else
                        {
                            Double cpuTempD = Math.Round(UInt16.Parse(cpuTemp, NumberStyles.HexNumber) / 256.0, 1);
                            LogBox.Append(" TSoC:");
                            LogBox.Append($"{cpuTempD}�C ", ReadOnlyRichTextBox.ColorDetail);
                        }

                        var envTemp = split[9].Split(':')[0]; ;
                        if (envTemp == "0000" || envTemp == "FFFF")
                        {
                            LogBox.Append(" TEnv:");
                            LogBox.AppendLine("*N/A* ", ReadOnlyRichTextBox.ColorDetail);
                        }
                        else
                        {
                            Double envTempD = Math.Round(UInt16.Parse(envTemp, NumberStyles.HexNumber) / 256.0, 1);
                            LogBox.Append(" TEnv:");
                            LogBox.AppendLine($"{envTempD}�C ", ReadOnlyRichTextBox.ColorDetail);
                        }

                        try
                        {
                            //var errorLookup = errorCodeList.PlayStation5.ErrorCodes.First(x => x.ID == errorCode);
                            var errorLookup = errorCodeList.PlayStation5.ErrorCodes.First(x => Regex.IsMatch(errorCode, x.ID));
                            String msg = errorLookup.Message;
                            // now, let's replace the error detail "#XYZ" in the message string with the corresponding
                            // part of the error code.
                            String matcher = "#[XYZ]+";
                            Match m = Regex.Match(msg, matcher);
                            if (m.Success && m.Value.Length < 8)
                            {
                                var len = m.Value.Length;
                                // get as many digits from the end of the error code as there are letters X,Y,Z in the message string
                                var code = $"#{errorCode.Substring(errorCode.Length - len + 1)}";
                                var ch = m.Value[m.Length - 1];
                                // add a hyphen between any sequence of X,Y and Z
                                for (int i = len - 1; i > 0; --i)
                                {
                                    if (m.Value[i] != ch)
                                    {
                                        code = code.Insert(i + 1, "-");
                                        ch = m.Value[i];
                                    }
                                }
                                // and last, replace the #XYZ in message string with the corresponding code
                                msg = Regex.Replace(msg, matcher, $"{code}");
                            }
                            LogBox.AppendLine($"{msg}", ReadOnlyRichTextBox.ColorSuccess);
                            LogBox.AppendLine("");
                        }
                        catch
                        {
                            LogBox.AppendLine("Unknown Error", ReadOnlyRichTextBox.ColorInformation);
                            LogBox.AppendLine("");
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Clears Error Logs
        /// </summary>
        /// <returns></returns>
        private async Task ClearLogsAsync()
        {
            var device = await AutoDetectDeviceAsync();
            if (device == default)
            {
                LogBox.AppendLine("[-] No Playstation 5 Detected!", ReadOnlyRichTextBox.ColorError);
                return;
            }
            cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var serial = new SerialPort(device.Port);
            serial.Open();
            LogBox.Append("[+]\tClearing Logs...", ReadOnlyRichTextBox.ColorInformation);
            var command = "errlog clear";
            var checksum = SerialPort.CalculateChecksum(command);
            await serial.WriteLineAsync("errlog clear", cancellationTokenSource.Token);
            string? response = default;
            do
            {
                var line = await serial.ReadLineAsync(cancellationTokenSource.Token);
                if (!string.Equals($"{command}:{checksum:X2}", line, StringComparison.InvariantCultureIgnoreCase))
                {
                    //ignore the echo'd command capture everything else. 
                    response = line;
                }
            } while (serial.BytesToRead != 0);
            var split = response?.Split(' ');
            if (split == default || split.Any())
            {
                LogBox.Okay();
                return;
            }
            switch (split[0])
            {
                case "NG":
                    LogBox.Fail();
                    break;
                case "OK":
                    LogBox.Okay();
                    break;
            }
        }


        /// <summary>
        /// Run in monitor mode. This will listen to anything the console might be saying. 
        /// </summary>
        /// <returns></returns>
        private async Task RunMonitorModeAsync()
        {
            var device = await AutoDetectDeviceAsync();
            if (device == default)
            {
                LogBox.AppendLine("[-] No Playstation 5 Detected!", ReadOnlyRichTextBox.ColorError);
                return;
            }
            cancellationTokenSource = new CancellationTokenSource();
            using var serial = new SerialPort(device.Port);
            serial.Open();
            do
            {
                var line = await serial.ReadLineAsync(cancellationTokenSource.Token);
                LogBox.AppendLine(line);

            } while (!cancellationTokenSource.IsCancellationRequested);
        }

        /// <summary>
        /// Run a list of commands saved in a text file. 
        /// </summary>
        /// <returns></returns>
        private async Task RunCommmandListAsync()
        {
            var device = await AutoDetectDeviceAsync();
            if (device == default)
            {
                LogBox.AppendLine("[-] No Playstation 5 Detected!", ReadOnlyRichTextBox.ColorError);
                return;
            }

            using var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.RestoreDirectory = true;
            ofd.Title = @"Select Command List";
            ofd.DefaultExt = @"txt";
            ofd.Filter = @"txt files (*.txt)|*.txt";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            FileInfo file = new(ofd.FileName);
            using var stream = new StreamReader(file.FullName);
            string? command = default;
            cancellationTokenSource = new CancellationTokenSource();
            using var serial = new SerialPort(device.Port);
            serial.Open();
            do
            {
                command = await stream.ReadLineAsync();
                if (string.IsNullOrEmpty(command)) continue;
                await serial.WriteLineAsync(command, cancellationTokenSource.Token);
                do
                {
                    var response = await serial.ReadLineAsync(cancellationTokenSource.Token);
                    LogBox.AppendLine(response);

                } while (serial.BytesToRead != 0);
            } while (!stream.EndOfStream);
        }


        private readonly AsyncAutoResetEvent AutoResetEventRawCommand = new AsyncAutoResetEvent(false);
        /// <summary>
        /// Run raw command from user. Keeps port open.
        /// </summary>
        /// <returns></returns>
        private async Task RunRawCommandAsync()
        {
            var device = await AutoDetectDeviceAsync();
            if (device == default)
            {
                LogBox.AppendLine("[-] No Playstation 5 Detected!", ReadOnlyRichTextBox.ColorError);
                return;
            }
            using var serial = new SerialPort(device.Port);
            serial.Open();
            do
            {
                cancellationTokenSource = new CancellationTokenSource();
                await AutoResetEventRawCommand.WaitAsync(cancellationTokenSource.Token);
                var command = TextBoxRawCommand.Text.Trim();
                TextBoxRawCommand.Clear();
                cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await serial.WriteLineAsync(command);
                do
                {
                    var line = await serial.ReadLineAsync(cancellationTokenSource.Token);
                    LogBox.AppendLine(line);
                } while (serial.BytesToRead != 0);
            } while (!cancellationTokenSource.IsCancellationRequested);
        }

        private readonly AsyncAutoResetEvent AutoResetEventInterpretError = new AsyncAutoResetEvent(false);
        /// <summary>
        /// Interprets error code line provided by user. 
        /// </summary>
        /// <returns></returns>
        private async Task InterpretErrorAsync()
        {
            do
            {
                cancellationTokenSource = new CancellationTokenSource();
                await AutoResetEventInterpretError.WaitAsync(cancellationTokenSource.Token);
                var command = TextBoxInterpretError.Text.Trim();
                ShowLineDetail(command);
                cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            } while (!cancellationTokenSource.IsCancellationRequested);
        }

        #endregion

        private void TextBoxRawCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!TextBoxRawCommand.Text.Any()) return; // dont send empty commands
            if (e.KeyChar == (char)Keys.Enter)
            {
                AutoResetEventRawCommand.Set();
            }
        }

        private void TextBoxInterpretError_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!TextBoxInterpretError.Text.Any()) return; // dont send empty commands
            if (e.KeyChar == (char)Keys.Enter)
            {
                AutoResetEventInterpretError.Set();
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ComboBoxOperationType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void TextBoxInterpretError_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
