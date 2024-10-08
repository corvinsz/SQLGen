﻿using Microsoft.Extensions.DependencyInjection;
using SQLGen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SQLGen.Views.Dialogs;
/// <summary>
/// Interaction logic for SettingsControl.xaml
/// </summary>
public partial class SettingsDialog : UserControl
{
    public SettingsDialog()
    {
        InitializeComponent();
        this.DataContext = App.ServiceProvider.GetRequiredService<SettingsViewModel>();
    }
}
