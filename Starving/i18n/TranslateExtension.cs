using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Starving
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "Starving.i18n.AppResources";
        public IValueConverter Converter { get; set; }

        public TranslateExtension()
        {
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);

            /*CultureInfo ci;
            if (string.IsNullOrEmpty(Helpers.Settings.Language))
            {
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
            else if (Helpers.Settings.Language == "Spanish")
            {
                ci = new CultureInfo("es");
            }
            else
            {
                ci = new CultureInfo("en");
            }*/

            var ci = new CultureInfo("en");

            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name), "Text");
#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            if (Converter == null)
                return translation;
            var translationConverted = Converter.Convert(translation, typeof(FormattedString), null, CultureInfo.CurrentCulture);
            return translationConverted;
        }
    }

}

