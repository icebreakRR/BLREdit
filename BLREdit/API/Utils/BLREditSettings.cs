﻿using BLREdit.Export;
using BLREdit.Game;
using BLREdit.UI;

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;

namespace BLREdit;

public sealed class BLREditSettings : INotifyPropertyChanged
{
    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    #endregion Events

    private static readonly BLREditSettings settings = IOResources.DeserializeFile<BLREditSettings>($"{IOResources.SETTINGS_FILE}") ?? new();
    public static BLREditSettings Settings { get { return settings; } }

    //Saves The Default Client will get validatet after GameClients have been loaded to make sure it's still a valid client
    private BLRClient client = null;
    public BLRClient DefaultClient { get { return client; } set { client = value; OnPropertyChanged(); } }
    //Saves the Default Server (not in use anymore)
    private BLRServer server = null;
    public BLRServer DefaultServer { get { return server; } set { server = value; OnPropertyChanged(); } }

    //Toggles Advanced Modding
    public UIBool AdvancedModding { get; set; } = new(false);

    //Toggles Debugging (has no use currently)
    public UIBool Debugging { get; set; } = new(false);

    //Toggle to Show if Important Runtime is missing
    public UIBool DoRuntimeCheck { get; set; } = new(true);

    //Forces Display of needed Runtimes
    public UIBool ForceRuntimeCheck { get; set; } = new(false);

    //Might not do anything anymore.
    public UIBool ShowUpdateNotice { get; set; } = new(true);

    //Toggles Advanced Modding
    public UIBool ServerWatchDog{ get; set; } = new(false);

    //Filters Modules that are not in the GitHub AvailableModule List
    public UIBool StrictModuleChecks { get; set; } = new(true);

    //Adds modules that are in the Module Folder of the Client to the installed Module list and adds them to Proxy Load order
    public UIBool AllowCustomModules { get; set; } = new(false);

    //BotCount for Server Start
    private int botCount = 8;
    public int BotCount { get { return botCount; } set { botCount = value; OnPropertyChanged(); } }
    //PlayerCount for Server Start
    private int playerCount = 16;
    public int PlayerCount { get { return playerCount; } set { playerCount = value; OnPropertyChanged(); } }





    public static LaunchOptions GetLaunchOptions()
    {
        return new LaunchOptions() { UserName = ExportSystem.ActiveProfile.PlayerName, Server=Settings.DefaultServer };
    }

    public static void SyncDefaultClient()
    {
        foreach (var client in MainWindow.GameClients)
        {
            if (client.OriginalPath == Settings.DefaultClient.OriginalPath)
            {
                Settings.DefaultClient = client;
                return;
            }
        }
    }

    public static void Save()
    {
        if (MainWindow.GameClients is not null && MainWindow.GameClients.Count > 0)
        {
            bool client = false;
            foreach (var c in MainWindow.GameClients)
            {
                if (c.Equals(Settings.DefaultClient))
                {
                    client = true;
                }
            }
            if (!client) { Settings.DefaultClient = null; }

            IOResources.SerializeFile($"{IOResources.SETTINGS_FILE}", Settings);
        }
    }
}