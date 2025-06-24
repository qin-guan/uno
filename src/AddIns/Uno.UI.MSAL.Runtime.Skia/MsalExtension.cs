using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Uno.Foundation.Extensibility;
using Uno.UI.MSAL.Extensibility;

namespace Uno.UI.MSAL.Runtime.Skia;

[EditorBrowsable(EditorBrowsableState.Never)]
public class MsalExtension : IMsalExtension
{
	public void InitializeAbstractApplicationBuilder<T>(T builder) where T : Microsoft.Identity.Client.AbstractApplicationBuilder<T>
	{
#if __ANDROID__
		(builder as PublicClientApplicationBuilder)?.WithParentActivityOrWindow(() => ContextHelper.Current as Android.App.Activity);
#elif __APPLE_UIKIT__
#pragma warning disable CA1422 // Validate platform compatibility
		(builder as PublicClientApplicationBuilder)?.WithParentActivityOrWindow(() => UIKit.UIApplication.SharedApplication?.KeyWindow?.RootViewController);
#pragma warning restore CA1422 // Validate platform compatibility
#elif __WASM__
		builder.WithHttpClientFactory(WasmHttpFactory.Instance);
#endif
	}
	public void InitializeAcquireTokenInteractiveParameterBuilder(Microsoft.Identity.Client.AcquireTokenInteractiveParameterBuilder builder)
	{
#if __WASM__
		builder.WithCustomWebUi(WasmWebUi.Instance);
#endif
	}
}
