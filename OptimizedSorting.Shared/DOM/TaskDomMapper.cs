namespace Skyline.DataMiner.Exercise.OptimizedSorting.Shared.DOM
{
	using System;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;

	internal static class TaskDomMapper
	{
		internal const string ModuleId = "(exercise)tasks";
		internal static DomDefinitionId DomDefinitionId = new DomDefinitionId(new Guid("cc05e867-4d37-41d2-b8e5-858107d341bb"))
		{
			ModuleId = ModuleId,
		};

		internal static class TaskProperties
		{
			internal static SectionDefinitionID SectionDefinitionId = new SectionDefinitionID(new Guid("81d78c00-318e-4dcf-a3ae-2d43955a83ed"))
			{
				ModuleId = ModuleId,
			};

			internal static FieldDescriptorID ID = new FieldDescriptorID(new Guid("e9f79ec7-7592-4d18-b05c-fb0c30d54ba0"));
			internal static FieldDescriptorID Title = new FieldDescriptorID(new Guid("8598f620-f223-4f09-8d0a-dc3d601aa4d7"));
			internal static FieldDescriptorID Description = new FieldDescriptorID(new Guid("5eb6a517-8783-42ca-8a60-be186d6c4f8f"));
			internal static FieldDescriptorID Status = new FieldDescriptorID(new Guid("40c176ad-9335-4047-9dd5-5b27e55ed1f8"));
			internal static FieldDescriptorID Assignee = new FieldDescriptorID(new Guid("ab5d9602-0bac-419b-b8c9-61154f757a00"));
			internal static FieldDescriptorID Customer = new FieldDescriptorID(new Guid("2c77e8b6-ef67-483b-99e8-9eca561811ef"));
		}
	}
}
