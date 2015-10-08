using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BikeR.WPApp.DataModel.valueconverter
{

    //[ValueConversion(typeof(string), typeof(LinearGradientBrush))]
    public class TagStatusKindValueConverter: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            NfcField nfcField = (NfcField)value;


            string resourceName = string.Format("linear_{0}_{1}", nfcField.TagStatusEnum.ToString().ToLower(), nfcField.TagKindEnum.ToString().ToLower());


            var res = Application.Current.Resources[resourceName];


            return res;
             //return (FrameElem.TryFindResource("OKStatus") as Brush);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
