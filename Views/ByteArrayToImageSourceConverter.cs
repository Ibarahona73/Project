using System;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Controls;

namespace Project.Views
{
	public class ByteArrayToImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is byte[] bytes)
			{
				var imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
				return imageSource;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
