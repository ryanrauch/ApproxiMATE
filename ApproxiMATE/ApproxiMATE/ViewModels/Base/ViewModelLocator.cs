using ApproxiMATE.Helpers;
using ApproxiMATE.Services;
using ApproxiMATE.Services.Interfaces;
using Autofac;
using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace ApproxiMATE.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static IContainer _container;

        //private static readonly ViewModelLocator _instance = new ViewModelLocator();

        //public static ViewModelLocator Instance
        //{
        //    get
        //    {
        //        return _instance;
        //    }
        //}

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<JwtRequestService>().As<IRequestService>().SingleInstance();
            builder.RegisterInstance(new HexagonalEquilateralScale()).As<IHexagonal>();
            builder.RegisterType<IssueViewModel>();
            builder.RegisterInstance(new MainMapViewModel());
            _container = builder.Build();

            //_container = new ContainerBuilder();
            //// View models - by default, TinyIoC will register concrete classes as multi-instance.
            //_container.RegisterInstance(BasketViewModel);
            //_container.Register<CatalogViewModel>();
            //// Services - by default, TinyIoC will register interface registrations as singletons.
            //_container.Register<INavigationService, NavigationService>();
            //_container.Register<IDialogService, DialogService>();
        }

        public static void UpdateDependencies(bool useMockServices)
        {
            // Change injected dependencies
            if (useMockServices)
            {
                //_container.Register<ICatalogService, CatalogMockService>();
                //_container.Register<IBasketService, BasketMockService>();
                //_container.Register<IOrderService, OrderMockService>();
                //_container.Register<IUserService, UserMockService>();
                //_container.Register<ICampaignService, CampaignMockService>();
                UseMockService = true;
            }
            else
            {
                //_container.Register<ICatalogService, CatalogService>();
                //_container.Register<IBasketService, BasketService>();
                //_container.Register<IOrderService, OrderService>();
                //_container.Register<IUserService, UserService>();
                //_container.Register<ICampaignService, CampaignService>();
                UseMockService = false;
            }
        }
        //public static object Resolve(Type type)
        //{
        //    return _container.Resolve(type);
        //}

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }

        //public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        //{
        //    _container.Register<TInterface, T>().AsSingleton();
        //}
    }
}
