using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using Clf.Common.ImageProcessing;
using Clf.Blazor.Basic.Components.Controls.Enums;

namespace Clf.Blazor.Basic.Components.Controls.Helpers
{
  public static class Utilities
  {
    public static Colour GetColourFromBorderStatus(BorderStatus pvStatus)
    {
      switch (pvStatus)
      {
        case BorderStatus.NotConnected:
          return new Colour() { Red = 200, Green = 0, Blue = 200, Alpha = 200 };
        case BorderStatus.MinorAlarm:
          return new Colour() { Red = 255, Green = 128, Blue = 0, Alpha = 255 };
        case BorderStatus.MajorAlarm:
          return new Colour() { Red = 255, Green = 0, Blue = 0, Alpha = 255 };
        case BorderStatus.Invalid:
          return new Colour() { Red = 165, Green = 165, Blue = 165, Alpha = 255 };
        case BorderStatus.Connected:
        default:
          return Colour.Transparent;
      }

    }

    public static bool GetBorderStatusDisable(BorderStatus borderStatus)
    {
      switch (borderStatus)
      {
        case BorderStatus.NotConnected:
          return true;
        case BorderStatus.Connected:
        case BorderStatus.MajorAlarm:
        case BorderStatus.MinorAlarm:
        case BorderStatus.Invalid:
        default:
          return false;
      }
    }

    public static string GetButtonClass(ButtonType buttonType)
    {
      switch (buttonType)
      {
        case ButtonType.Navigation:
          return "clf-button-navigation";
        case ButtonType.Transparent:
          return "clf-button-transparent";
        case ButtonType.Default:
        default:
          return string.Empty;
      }
    }
  }
}