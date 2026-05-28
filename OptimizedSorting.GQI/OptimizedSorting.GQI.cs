/*
****************************************************************************
*  Copyright (c),  Skyline Communications NV  All Rights Reserved.    *
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
	using Skyline.DataMiner.Analytics.GenericInterface.Operators;
	using Skyline.DataMiner.Exercise.OptimizedSorting.Shared.DOM;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;
	using SLDataGateway.API.Querying;
	using SLDataGateway.API.Types.Querying;

	[GQIMetaData(Name = "Exercise - Optimized Tasks")]
	public sealed class OptimizedSorting : IGQIDataSource, IGQIOnInit, IGQIOptimizableDataSource, IGQIOnPrepareFetch
	{
		// Mapping: FieldDescriptorID key -> GQIColumn
		private static readonly Dictionary<FieldDescriptorID, GQIColumn> _exposerToColumn = new Dictionary<FieldDescriptorID, GQIColumn>();

		private static readonly Dictionary<FieldDescriptorID, GQIColumn> _fieldToColumn;

		private static readonly List<GQIColumn> _columns = new List<GQIColumn>
		{
			new GQIStringColumn("ID"),
			new GQIStringColumn("Title"),
			new GQIStringColumn("Description"),
			new GQIIntColumn("Status"),
			new GQIStringColumn("Assignee"),
			new GQIStringColumn("Customer"),
		};

		// Mapping: GQIColumn -> FieldDescriptorID
		private static readonly Dictionary<GQIColumn, FieldDescriptorID> _columnToField = new Dictionary<GQIColumn, FieldDescriptorID>
		{
			{ _columns[0], TaskDomMapper.TaskProperties.TaskID },
			{ _columns[1], TaskDomMapper.TaskProperties.Title },
			{ _columns[2], TaskDomMapper.TaskProperties.Description },
			{ _columns[3], TaskDomMapper.TaskProperties.Status },
			{ _columns[4], TaskDomMapper.TaskProperties.Assignee },
			{ _columns[5], TaskDomMapper.TaskProperties.Customer },
		};

		private IGQISortOperator _sortOperator;
		private IGQILogger _logger;
		private DomHelper _helper;

		public OnInitOutputArgs OnInit(OnInitInputArgs args)
		{
			_logger = args.Logger;
			_helper = new DomHelper(args.DMS.SendMessages, TaskDomMapper.ModuleId);
			return default;
		}

		public GQIColumn[] GetColumns() => _columns.ToArray();

		public IGQIQueryNode Optimize(IGQIDataSourceNode currentNode, IGQICoreOperator nextOperator)
		{
			if (nextOperator.IsSortOperator(out var sortOperator))
			{
				_sortOperator = sortOperator;
				return currentNode;
			}

			return currentNode.Append(nextOperator);
		}

		public OnPrepareFetchOutputArgs OnPrepareFetch(OnPrepareFetchInputArgs args)
		{
			return default;
		}

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			try
			{
				var filter = DomInstanceExposers.DomDefinitionId.Equal(TaskDomMapper.DomDefinitionId.Id);
				IQuery<DomInstance> query = null;

				if (_sortOperator != null)
				{
					_logger.Information($"[OptimizedSorting] Sort operator detected with {_sortOperator.Fields.Count} field(s).");

					foreach (var sortField in _sortOperator.Fields)
					{
						var match = _columnToField.FirstOrDefault(kvp => sortField.Column.Equals(kvp.Key));

						if (match.Key == null)
						{
							_logger.Warning("[OptimizedSorting] Column not found in mapping, skipping.");
							continue;
						}

						_logger.Information($"[OptimizedSorting] Applying sort on FieldDescriptorID: {match.Value.Id}");

						var field = DomInstanceExposers.FieldValues.DomInstanceField(match.Value);

						query = sortField.Direction == GQISortDirection.Descending
							? filter.OrderByDescending(field)
							: filter.OrderBy(field);
					}
				}

				var instances = query != null
					? _helper.DomInstances.Read(query)
					: _helper.DomInstances.Read(filter);
				_logger.Information($"[OptimizedSorting] Fetched {instances.Count()} instances from database.");

				return new GQIPage(instances
					.Select(i => new TaskInstance(i))
					.Select(CreateRow)
					.ToArray());
			}
			catch (Exception ex)
			{
				_logger.Error($"[OptimizedSorting] Exception: {ex.Message}");
				_logger.Error($"[OptimizedSorting] StackTrace: {ex.StackTrace}");
				throw;
			}
		}

		private static GQIRow CreateRow(TaskInstance task)
		{
			return new GQIRow(Convert.ToString(task.ID.Id), new[]
			{
				new GQICell { Value = task.TaskProperties.TaskID },
				new GQICell { Value = task.TaskProperties.Title },
				new GQICell { Value = task.TaskProperties.Description },
				new GQICell { Value = (int)task.TaskProperties.Status, DisplayValue = Enum.GetName(typeof(ExerciseTasksIds.Enums.Status), task.TaskProperties.Status) },
				new GQICell { Value = task.TaskProperties.Assignee },
				new GQICell { Value = task.TaskProperties.Customer },
			})
			{
				Metadata = new GenIfRowMetadata(new[]
				{
					new ObjectRefMetadata { Object = task.ID },
				}),
			};
		}
	}
}