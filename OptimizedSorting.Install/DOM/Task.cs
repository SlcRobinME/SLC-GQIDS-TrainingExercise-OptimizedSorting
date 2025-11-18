namespace Skyline.DataMiner.Exercise.OptimizedSorting.Install.DOM
{
	using System;
	using System.Collections.Generic;

	using DomHelpers.ExerciseTasks;
	using Skyline.DataMiner.Exercise.OptimizedSorting.Shared.DOM;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.GenericEnums;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Builders;

	internal partial class DomInstaller
	{
		private void InstallTask(DomHelper helper)
		{
			var taskProperties = new SectionDefinitionBuilder()
				.WithID(TaskDomMapper.TaskProperties.SectionDefinitionId)
				.WithName("Task Properties")
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(TaskDomMapper.TaskProperties.TaskID)
					.WithName("Task ID")
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The unique identifier for the task."))

				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(TaskDomMapper.TaskProperties.Title)
					.WithName(nameof(TaskDomMapper.TaskProperties.Title))
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The title of the task."))

				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(TaskDomMapper.TaskProperties.Description)
					.WithName(nameof(TaskDomMapper.TaskProperties.Description))
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The description of the task."))

				.AddFieldDescriptor(new GenericEnumFieldDescriptorBuilder()
					.WithID(TaskDomMapper.TaskProperties.Status)
					.WithName(nameof(TaskDomMapper.TaskProperties.Status))
					.WithEnumType(GenericEnumFieldDescriptorBuilder.EnumType.Int)
					.WithIsOptional(true)
					.AddEnumValue(new GenericEnumEntry<int>("Not Started", 1))
					.AddEnumValue(new GenericEnumEntry<int>("In Progress", 2))
					.AddEnumValue(new GenericEnumEntry<int>("Code Review", 3))
					.AddEnumValue(new GenericEnumEntry<int>("Quality Assurance", 4))
					.AddEnumValue(new GenericEnumEntry<int>("Completed", 5))
					.WithTooltip("The unique identifier for the task."))

				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(TaskDomMapper.TaskProperties.Assignee)
					.WithName(nameof(TaskDomMapper.TaskProperties.Assignee))
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The person who is assigned to the task."))

				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(TaskDomMapper.TaskProperties.Customer)
					.WithName(nameof(TaskDomMapper.TaskProperties.Customer))
					.WithType(typeof(string))
					.WithIsOptional(true)
					.WithTooltip("The customer for who the task is being performed."))
				.Build();

			Import(helper.SectionDefinitions, SectionDefinitionExposers.ID.Equal(TaskDomMapper.TaskProperties.SectionDefinitionId), taskProperties);
			Log("Imported the 'TaskProperties' section definition.");

			var task = new DomDefinitionBuilder()
				.WithID(TaskDomMapper.DomDefinitionId)
				.WithName("Task")
				.AddSectionDefinitionLink(new Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions.SectionDefinitionLink
				{
					SectionDefinitionID = TaskDomMapper.TaskProperties.SectionDefinitionId,
					IsOptional = true,
					IsSoftDeleted = false,
					AllowMultipleSections = false,
				})
				.Build();

			Import(helper.DomDefinitions, DomDefinitionExposers.Id.Equal(TaskDomMapper.DomDefinitionId), task);
			Log("Imported the 'Task' DOM definition.");
		}

		private void CreateDummyTasks(DomHelper helper)
		{
			Log("Generating 50 task instances...");

			var taskData = new[]
			{
				new { Guid = new Guid("87802635-2e0a-78e0-5466-72fa47dae029"), ID = "TASK-1000", Title = "Implement user authentication", Description = "High priority task requiring immediate attention", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "John Smith", Customer = "Acme Corp" },
				new { Guid = new Guid("07aefe76-984c-b707-7211-6d9c3fa894f6"), ID = "TASK-1001", Title = "Fix database connection issue", Description = "Critical bug affecting production environment", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Sarah Johnson", Customer = "TechStart Inc" },
				new { Guid = new Guid("81aabb8b-ab16-e2e0-d835-0f519e74b168"), ID = "TASK-1002", Title = "Update API documentation", Description = "Documentation update required", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Michael Brown", Customer = "Global Solutions Ltd" },
				new { Guid = new Guid("eed11f33-07f9-9dfc-1fc8-14f8c14438ab"), ID = "TASK-1003", Title = "Refactor legacy code", Description = "Technical debt that needs to be addressed", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Emma Wilson", Customer = "Innovation Labs" },
				new { Guid = new Guid("8b6e7cd2-5f30-cb12-1ecb-2e1789b519fe"), ID = "TASK-1004", Title = "Add unit tests", Description = "New feature implementation", Status = ExerciseTasksIds.Enums.Status.CodeReview, Assignee = "David Lee", Customer = "Enterprise Systems" },
				new { Guid = new Guid("20680460-a6d7-84fc-bf0c-614d97e72193"), ID = "TASK-1005", Title = "Optimize query performance", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Lisa Garcia", Customer = "Digital Ventures" },
				new { Guid = new Guid("217f63fc-2505-d438-0c05-cf2ed8e41469"), ID = "TASK-1006", Title = "Design new dashboard", Description = "Enhancement requested by customer", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Robert Martinez", Customer = "Cloud Services Co" },
				new { Guid = new Guid("bc9715ae-18dc-2bd3-7fea-cc23fdc43436"), ID = "TASK-1007", Title = "Integrate payment gateway", Description = "High priority task requiring immediate attention", Status = ExerciseTasksIds.Enums.Status.QualityAssurance, Assignee = "Jennifer Davis", Customer = "Data Analytics Inc" },
				new { Guid = new Guid("251b9baa-f836-3bf9-cb94-13d1d6ceabda"), ID = "TASK-1008", Title = "Security audit", Description = "Security vulnerability fix", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "William Rodriguez", Customer = "Software Partners" },
				new { Guid = new Guid("819ff8f8-644f-e3b7-8f53-a21b5814d648"), ID = "TASK-1009", Title = "Migrate to cloud infrastructure", Description = "Platform upgrade activity", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Jessica Taylor", Customer = "Tech Innovations" },
				new { Guid = new Guid("3d8498d1-eec9-b633-1c5c-f148050254b1"), ID = "TASK-1010", Title = "Create REST endpoints", Description = "New feature implementation", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "John Smith", Customer = "Acme Corp" },
				new { Guid = new Guid("052bf13e-b225-45e7-e338-95e256dde2b7"), ID = "TASK-1011", Title = "Setup CI/CD pipeline", Description = "Routine maintenance task", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Sarah Johnson", Customer = "TechStart Inc" },
				new { Guid = new Guid("3effcbbd-6e4a-0911-c5e8-65f3736fde8d"), ID = "TASK-1012", Title = "Implement caching layer", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.CodeReview, Assignee = "Michael Brown", Customer = "Global Solutions Ltd" },
				new { Guid = new Guid("66ddf0ff-d1e9-1f22-b13e-59915950603e"), ID = "TASK-1013", Title = "Bug fix - login error", Description = "Critical bug affecting production environment", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Emma Wilson", Customer = "Innovation Labs" },
				new { Guid = new Guid("398a3979-1602-d066-e021-403c4cf3d274"), ID = "TASK-1014", Title = "Add logging framework", Description = "Enhancement requested by customer", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "David Lee", Customer = "Enterprise Systems" },
				new { Guid = new Guid("65e30859-7b00-ebcd-a8e0-8c0c1480b074"), ID = "TASK-1015", Title = "Update dependencies", Description = "Routine maintenance task", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Lisa Garcia", Customer = "Digital Ventures" },
				new { Guid = new Guid("39792788-a4e4-e88b-890c-86c891e6d2bd"), ID = "TASK-1016", Title = "Code review session", Description = "Technical debt that needs to be addressed", Status = ExerciseTasksIds.Enums.Status.QualityAssurance, Assignee = "Robert Martinez", Customer = "Cloud Services Co" },
				new { Guid = new Guid("5037e56b-fb6c-fd3d-6c9f-91218a5abb5f"), ID = "TASK-1017", Title = "Performance testing", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Jennifer Davis", Customer = "Data Analytics Inc" },
				new { Guid = new Guid("25b4cb70-987a-b48f-7dfb-dbf3f5055ac2"), ID = "TASK-1018", Title = "Database schema migration", Description = "Platform upgrade activity", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "William Rodriguez", Customer = "Software Partners" },
				new { Guid = new Guid("2fa0ce0e-5f49-8c08-e97b-cd4513695e2e"), ID = "TASK-1019", Title = "Setup monitoring system", Description = "High priority task requiring immediate attention", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Jessica Taylor", Customer = "Tech Innovations" },
				new { Guid = new Guid("c6572751-ac31-d343-5582-d3711f4a0866"), ID = "TASK-1020", Title = "Implement user authentication", Description = "Security vulnerability fix", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "John Smith", Customer = "Acme Corp" },
				new { Guid = new Guid("bb13cf45-8cba-1116-c327-3cfecb4aa114"), ID = "TASK-1021", Title = "Fix database connection issue", Description = "Critical bug affecting production environment", Status = ExerciseTasksIds.Enums.Status.CodeReview, Assignee = "Sarah Johnson", Customer = "TechStart Inc" },
				new { Guid = new Guid("cda2ac9c-93ae-6191-8acc-d9c68c966d9f"), ID = "TASK-1022", Title = "Update API documentation", Description = "Documentation update required", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Michael Brown", Customer = "Global Solutions Ltd" },
				new { Guid = new Guid("43006673-a255-c473-1146-03097045070a"), ID = "TASK-1023", Title = "Refactor legacy code", Description = "Technical debt that needs to be addressed", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Emma Wilson", Customer = "Innovation Labs" },
				new { Guid = new Guid("72d4bfe0-3ba7-b1b8-3eb1-30edb34f852c"), ID = "TASK-1024", Title = "Add unit tests", Description = "New feature implementation", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "David Lee", Customer = "Enterprise Systems" },
				new { Guid = new Guid("658a9cba-5e4a-fabe-38dd-bc689087f144"), ID = "TASK-1025", Title = "Optimize query performance", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.QualityAssurance, Assignee = "Lisa Garcia", Customer = "Digital Ventures" },
				new { Guid = new Guid("cce6cec5-fac2-38f7-ad4e-05a4718511c2"), ID = "TASK-1026", Title = "Design new dashboard", Description = "Enhancement requested by customer", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Robert Martinez", Customer = "Cloud Services Co" },
				new { Guid = new Guid("d34ed6c0-a2ba-82c0-01da-3ac97f5020b6"), ID = "TASK-1027", Title = "Integrate payment gateway", Description = "High priority task requiring immediate attention", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Jennifer Davis", Customer = "Data Analytics Inc" },
				new { Guid = new Guid("e428ebc0-b868-c06a-a452-b7fb581a88b2"), ID = "TASK-1028", Title = "Security audit", Description = "Security vulnerability fix", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "William Rodriguez", Customer = "Software Partners" },
				new { Guid = new Guid("baa7f9cc-1d39-2289-0e64-cbe8c02eb95c"), ID = "TASK-1029", Title = "Migrate to cloud infrastructure", Description = "Platform upgrade activity", Status = ExerciseTasksIds.Enums.Status.CodeReview, Assignee = "Jessica Taylor", Customer = "Tech Innovations" },
				new { Guid = new Guid("85e34f2e-2c5d-0859-6c8e-2f68eac429e7"), ID = "TASK-1030", Title = "Create REST endpoints", Description = "New feature implementation", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "John Smith", Customer = "Acme Corp" },
				new { Guid = new Guid("85fd8751-66a9-a27d-6106-d604ed820076"), ID = "TASK-1031", Title = "Setup CI/CD pipeline", Description = "Routine maintenance task", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Sarah Johnson", Customer = "TechStart Inc" },
				new { Guid = new Guid("09cd5a99-0236-9f55-a09e-4eabee213749"), ID = "TASK-1032", Title = "Implement caching layer", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Michael Brown", Customer = "Global Solutions Ltd" },
				new { Guid = new Guid("3896e33a-3331-c7b3-d240-235756541ae9"), ID = "TASK-1033", Title = "Bug fix - login error", Description = "Critical bug affecting production environment", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Emma Wilson", Customer = "Innovation Labs" },
				new { Guid = new Guid("90cfb88d-1ff7-4fb0-315d-e50a18d1b440"), ID = "TASK-1034", Title = "Add logging framework", Description = "Enhancement requested by customer", Status = ExerciseTasksIds.Enums.Status.QualityAssurance, Assignee = "David Lee", Customer = "Enterprise Systems" },
				new { Guid = new Guid("09ca8cce-c7ec-6544-a705-37f5defe484b"), ID = "TASK-1035", Title = "Update dependencies", Description = "Routine maintenance task", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Lisa Garcia", Customer = "Digital Ventures" },
				new { Guid = new Guid("351d6484-a751-1a52-3346-06d34ea8af00"), ID = "TASK-1036", Title = "Code review session", Description = "Technical debt that needs to be addressed", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Robert Martinez", Customer = "Cloud Services Co" },
				new { Guid = new Guid("ca88e38b-108a-c6fd-746a-e8f1e8bb7133"), ID = "TASK-1037", Title = "Performance testing", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Jennifer Davis", Customer = "Data Analytics Inc" },
				new { Guid = new Guid("b4869914-7fb4-26ee-3aaf-d6a5c07116ff"), ID = "TASK-1038", Title = "Database schema migration", Description = "Platform upgrade activity", Status = ExerciseTasksIds.Enums.Status.CodeReview, Assignee = "William Rodriguez", Customer = "Software Partners" },
				new { Guid = new Guid("fa693756-7e75-73c1-6656-c1e7bf603278"), ID = "TASK-1039", Title = "Setup monitoring system", Description = "High priority task requiring immediate attention", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Jessica Taylor", Customer = "Tech Innovations" },
				new { Guid = new Guid("b5bfd3d5-fb22-0c4b-f462-8fda3a580503"), ID = "TASK-1040", Title = "Implement user authentication", Description = "Security vulnerability fix", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "John Smith", Customer = "Acme Corp" },
				new { Guid = new Guid("b09be953-7651-96bd-d712-4d5ebe0667da"), ID = "TASK-1041", Title = "Fix database connection issue", Description = "Critical bug affecting production environment", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Sarah Johnson", Customer = "TechStart Inc" },
				new { Guid = new Guid("edcf5e7d-60b1-5bdc-ec5d-0613eee7390d"), ID = "TASK-1042", Title = "Update API documentation", Description = "Documentation update required", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "Michael Brown", Customer = "Global Solutions Ltd" },
				new { Guid = new Guid("d594be51-81c4-5f26-1529-de0faded4dee"), ID = "TASK-1043", Title = "Refactor legacy code", Description = "Technical debt that needs to be addressed", Status = ExerciseTasksIds.Enums.Status.QualityAssurance, Assignee = "Emma Wilson", Customer = "Innovation Labs" },
				new { Guid = new Guid("db5db5fa-1788-f351-0c40-c4f370aaa7f0"), ID = "TASK-1044", Title = "Add unit tests", Description = "New feature implementation", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "David Lee", Customer = "Enterprise Systems" },
				new { Guid = new Guid("000c40d9-9172-fb15-3770-d88882cdadff"), ID = "TASK-1045", Title = "Optimize query performance", Description = "Performance improvement initiative", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Lisa Garcia", Customer = "Digital Ventures" },
				new { Guid = new Guid("3818180f-5d89-3985-6ca0-c984bc32ef6b"), ID = "TASK-1046", Title = "Design new dashboard", Description = "Enhancement requested by customer", Status = ExerciseTasksIds.Enums.Status.NotStarted, Assignee = "Robert Martinez", Customer = "Cloud Services Co" },
				new { Guid = new Guid("6d36ed81-4de0-697b-a061-a4173fc8703e"), ID = "TASK-1047", Title = "Integrate payment gateway", Description = "High priority task requiring immediate attention", Status = ExerciseTasksIds.Enums.Status.CodeReview, Assignee = "Jennifer Davis", Customer = "Data Analytics Inc" },
				new { Guid = new Guid("57e4c592-6f91-7b66-bbd2-93d1c8903f04"), ID = "TASK-1048", Title = "Security audit", Description = "Security vulnerability fix", Status = ExerciseTasksIds.Enums.Status.InProgress, Assignee = "William Rodriguez", Customer = "Software Partners" },
				new { Guid = new Guid("94076d81-4096-c21e-9caf-9828b96dfaa4"), ID = "TASK-1049", Title = "Migrate to cloud infrastructure", Description = "Platform upgrade activity", Status = ExerciseTasksIds.Enums.Status.Completed, Assignee = "Jessica Taylor", Customer = "Tech Innovations" },
			};

			var tasks = new List<DomInstance>();
			foreach (var data in taskData)
			{
				var task = new TaskInstance(data.Guid);
				task.TaskProperties.TaskID = data.ID;
				task.TaskProperties.Title = data.Title;
				task.TaskProperties.Description = data.Description;
				task.TaskProperties.Status = data.Status;
				task.TaskProperties.Assignee = data.Assignee;
				task.TaskProperties.Customer = data.Customer;

				tasks.Add(task);
			}

			Log("Successfully generated 50 task instances.");

			helper.DomInstances.CreateOrUpdate(tasks);

			Log("Successfully imported 50 task instances.");
		}
	}
}
