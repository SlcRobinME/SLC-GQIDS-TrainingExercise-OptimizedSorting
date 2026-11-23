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
	using SLDataGateway.API.Querying;
	using SLDataGateway.API.Types.Querying;

	[GQIMetaData(Name = "Exercise - Optimized Tasks")]
	public sealed class OptimizedSorting : IGQIDataSource, IGQIOnInit, IGQIOptimizableDataSource, IGQIOnPrepareFetch
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

		private static readonly Dictionary<string, FieldExposer> _exposers = new Dictionary<string, FieldExposer>
		{
			["ID"] = DomInstanceExposers.FieldValues.DomInstanceField(TaskDomMapper.TaskProperties.TaskID),
			["Title"] = DomInstanceExposers.FieldValues.DomInstanceField(TaskDomMapper.TaskProperties.Title),
			["Description"] = DomInstanceExposers.FieldValues.DomInstanceField(TaskDomMapper.TaskProperties.Description),
			["Status"] = DomInstanceExposers.FieldValues.DomInstanceField(TaskDomMapper.TaskProperties.Status),
			["Assignee"] = DomInstanceExposers.FieldValues.DomInstanceField(TaskDomMapper.TaskProperties.Assignee),
			["Customer"] = DomInstanceExposers.FieldValues.DomInstanceField(TaskDomMapper.TaskProperties.Customer),
		};

		private IGQILogger _logger;
		private DomHelper _helper;
		private IGQISortOperator _sortOperator;
		private IQuery<DomInstance> _query;

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
				var instances = _helper.DomInstances.Read(_query);

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

		public IGQIQueryNode Optimize(IGQIDataSourceNode currentNode, IGQICoreOperator nextOperator)
		{
			if (nextOperator is IGQISortOperator sortOperator && sortOperator.Fields.All(field => TryGetExposer(field.Column, out _)))
			{
				_sortOperator = sortOperator;
				return currentNode;
			}

			return currentNode.Append(nextOperator);
		}

		public OnPrepareFetchOutputArgs OnPrepareFetch(OnPrepareFetchInputArgs args)
		{
			FilterElement<DomInstance> filter = DomInstanceExposers.DomDefinitionId.Equal(TaskDomMapper.DomDefinitionId.Id);
			IQuery<DomInstance> query = filter.ToQuery();

			if (_sortOperator != null)
			{
				for (int i = 0; i < _sortOperator.Fields.Count; i++)
				{
					IGQISortField field = _sortOperator.Fields[i];
					TryGetExposer(field.Column, out FieldExposer exposer);
					bool descending = field.Direction == GQISortDirection.Descending;

					if (i == 0)
					{
						query = descending ? query.OrderByDescending(exposer) : query.OrderBy(exposer);
					}
					else
					{
						query = descending ? query.ThenByDescending(exposer) : query.ThenBy(exposer);
					}
				}
			}

			_query = query;
			return default;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "More readable this way")]
		private static GQIRow CreateRow(TaskInstance task)
		{
			return new GQIRow(Convert.ToString(task.ID.Id), new[]
			{
				new GQICell { Value = task.TaskProperties.TaskID, },
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

		private static bool TryGetExposer(IGQIColumn column, out FieldExposer exposer)
		{
			exposer = null;
			return column is GQIColumn gqiColumn && _exposers.TryGetValue(gqiColumn.Name, out exposer);
		}
	}
}
