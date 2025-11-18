/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

17/11/2025	1.0.0.1		AMA, Skyline	Initial version
****************************************************************************
*/

namespace Skyline.DataMiner.Exercise.OptimizedSorting.GQI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using DomHelpers.ExerciseTasks;

	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.Exercise.OptimizedSorting.Shared.DOM;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	[GQIMetaData(Name = "Exercise - Optimized Tasks")]
	public sealed class OptimizedSorting : IGQIDataSource, IGQIOnInit
	{
		private static readonly List<GQIColumn> _columns = new List<GQIColumn>
		{
			new GQIStringColumn("ID"),
			new GQIStringColumn("Title"),
			new GQIStringColumn("Description"),
			new GQIIntColumn("Status"),
			new GQIStringColumn("Assignee"),
			new GQIStringColumn("Customer"),
		};

		private IGQILogger _logger;
		private DomHelper _helper;

		public OnInitOutputArgs OnInit(OnInitInputArgs args)
		{
			_logger = args.Logger;
			_helper = new DomHelper(args.DMS.SendMessages, TaskDomMapper.ModuleId);
			return default;
		}

		public GQIColumn[] GetColumns() => _columns.ToArray();

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			try
			{
				var filter = DomInstanceExposers.DomDefinitionId.Equal(TaskDomMapper.DomDefinitionId.Id);
				var instances = _helper.DomInstances.Read(filter);

				return new GQIPage(instances
					.Select(i => new TaskInstance(i))
					.Select(CreateRow)
					.ToArray());
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				_logger.Error(ex.StackTrace);
				throw;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "More readable this way")]
		private static GQIRow CreateRow(TaskInstance task)
		{
			return new GQIRow(Convert.ToString(task.ID.Id), new[]
			{
				new GQICell { Value = task.TaskProperties.ID, },
				new GQICell { Value = task.TaskProperties.Title, },
				new GQICell { Value = task.TaskProperties.Description, },
				new GQICell { Value = (int)task.TaskProperties.Status, DisplayValue = Enum.GetName(typeof(ExerciseTasksIds.Enums.Status), task.TaskProperties.Status), },
				new GQICell { Value = task.TaskProperties.Assignee, },
				new GQICell { Value = task.TaskProperties.Customer, },
			})
			{
				Metadata = new GenIfRowMetadata(new[]
				{
					new ObjectRefMetadata
					{
						Object = task.ID,
					},
				}),
			};
		}
	}
}
