using Autofac;

namespace ServiceInsight.CustomViewer.Plugin
{
	#region  IoCModule
    public class CustomViewerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyCustomDecryptionViewModel>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<MyCustomDecryptionView>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<MessageEncryptor>().AsImplementedInterfaces().AsSelf().SingleInstance();
        }
    }
	#endregion
}