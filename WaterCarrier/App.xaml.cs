using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Windows;
using WaterCarrier.Application.Interfaces;
using WaterCarrier.Infrastructure.Persistence.Entities;
using WaterCarrier.Infrastructure.Repositories;
using WaterCarrier.Services;
using WaterCarrier.ViewModels;

namespace WaterCarrier;

public partial class App : System.Windows.Application
{
    public IServiceProvider Services { get; }

    public new static App Current => (App)System.Windows.Application.Current;

    public App()
    {
        Services = ConfigureServices();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = new MainWindow
        {
            DataContext = Services.GetRequiredService<MainViewModel>()
        };
        mainWindow.Show();
    }

    /// <summary>
    /// Конфигурирует сервисы для приложения.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // 1. NHibernate SessionFactory
        services.AddSingleton<ISessionFactory>(provider =>
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(EmployeeEntity).Assembly);
            return configuration.BuildSessionFactory();
        });

        // 2. NHibernate Session 
        services.AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());

        // 3. Репозитории 
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // 4. Services
        services.AddSingleton<IDialogService, DialogService>();

        // 5. ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddTransient<EmployeeViewModel>(); 
        services.AddTransient<CounterpartyViewModel>();
        services.AddTransient<OrderViewModel>();
        services.AddTransient<EmployeeEditorViewModel>();
        services.AddTransient<CounterpartyEditorViewModel>();
        services.AddTransient<OrderEditorViewModel>();

        return services.BuildServiceProvider();
    }
} 