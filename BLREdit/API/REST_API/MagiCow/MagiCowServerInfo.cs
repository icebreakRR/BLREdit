﻿using BLREdit.API.REST_API.Server;
using BLREdit.Import;
using BLREdit.UI.Windows;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace BLREdit.API.REST_API.MagiCow;

public sealed class MagiCowServerInfo : ServerInfo
{
    public int BotCount { get; set; }
    public string GameMode { get; set; }
    public int GoalScore { get; set; }
    public string Map { get; set; }
    public int MaxPlayers { get; set; }
    public int PlayerCount { get; set; }
    public string Playlist { get; set; }
    public string ServerName { get; set; }
    public string MatchStats { get { return $"Time: {GetTimeDisplay()}\nLeader: {GetScoreDisplay()}"; } }


    public bool IsOnline { get; set; } = false;

    private BLRMap map;
    [JsonIgnore] public BLRMap BLRMap
    {
        get
        {
            if (map is null) { foreach (var m in MapModeSelect.Maps) { if (m.MagiCowName.ToLower() == Map.ToLower()) { map = m; break; } } }
            return map;
        }
    }

    private BLRMode mode;
    [JsonIgnore] public BLRMode BLRMode
    {
        get
        {
            if (mode is null) { foreach (var m in MapModeSelect.Modes) { if (m.ModeName.ToLower() == GameMode.ToLower()) { mode = m; break; } } }
            return mode;
        }
    }

    private ObservableCollection<string> list;

    [JsonIgnore] public ObservableCollection<string> List
    {
        get
        {
            if (list is null) { list = new() { $"{PlayerCount}/{MaxPlayers} Players" }; foreach (var team in TeamList) { foreach (var player in team.PlayerList) { list.Add($"[{player.Name}]: ({player.Score}) {player.Kills}/{player.Deaths}"); } } }
            return list;
        }
    }

    private ObservableCollection<string> team1list;
    [JsonIgnore] public ObservableCollection<string> Team1List
    {
        get
        {
            if (team1list is null && TeamList.Count > 0) { team1list = new() { $"{TeamList[0].PlayerCount}/{MaxPlayers / 2} Team 1" }; foreach (var player in TeamList[0].PlayerList) { team1list.Add($"[{player.Name}]: ({player.Score}) {player.Kills}/{player.Deaths}"); } }
            return team1list;
        }
    }

    private ObservableCollection<string> team2list;
    [JsonIgnore] public ObservableCollection<string> Team2List
    {
        get
        {
            if (team2list is null && TeamList.Count > 1) { team2list = new() { $"{TeamList[1].PlayerCount}/{MaxPlayers / 2} Team 2" }; foreach (var player in TeamList[1].PlayerList) { team2list.Add($"[{player.Name}]: ({player.Score}) {player.Kills}/{player.Deaths}"); } }
            return team2list;
        }
    }

    public override string ToString()
    {
        return LoggingSystem.ObjectToTextWall(this);
    }
}