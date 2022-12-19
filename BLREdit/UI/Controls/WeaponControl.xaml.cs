﻿using BLREdit.UI.Views;

using System;
using System.Resources;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BLREdit.UI.Controls;

/// <summary>
/// Interaction logic for WeaponControl.xaml
/// </summary>
public sealed partial class WeaponControl : UserControl, INotifyPropertyChanged
{
    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion Events

    private Visibility gripVisibility = Visibility.Collapsed;
    public Visibility GripVisibility { get { return gripVisibility; } private set { gripVisibility = value; OnPropertyChanged(); } }

    private Visibility tagVisibility = Visibility.Visible;
    public Visibility TagVisibility { get { return tagVisibility; } private set { tagVisibility = value; OnPropertyChanged(); } }

    private Visibility skinVisibility = Visibility.Visible;
    public Visibility SkinVisibility { get { return skinVisibility; } private set { skinVisibility = value; OnPropertyChanged(); } }

    public WeaponControl()
    {
        InitializeComponent();
        BLREditSettings.Settings.AdvancedModding.PropertyChanged += SettingsChanged;
        GripVisibility = BLREditSettings.Settings.AdvancedModding.Visibility;
    }

    public void SetVisibility(bool isPrimary = true)
    {
        if (isPrimary)
        {
            GripVisibility = Visibility.Collapsed;
            TagVisibility = Visibility.Visible;
            SkinVisibility = Visibility.Visible;
        }
        else 
        {
            GripVisibility = Visibility.Visible;
            TagVisibility = Visibility.Collapsed;
            SkinVisibility = Visibility.Collapsed;
        }
    }

    public void SettingsChanged(object sender, PropertyChangedEventArgs e)
    {
        LoggingSystem.Log($"Recieved Event Property:{e.PropertyName}");
        if (e.PropertyName == nameof(BLREditSettings.Settings.AdvancedModding.Visibility))
        {
            if (DataContext is BLRWeapon weapon)
            {
                if (weapon.IsPrimary)
                {
                    GripVisibility = BLREditSettings.Settings.AdvancedModding.Visibility;
                }
            }
        }
    }

    private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        ScopePreviewImage.DataContext = this.DataContext;
    }

    public static int PrimarySelectedBorder { get; private set; } = 6;
    public static int SecondarySelectedBorder { get; private set; } = -1;
    private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.Source is Image image)
        {
            if (image.Parent is Border border)
            {
                if (DataContext is BLRWeapon weapon)
                {
                    if (weapon.IsPrimary)
                    {
                        PrimarySelectedBorder = this.ControlGrid.Children.IndexOf(border);
                        LoggingSystem.Log(PrimarySelectedBorder.ToString());
                    }
                    else
                    {
                        SecondarySelectedBorder = this.ControlGrid.Children.IndexOf(border);
                    }
                }
            }
        }
    }

    internal void ApplyBorder()
    {
        if (DataContext is BLRWeapon weapon)
        {
            int index;

            if (weapon.IsPrimary)
            {
                index = PrimarySelectedBorder;
            }
            else
            {
                index = SecondarySelectedBorder;
            }

            if (index > -1 && index < this.ControlGrid.Children.Count && this.ControlGrid.Children[index] is Border border)
            {
                MainWindow.LastSelectedBorder = border;
            }
        }

    }
}
