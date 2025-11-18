namespace Skyline.DataMiner.Exercise.OptimizedSorting.Install.DOM
{
	using System;
	using System.Linq;

	using Skyline.AppInstaller;
	using Skyline.DataMiner.Exercise.OptimizedSorting.Shared.DOM;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.AppPackages;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.Modules;
	using Skyline.DataMiner.Net.ManagerStore;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Utils.DOM.Builders;

	internal partial class DomInstaller
	{
		private readonly IConnection _connection;

		private readonly Action<string> _logMethod;

		public DomInstaller(IConnection connection, AppInstallContext context)
		{
			_connection = connection ?? throw new ArgumentNullException(nameof(connection));
			_logMethod = new AppInstaller(connection, context).Log;
		}

		public void InstallDefaultContent()
		{
			Log("Installation for Manillen started...");

			var moduleHelper = new ModuleSettingsHelper(_connection.HandleMessages);
			var moduleExist = moduleHelper.ModuleSettings.Count(ModuleSettingsExposers.ModuleId.Equal(TaskDomMapper.ModuleId)) == 0;
			if (!moduleExist)
			{
				Log("Installing Module Settings...");
			}
			else
			{
				Log("Updating Module Settings...");
			}

			var module = new DomModuleBuilder()
				.WithModuleId(TaskDomMapper.ModuleId)
				.WithInformationEvents(false)
				.WithHistory(false)
				.Build();

			Import(moduleHelper.ModuleSettings, ModuleSettingsExposers.ModuleId.Equal(TaskDomMapper.ModuleId), module);

			if (!moduleExist)
			{
				Log("Installed Module Settings");
			}
			else
			{
				Log("Updated Module Settings");
			}

			var domHelper = new DomHelper(_connection.HandleMessages, TaskDomMapper.ModuleId);
			InstallTask(domHelper);
			CreateDummyTasks(domHelper);
		}

		private void Log(string message)
		{
			_logMethod?.Invoke($"[DOM.Installer.OptimizedSorting] {message}");
		}

		private static void Import<T>(ICrudHelperComponent<T> crudHelperComponent, FilterElement<T> equalityFilter, T dataType)
			where T : DataType
		{
			bool exists = crudHelperComponent.Read(equalityFilter).Any();

			if (exists)
			{
				crudHelperComponent.Update(dataType);
			}
			else
			{
				crudHelperComponent.Create(dataType);
			}
		}
	}
}
