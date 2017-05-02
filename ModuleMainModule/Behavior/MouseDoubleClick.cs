﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NLog;
#pragma warning disable 618

namespace ModuleMainModule.Behavior
{
    public class MouseDoubleClick
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command",
        typeof(ICommand),
        typeof(MouseDoubleClick),
        new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
                                                typeof(object),
                                                typeof(MouseDoubleClick),
                                                new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static void SetCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }
        public static object GetCommandParameter(DependencyObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }
        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Control control = target as Control;
                if (control != null)
                {
                    if ((e.NewValue != null) && (e.OldValue == null))
                    {
                        control.MouseDoubleClick += OnMouseDoubleClick;
                    }
                    else if ((e.NewValue == null) && (e.OldValue != null))
                    {
                        control.MouseDoubleClick -= OnMouseDoubleClick;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Behavior", ex);
            }
        }
        private static void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Control control = sender as Control;
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                object commandParameter = control.GetValue(CommandParameterProperty);
                command.Execute(commandParameter);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Behavior", ex);
            }
        }
    }
}