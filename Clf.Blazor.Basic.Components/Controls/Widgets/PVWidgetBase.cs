using Clf.Blazor.Basic.Components.Controls.Models;
using Convergence;
using Convergence.IO.EPICS.CA;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Clf.Blazor.Basic.Components.Controls.Widgets
{
	public class PVWidgetBase : WidgetBase
	{
		[Parameter]
		public string PVName { get; set; } = string.Empty;
		[Parameter]
		public Type PVDataType { get; set; } = typeof(string);
		[Parameter]
		public int PVElementCount { get; set; } = 1;
		[Parameter]
		public bool PVIsDisabled { get; set; } = false;
		[Parameter]
		public BorderStatus PVBorderStatus { get; set; } = BorderStatus.NotConnected;

		public Convergence.IO.EPICS.CA.EventCallbackDelegate.ConnectCallback? ConnectionChangedEvent { get; set; }
		private Convergence.IO.EPICS.CA.EventCallbackDelegate.ConnectCallback? NullConnCallback = null;
		private Convergence.IO.EPICS.CA.EventCallbackDelegate.WriteCallback? NullWriteCallback = null;

		public async Task<EndPointID?> TaskConnectPV()
		{
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName);
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_DOUBLE_f64,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, OnConnectionChanged);
			if (connResult == EndPointStatus.Okay)
			{
				return endpoint;
			}
			else
			{
				return null;
			}

		}

		public async Task<EndPointStatus> TaskPutPV(EndPointID endPointID, object value)
		{
			if (endPointID != null && value != null)
			{
				// Get ValueToWrite from Text and try convert into the chosen System.Type
				object valueToWrite = Convert.ChangeType(value, PVDataType);
				nint valuePtr = GCHandle.Alloc(valueToWrite, GCHandleType.Pinned).AddrOfPinnedObject();
				// Write async and I dont really care about the callback
				EndPointStatus status = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.WriteAsync
					<Convergence.IO.EPICS.CA.EventCallbackDelegate.WriteCallback>(endPointID, valuePtr, NullWriteCallback);
				return status;
			}
			else
			{
				return EndPointStatus.Disconnected;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the precision value.
		/// </summary>
		/// <returns></returns>
		public async Task<int> TaskGetPREC()
		{
			int precision = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".PREC");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_SHORT_i16,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					precision = (int)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return precision;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the HIHI value.
		/// </summary>
		/// <returns></returns>
		public async Task<float> TaskGetHIHI()
		{
			float hihi = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".HIHI");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_FLOAT_f32,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					hihi = (float)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return hihi;
			}
			else
			{
				return 0.0F;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the HIGH value.
		/// </summary>
		/// <returns></returns>
		public async Task<float> TaskGetHIGH()
		{
			float high = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".HIGH");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_FLOAT_f32,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					high = (float)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return high;
			}
			else
			{
				return 0.0F;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the LOLO value.
		/// </summary>
		/// <returns></returns>
		public async Task<float> TaskGetLOLO()
		{
			float lolo = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".LOLO");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_FLOAT_f32,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					lolo = (float)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return lolo;
			}
			else
			{
				return 0.0F;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the LOW value.
		/// </summary>
		/// <returns></returns>
		public async Task<float> TaskGetLOW()
		{
			float low = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".LOW");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_FLOAT_f32,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					low = (float)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return low;
			}
			else
			{
				return 0.0F;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the HOPR value.
		/// </summary>
		/// <returns></returns>
		public async Task<float> TaskHOPR()
		{
			float hopr = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".HOPR");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_FLOAT_f32,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					hopr = (float)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return hopr;
			}
			else
			{
				return 0.0F;
			}
		}

		/// <summary>
		/// Simply connects to the PV and reads the LOPR value.
		/// </summary>
		/// <returns></returns>
		public async Task<float> TaskLOPR()
		{
			float lopr = 0;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".HOPR");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
				datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_FLOAT_f32,
				elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					lopr = (float)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return lopr;
			}
			else
			{
				return 0.0F;
			}
		}

		public async Task<string> TaskGetUnits()
		{
			string units = string.Empty;
			var endpoint = new EndPointID(Protocols.EPICS_CA, PVName + ".EGU");
			var epicsSettings = new Convergence.IO.EPICS.CA.Settings(
								datatype: Convergence.IO.EPICS.CA.DbFieldType.DBF_STRING_s39,
								elementCount: 1);
			var endPointArgs = new EndPointBase<Convergence.IO.EPICS.CA.Settings> { EndPointID = endpoint, Settings = epicsSettings };
			var connResult = await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ConnectAsync(endPointArgs, NullConnCallback);
			if (connResult == EndPointStatus.Okay)
			{
				await Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.ReadAsync<Convergence.IO.EPICS.CA.EventCallbackDelegate.ReadCallback>(endpoint, (rbv) =>
				{
					units = (string)Convergence.IO.EPICS.CA.Helpers.DecodeEventData(rbv);
				});
				// Disconnect the channel
				Convergence.IO.EPICS.CA.ConvergeOnEPICSChannelAccess.Hub.Disconnect(endpoint);
				return units;
			}
			else
			{
				return string.Empty;
			}
		}

	}
}
